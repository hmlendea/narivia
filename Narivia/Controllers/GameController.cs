using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Narivia.Models;
using Narivia.Repositories;
using Narivia.Utils;

namespace Narivia.Controllers
{
    public class GameController
    {
        RepositoryXml<Biome> biomeRepository;
        RepositoryXml<Culture> cultureRepository;
        FactionRepository factionRepository;
        RepositoryXml<Holding> holdingRepository;
        RegionRepository regionRepository;
        RepositoryXml<Resource> resourceRepository;
        UnitRepository unitRepository;
        BorderRepository borderRepository;
        ArmyRepository armyRepository;

        World world;

        string[,] worldTiles;

        string playerFactionId;
        int turn;

        public void NewGame(string worldId, string factionId)
        {
            LoadEntities(worldId);
            LoadWorld(worldId);

            InitializeGame(factionId);
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
                // Attack
            }

            turn += 1;
        }

        public bool RegionHasBorder(string region1Id, string region2Id)
        {
            if (borderRepository.Contains(region1Id, region2Id))
                return true;

            return false;
        }

        void LoadEntities(string worldId)
        {
            biomeRepository = new RepositoryXml<Biome>(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "biomes.xml"));
            cultureRepository = new RepositoryXml<Culture>(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "cultures.xml"));
            factionRepository = new FactionRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "factions.xml"));
            holdingRepository = new RepositoryXml<Holding>(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "holdings.xml"));
            regionRepository = new RegionRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "regions.xml"));
            resourceRepository = new RepositoryXml<Resource>(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "resources.xml"));
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
    }
}
