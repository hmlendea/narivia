using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Narivia.BusinessLogic.GameManagers.Interfaces;
using Narivia.BusinessLogic.Mapping;
using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.DataAccess.Repositories;
using Narivia.Infrastructure.Extensions;
using Narivia.Infrastructure.Helpers;
using Narivia.Models;

namespace Narivia.BusinessLogic.GameManagers
{
    public class GameDomainService : IGameDomainService
    {
        Dictionary<string, Biome> biomes;
        Dictionary<string, Culture> cultures;
        Dictionary<string, Faction> factions;
        Dictionary<string, Holding> holdings;
        Dictionary<string, Region> regions;
        Dictionary<string, Resource> resources;
        Dictionary<string, Unit> units;
        Dictionary<Tuple<string, string>, Army> armies;
        Dictionary<Tuple<string, string>, Border> borders;

        World world;

        string[,] worldTiles;
        string[,] biomeMap;

        string playerFactionId;
        int turn;

        public string[,] WorldTiles
        {
            get { return worldTiles; }
            set { worldTiles = value; }
        }

        public string[,] BiomeMap
        {
            get { return biomeMap; }
            set { biomeMap = value; }
        }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        /// <param name="worldId">World identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        public void NewGame(string worldId, string factionId)
        {
            LoadEntities(worldId);
            LoadWorld(worldId);

            InitializeGame(factionId);
            InitializeEntities();
        }

        /// <summary>
        /// Advances the game by one turn.
        /// </summary>
        public void NextTurn()
        {
            List<Faction> factionList = factions.Values.ToList();

            foreach (Faction faction in factionList)
            {
                // TODO: Process AI
                // Economy
                // Build
                // Recruit

                string targetRegionId = ChooseAttack(faction.Id);
                AttackRegion(faction.Id, targetRegionId);
            }

            turn += 1;
        }

        /// <summary>
        /// Transfers the specified region to the specified faction.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        public void TransferRegion(string regionId, string factionId)
        {
            Region region = regions[regionId];
            region.FactionId = factionId;
        }

        /// <summary>
        /// Checks wether the specified regions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified regions share a border, <c>false</c> otherwise.</returns>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        public bool RegionHasBorder(string region1Id, string region2Id)
        {
            return borders.Values.Any(x => (x.Region1Id == region1Id && x.Region2Id == region2Id) ||
                                           (x.Region1Id == region2Id && x.Region2Id == region1Id));
        }

        public List<Biome> GetAllBiomes()
        {
            List<Biome> biomeList = biomes.Values.ToList();

            return biomeList;
        }

