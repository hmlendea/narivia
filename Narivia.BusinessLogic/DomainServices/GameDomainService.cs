using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Narivia.BusinessLogic.DomainServices.Interfaces;
using Narivia.BusinessLogic.Mapping;
using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.DataAccess.Repositories;
using Narivia.Infrastructure.Helpers;
using Narivia.Models;

namespace Narivia.BusinessLogic.DomainServices
{
    public class GameDomainService : IGameDomainService
    {
        IArmyRepository armyRepository;
        IBiomeRepository biomeRepository;
        IBorderRepository borderRepository;
        ICultureRepository cultureRepository;
        IFactionRepository factionRepository;
        IHoldingRepository holdingRepository;
        IRegionRepository regionRepository;
        IResourceRepository resourceRepository;
        IUnitRepository unitRepository;

        BattleDomainService battleDomainService;

        World world;

        string[,] worldTiles;

        string playerFactionId;
        int turn;

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
            InitialiseDomainServices();
            InitializeEntities();
        }

        /// <summary>
        /// Advances the game by one turn.
        /// </summary>
        public void NextTurn()
        {
            List<Faction> factions = factionRepository.GetAll().ToDomainModels().ToList();

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
        /// Transfers the specified region to the specified faction.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        public void TransferRegion(string regionId, string factionId)
        {
            Region region = regionRepository.Get(regionId).ToDomainModel();
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
            Border border = borderRepository.Get(region1Id, region2Id).ToDomainModel();
            return border != null;
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
            List<Region> regions = regionRepository.GetAll().ToDomainModels().ToList();
            Dictionary<int, string> regionColourIds = new Dictionary<int, string>();

            worldTiles = new string[world.Width, world.Height];

            // Mapping the region colours
            foreach (Region region in regions)
                regionColourIds.Add(ColourTranslator.ToArgb(region.Colour), region.Id);

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
            List<Faction> factions = factionRepository.GetAll().ToDomainModels().ToList();
            List<Unit> units = unitRepository.GetAll().ToDomainModels().ToList();

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

                    armyRepository.Add(army.ToEntity());
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

            borderRepository.Add(border.ToEntity());
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
            List<Region> regionsOwned = regionRepository.GetAll()
                .Where(x => x.FactionId == factionId)
                .ToDomainModels().ToList();
            List<string> choices = new List<string>();

            foreach (Region region in regionsOwned)
            {
                List<Border> borders = borderRepository.GetAll().Where(x => x.Region1Id == region.Id ||
                                                                            x.Region2Id == region.Id)
                                                       .ToDomainModels().ToList();

                foreach (Border border in borders)
                {
                    Region region2 = regionRepository.Get(border.Region2Id).ToDomainModel();

                    if (region2.Id != region.Id && region2.FactionId != "gaia")
                        choices.Add(region2.Id);
                }
            }

            while (choices.Count > 0)
            {
                string regionId = choices[random.Next(choices.Count)];
                Region region = regionRepository.Get(regionId).ToDomainModel();

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
