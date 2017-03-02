using System;
using System.Collections.Generic;
using System.IO;

using Narivia.Models;
using Narivia.DataAccess.Repositories;
using Narivia.Infrastructure.Helpers;

namespace Narivia.DomainServices
{
    public class GameDomainService
    {
        BiomeRepository biomeRepository;
        CultureRepository cultureRepository;
        FactionRepository factionRepository;
        HoldingRepository holdingRepository;
        RegionRepository regionRepository;
        ResourceRepository resourceRepository;
        UnitRepository unitRepository;
        BorderRepository borderRepository;
        ArmyRepository armyRepository;

        BattleDomainService battleDomainService;

        World world;

        string[,] worldTiles;

        string playerFactionId;
        int turn;

        public void NewGame(string worldId, string factionId)
        {
            LoadEntities(worldId);
            LoadWorld(worldId);

            InitializeGame(factionId);
            InitialiseDomainServices();
            InitializeEntities();
        }

        public void NextTurn()
        {
            List<Faction> factions = factionRepository.GetAll();

            foreach (Faction faction in factions)
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
        /// Transfers the region to another faction.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        public void TransferRegion(string regionId, string factionId)
        {
            Region region = regionRepository.Get(regionId);
            region.FactionId = factionId;
        }

        public bool RegionHasBorder(string region1Id, string region2Id)
        {
            if (borderRepository.Contains(region1Id, region2Id))
                return true;

            return false;
        }

        void LoadEntities(string worldId)
        {
            biomeRepository = new BiomeRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "biomes.xml"));
            cultureRepository = new CultureRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "cultures.xml"));
            factionRepository = new FactionRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "factions.xml"));
            holdingRepository = new HoldingRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "holdings.xml"));
            regionRepository = new RegionRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "regions.xml"));
            resourceRepository = new ResourceRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "resources.xml"));
            unitRepository = new UnitRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "units.xml"));
        }

        void LoadWorld(string worldId)
        {
            borderRepository = new BorderRepository();
            armyRepository = new ArmyRepository();

            LoadMap(worldId);
            LoadBorders();
        }

        void LoadMap(string worldId)
        {
            List<Region> regions = regionRepository.GetAll();
            Dictionary<int, string> regionColourIds = new Dictionary<int, string>();

            worldTiles = new string[world.Width, world.Height];

            // Mapping the region colours
            foreach (Region region in regions)
                regionColourIds.Add(region.Colour.ToArgb(), region.Id);

            // Reading the map pixel by pixel
            using (FastBitmap bmp = new FastBitmap(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "map.png")))
            {
                for (int y = 0; y < world.Width; y++)
                    for (int x = 0; x < world.Height; y++)
                        worldTiles[x, y] = regionColourIds[bmp.GetPixel(x, y).ToArgb()];
            }
        }

        void InitialiseDomainServices()
        {
            battleDomainService = new BattleDomainService()
            {
                ArmyRepository = armyRepository,
                FactionRepository = factionRepository,
                RegionRepository = regionRepository,
                UnitRepository = unitRepository
            };
        }

        void InitializeGame(string factionId)
        {
            playerFactionId = factionId;
            turn = 0;
        }

        void InitializeEntities()
        {
            List<Faction> factions = factionRepository.GetAll();
            List<Unit> units = unitRepository.GetAll();

            foreach (Faction faction in factions)
            {
                // TODO: set starting wealth
                faction.Wealth = 0;

                foreach (Unit unit in units)
                {
                    Army army = new Army
                    {
                        FactionId = faction.Id,
                        UnitId = unit.Id,
                        Size = 0
                    };

                    armyRepository.Add(army);
                }
            }
        }

        void SetBorder(string region1Id, string region2Id)
        {
            if (RegionHasBorder(region1Id, region2Id))
                return;

            Border border = new Border
            {
                Region1Id = region1Id,
                Region2Id = region2Id
            };

            borderRepository.Add(border);
        }

        void LoadBorders()
        {
            for (int x = 0; x < world.Width; x += 5)
                for (int y = 0; y < world.Height; y += 5)
                {
                    List<string> region2IdVisited = new List<string>();
                    string region1Id = worldTiles[x, y];

                    for (int dx = -2; dx <= 2; dx++)
                        if (x + dx >= 0 && x + dx < world.Width)
                            for (int dy = -2; dy <= 2; dy++)
                                if (y + dy >= 0 && y + dy < world.Height)
                                {
                                    string region2Id = worldTiles[x + dx, y + dy];

                                    if (region2IdVisited.Contains(region2Id))
                                        continue;

                                    region2IdVisited.Add(region2Id);

                                    if (region1Id != region2Id)
                                        SetBorder(region1Id, region2Id);
                                }
                }
        }

        /// <summary>
        /// Chooses a region to attack.
        /// </summary>
        /// <returns>The region identifier.</returns>
        /// <param name="factionId">Faction identifier.</param>
        string ChooseAttack(string factionId)
        {
            Random random = new Random();
            List<Region> regionsOwned = regionRepository.GetAllByFaction(factionId);
            List<string> choices = new List<string>();

            foreach (Region region in regionsOwned)
            {
                List<Border> borders = borderRepository.GetAllByRegion(region.Id);

                foreach (Border border in borders)
                {
                    Region region2 = regionRepository.Get(border.Region2Id);

                    if (region2.Id != region.Id && region2.FactionId != "gaia")
                        choices.Add(region2.Id);
                }
            }

            while (choices.Count > 0)
            {
                string regionId = choices[random.Next(choices.Count)];
                Region region = regionRepository.Get(regionId);

                if (region.FactionId != "gaia")
                    return regionId;

                choices.Remove(regionId);
            }

            // TODO: Better handling of no region to attack
            return null;
        }

        void AttackRegion(string factionId, string regionId)
        {
            BattleResult battleResult = battleDomainService.AttackRegion(factionId, regionId);

            // TODO: Use the result and send notifications, change relations, etc...
        }
    }
}