        void LoadEntities(string worldId)
        {
            IBiomeRepository biomeRepository = new BiomeRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "biomes.xml"));
            ICultureRepository cultureRepository = new CultureRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "cultures.xml"));
            IFactionRepository factionRepository = new FactionRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "factions.xml"));
            IHoldingRepository holdingRepository = new HoldingRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "holdings.xml"));
            IRegionRepository regionRepository = new RegionRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "regions.xml"));
            IResourceRepository resourceRepository = new ResourceRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "resources.xml"));
            IUnitRepository unitRepository = new UnitRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "units.xml"));

            List<Biome> biomeList = biomeRepository.GetAll().ToDomainModels().ToList();
            List<Culture> cultureList = cultureRepository.GetAll().ToDomainModels().ToList();
            List<Faction> factionList = factionRepository.GetAll().ToDomainModels().ToList();
            List<Holding> holdingList = holdingRepository.GetAll().ToDomainModels().ToList();
            List<Region> regionList = regionRepository.GetAll().ToDomainModels().ToList();
            List<Resource> resourceList = resourceRepository.GetAll().ToDomainModels().ToList();
            List<Unit> unitList = unitRepository.GetAll().ToDomainModels().ToList();

            biomes = new Dictionary<string, Biome>();
            cultures = new Dictionary<string, Culture>();
            factions = new Dictionary<string, Faction>();
            holdings = new Dictionary<string, Holding>();
            regions = new Dictionary<string, Region>();
            resources = new Dictionary<string, Resource>();
            units = new Dictionary<string, Unit>();

            biomeList.ForEach(biome => biomes.Add(biome.Id, biome));
            cultureList.ForEach(culture => cultures.Add(culture.Id, culture));
            factionList.ForEach(faction => factions.Add(faction.Id, faction));
            holdingList.ForEach(holding => holdings.Add(holding.Id, holding));
            regionList.ForEach(region => regions.Add(region.Id, region));
            resourceList.ForEach(resource => resources.Add(resource.Id, resource));
            unitList.ForEach(unit => units.Add(unit.Id, unit));
        }

        void LoadWorld(string worldId)
        {
            armies = new Dictionary<Tuple<string, string>, Army>();
            borders = new Dictionary<Tuple<string, string>, Border>();

            LoadMap(worldId);
            LoadBorders();
        }

        void LoadMap(string worldId)
        {
            Dictionary<int, string> regionColourIds = new Dictionary<int, string>();
            Dictionary<int, string> biomeColourIds = new Dictionary<int, string>();

            XmlSerializer xs = new XmlSerializer(typeof(World));

            using (FileStream fs = new FileStream(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "world.xml"), FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                world = (World)xs.Deserialize(sr);
            }

            worldTiles = new string[world.Width, world.Height];
            biomeMap = new string[world.Width, world.Height];

            // Mapping the colours
            regions.Values.ToList().ForEach(region => regionColourIds.Register(ColourTranslator.ToArgb(region.Colour), region.Id));
            biomes.Values.ToList().ForEach(biome => biomeColourIds.Register(ColourTranslator.ToArgb(biome.Colour), biome.Id));

            // Reading the map pixel by pixel
            using (FastBitmap bmp = new FastBitmap(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "map.png")))
            {
                for (int y = 0; y < world.Width; y++)
                {
                    for (int x = 0; x < world.Height; x++)
                    {
                        int colour = bmp.GetPixel(x, y).ToArgb();

                        worldTiles[x, y] = regionColourIds[colour];
                    }
                }
            }

            // Reading the biome map pixel by pixel
            using (FastBitmap bmp = new FastBitmap(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "biomes_map.png")))
            {
                for (int y = 0; y < world.Width; y++)
                {
                    for (int x = 0; x < world.Height; x++)
                    {
                        int colour = bmp.GetPixel(x, y).ToArgb();

                        biomeMap[x, y] = biomeColourIds[colour];
                    }
                }
            }
        }

        void InitializeGame(string factionId)
        {
            playerFactionId = factionId;
            turn = 0;
        }

        void InitializeEntities()
        {
            foreach (Faction faction in factions.Values.ToList())
            {
                // TODO: set starting wealth
                faction.Wealth = 0;

                foreach (Unit unit in units.Values.ToList())
                {
                    Tuple<string, string> armyKey = new Tuple<string, string>(faction.Id, unit.Id);
                    Army army = new Army
                    {
                        FactionId = faction.Id,
                        UnitId = unit.Id,
                        Size = 0
                    };

                    armies.Add(armyKey, army);
                }
            }
        }

        void SetBorder(string region1Id, string region2Id)
        {
            if (RegionHasBorder(region1Id, region2Id))
            {
                return;
            }

            Tuple<string, string> borderKey = new Tuple<string, string>(region1Id, region2Id);
            Border border = new Border
            {
                Region1Id = region1Id,
                Region2Id = region2Id
            };

            borders.Add(borderKey, border);
        }

        void LoadBorders()
        {
            for (int x = 0; x < world.Width; x += 5)
            {
                for (int y = 0; y < world.Height; y += 5)
                {
                    List<string> region2IdVisited = new List<string>();
                    string region1Id = worldTiles[x, y];

                    for (int dx = -2; dx <= 2; dx++)
                    {
                        if (x + dx < 0 || x + dx >= world.Width)
                        {
                            continue;
                        }

                        for (int dy = -2; dy <= 2; dy++)
                        {
                            if (y + dy < 0 || y + dy >= world.Height)
                            {
                                continue;
                            }

                            string region2Id = worldTiles[x + dx, y + dy];

                            if (!region2IdVisited.Contains(region2Id) &&
                                region1Id != region2Id)
                            {
                                SetBorder(region1Id, region2Id);
                                region2IdVisited.Add(region2Id);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// WIP blitzkrieg sequencial algorithm for invading a faction.
        /// </summary>
        /// <returns>The region identifier.</returns>
        /// <param name="factionId">Attacking faction identifier.</param>
        /// <param name="targetFactionId">Targeted faction identifier.</param>
        /// <summary>
        public string Blitzkrieg_Seq(string factionId, string targetFactionId)
        {
            Random random = new Random();
            List<string> regionsOwnedIds = regions.Values.Where(x => x.FactionId == factionId).Select(x => x.Id).ToList();
            var bords = borders.Values;

            int pointsForSovereignty = 30;
            int pointsForHoldingCastle = 30;
            int pointsForHoldingCity = 20;
            int pointsForHoldingTemple = 10;
            int pointsForBorder = 15;
            int pointsForResourceEconomic = 5;
            int pointsForResourceMilitary = 10;


            Dictionary<string, int> targets = regions.Values.Where(x => x.FactionId == targetFactionId)
                                                     .Select(x => x.Id)
                                                     .Except(regionsOwnedIds)
                                                     .Where(x => regionsOwnedIds.Any(y => RegionHasBorder(x, y)))
                                                     .ToDictionary(x => x, y => 0);

            foreach (Region region in regions.Values.Where(x => targets.ContainsKey(x.Id)).ToList())
            {
                if (region.SovereignFactionId == factionId)
                {
                    targets[region.Id] += pointsForSovereignty;
                }


                foreach (Holding holding in holdings.Values.Where(x => x.RegionId == region.Id).ToList())
                {
                    switch (holding.Type)
                    {
                        case HoldingType.Castle:
                            targets[region.Id] += pointsForHoldingCastle;
                            break;

                        case HoldingType.City:
                            targets[region.Id] += pointsForHoldingCity;
                            break;

                        case HoldingType.Temple:
                            targets[region.Id] += pointsForHoldingTemple;
                            break;
                    }
                }

                Resource regionResource = resources.Values.FirstOrDefault(x => x.Id == region.ResourceId);

                if (regionResource != null)
                {
                    switch (regionResource.Type)
                    {
                        case ResourceType.Military:
                            targets[region.Id] += pointsForResourceMilitary;
                            break;

                        case ResourceType.Wealth:
                            targets[region.Id] += pointsForResourceEconomic;
                            break;
                    }
                }

                targets[region.Id] += regionsOwnedIds.Count(x => RegionHasBorder(x, region.Id)) * pointsForBorder;
            }

            if (targets.Count == 0)
            {
                return null;
            }

            int maxScore = targets.Max(x => x.Value);
            List<string> topTargets = targets.Keys.Where(x => targets[x] == maxScore).ToList();
            string regionId = topTargets[random.Next(0, topTargets.Count())];

            TransferRegion(regionId, factionId);

            return regionId;
        }

        /// <summary>
        /// WIP blitzkrieg parallelized algorithm for invading a faction.
        /// </summary>
        /// <returns>The region identifier.</returns>
        /// <param name="factionId">Attacking faction identifier.</param>
        /// <param name="targetFactionId">Targeted faction identifier.</param>
        /// <summary>
        public string Blitzkrieg_Parallel(string factionId, string targetFactionId)
        {
            Random random = new Random();
            List<string> regionsOwnedIds = regions.Values.Where(x => x.FactionId == factionId).Select(x => x.Id).ToList();
            var bords = borders.Values;

            int pointsForSovereignty = 30;
            int pointsForHoldingCastle = 30;
            int pointsForHoldingCity = 20;
            int pointsForHoldingTemple = 10;
            int pointsForBorder = 15;
            int pointsForResourceEconomic = 5;
            int pointsForResourceMilitary = 10;


            Dictionary<string, int> targets = regions.Values.Where(x => x.FactionId == targetFactionId)
                                                     .Select(x => x.Id)
                                                     .Except(regionsOwnedIds)
                                                     .Where(x => regionsOwnedIds.Any(y => RegionHasBorder(x, y)))
                                                     .ToDictionary(x => x, y => 0);

            Parallel.ForEach(regions.Values.Where(x => targets.ContainsKey(x.Id)).ToList(), (region) =>
            {
                if (region.SovereignFactionId == factionId)
                {
                    targets[region.Id] += pointsForSovereignty;
                }


                Parallel.ForEach(holdings.Values.Where(x => x.RegionId == region.Id).ToList(), (holding) =>
                {
                    switch (holding.Type)
                    {
                        case HoldingType.Castle:
                            targets[region.Id] += pointsForHoldingCastle;
                            break;

                        case HoldingType.City:
                            targets[region.Id] += pointsForHoldingCity;
                            break;

                        case HoldingType.Temple:
                            targets[region.Id] += pointsForHoldingTemple;
                            break;
                    }
                });

                Resource regionResource = resources.Values.FirstOrDefault(x => x.Id == region.ResourceId);

                if (regionResource != null)
                {
                    switch (regionResource.Type)
                    {
                        case ResourceType.Military:
                            targets[region.Id] += pointsForResourceMilitary;
                            break;

                        case ResourceType.Wealth:
                            targets[region.Id] += pointsForResourceEconomic;
                            break;
                    }
                }

                targets[region.Id] += regionsOwnedIds.Count(x => RegionHasBorder(x, region.Id)) * pointsForBorder;
            });

            if (targets.Count == 0)
            {
                return null;
            }

            int maxScore = targets.Max(x => x.Value);
            List<string> topTargets = targets.Keys.Where(x => targets[x] == maxScore).ToList();
            string regionId = topTargets[random.Next(0, topTargets.Count())];

            TransferRegion(regionId, factionId);

            return regionId;
        }

        /// <summary>
        /// Chooses a region to attack.
        /// </summary>
        /// <returns>The region identifier.</returns>
        /// <param name="factionId">Faction identifier.</param>
        string ChooseAttack(string factionId)
        {
            Random random = new Random();
            List<Region> regionsOwned = regions.Values.Where(x => x.FactionId == factionId).ToList();
            List<string> choices = new List<string>();

            foreach (Region region in regionsOwned)
            {
                List<Border> regionBorders = borders.Values
                                                    .Where(x => x.Region1Id == region.Id ||
                                                                       x.Region2Id == region.Id)
                                                    .ToList();

                foreach (Border border in regionBorders)
                {
                    Region region2 = regions[border.Region2Id];

                    if (region2.Id != region.Id && region2.FactionId != "gaia")
                    {
                        choices.Add(region2.Id);
                    }
                }
            }

            while (choices.Count > 0)
            {
                string regionId = choices[random.Next(choices.Count)];
                Region region = regions[regionId];

                if (region.FactionId != "gaia")
                {
                    return regionId;
                }

                choices.Remove(regionId);
            }

            // TODO: Better handling of no region to attack
            return null;
        }

        void AttackRegion(string factionId, string regionId)
        {
            Random random = new Random();

            Region targetRegion = regions[regionId];
            Faction attackerFaction = factions[factionId];
            Faction defenderFaction = factions[targetRegion.FactionId];

            List<Army> attackerArmies = armies.Values.Where(army => army.FactionId == attackerFaction.Id).ToList();
            List<Army> defenderArmies = armies.Values.Where(army => army.FactionId == defenderFaction.Id).ToList();

            int attackerTroops = attackerArmies.Select(y => y.Size).Sum();
            int defenderTroops = defenderArmies.Select(y => y.Size).Sum();

            while (attackerTroops > 0 && defenderTroops > 0)
            {
                int attackerUnitNumber = random.Next(attackerArmies.Count);
                int defenderUnitNumber = random.Next(defenderArmies.Count);

                while (attackerArmies.ElementAt(attackerUnitNumber).Size == 0)
                {
                    attackerUnitNumber = random.Next(attackerArmies.Count);
                }

                while (defenderArmies.ElementAt(defenderUnitNumber).Size == 0)
                {
                    defenderUnitNumber = random.Next(defenderArmies.Count);
                }

                string attackerUnitId = attackerArmies.ElementAt(attackerUnitNumber).UnitId;
                string defenderUnitId = defenderArmies.ElementAt(defenderUnitNumber).UnitId;

                Unit attackerUnit = units.Values.FirstOrDefault(unit => unit.Id == attackerUnitId);
                Army attackerArmy = attackerArmies.FirstOrDefault(army => army.FactionId == attackerFaction.Id);

                Unit defenderUnit = units.Values.FirstOrDefault(unit => unit.Id == defenderUnitId);
                Army defenderArmy = defenderArmies.FirstOrDefault(army => army.FactionId == defenderFaction.Id);


                // TODO: Attack and Defence bonuses

                attackerArmy.Size =
                    (attackerUnit.Health * attackerArmy.Size - defenderUnit.Power * defenderArmy.Size) /
                    attackerUnit.Health;

                defenderArmy.Size =
                    (defenderUnit.Health * defenderArmy.Size - attackerUnit.Power * attackerArmy.Size) /
                    defenderUnit.Health;

                attackerArmy.Size = Math.Max(0, attackerArmy.Size);
                defenderArmy.Size = Math.Max(0, defenderArmy.Size);
            }

            // TODO: In the GameDomainService I should change the realations based on wether the
            // region was sovereign or not

            if (attackerTroops > defenderTroops)
            {
                // TODO: Do something
            }

            // TODO: Do something
        }
    }
}
