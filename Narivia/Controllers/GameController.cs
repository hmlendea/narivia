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
        RepositoryXml<Faction> factionRepository;
        RepositoryXml<Holding> holdingRepository;
        RepositoryXml<Region> regionRepository;
        RepositoryXml<Resource> resourceRepository;
        RepositoryXml<Unit> unitRepository;
        World world;

        string[,] worldTiles;
        Dictionary<string, List<string>> borders;

        string playerFactionId;
        int turn;

        public void NewGame(string worldId, string factionId)
        {
            LoadWorld(worldId);
            LoadEntities(worldId);
            LoadMap(worldId);

            InitializeGame(factionId);
            InitializeEntities();

            DetermineBorders();
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
            if (!borders.ContainsKey(region1Id) || !borders.ContainsKey(region2Id))
                return false;

            if (borders[region1Id].Contains(region2Id))
                return true;

            if (borders[region2Id].Contains(region1Id))
                return true;

            return false;
        }

        void LoadEntities(string worldId)
        {
            biomeRepository = new RepositoryXml<Biome>(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "biomes.xml"));
            cultureRepository = new RepositoryXml<Culture>(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "cultures.xml"));
            factionRepository = new RepositoryXml<Faction>(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "factions.xml"));
            holdingRepository = new RepositoryXml<Holding>(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "holdings.xml"));
            regionRepository = new RepositoryXml<Region>(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "regions.xml"));
            resourceRepository = new RepositoryXml<Resource>(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "resources.xml"));
            unitRepository = new RepositoryXml<Unit>(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "units.xml"));
        }

        void LoadWorld(string worldId)
        {
            // TODO: Load world
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
                    // TODO: set starting troops
                }
            }
        }

        void DetermineBorders()
        {
            borders = new Dictionary<string, List<string>>();

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

                                    if (region1Id != region2Id &&
                                        !RegionHasBorder(region1Id, region2Id))
                                    {
                                        if (!borders.ContainsKey(region1Id))
                                            borders.Add(region1Id, new List<string>());

                                        borders[region1Id].Add(region2Id);
                                    }
                                }
                }
        }
    }
}
