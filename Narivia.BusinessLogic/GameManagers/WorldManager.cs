using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using Narivia.BusinessLogic.Mapping;
using Narivia.DataAccess.Repositories;
using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Infrastructure.Extensions;
using Narivia.Infrastructure.Helpers;
using Narivia.Models;

namespace Narivia.BusinessLogic.GameManagers
{
    public class WorldManager
    {
        public Dictionary<string, Biome> Biomes { get; set; }
        public Dictionary<string, Culture> Cultures { get; set; }
        public Dictionary<string, Faction> Factions { get; set; }
        public Dictionary<string, Holding> Holdings { get; set; }
        public Dictionary<string, Region> Regions { get; set; }
        public Dictionary<string, Resource> Resources { get; set; }
        public Dictionary<string, Unit> Units { get; set; }

        public Dictionary<Tuple<string, string>, Army> Armies { get; set; }
        public Dictionary<Tuple<string, string>, Border> Borders { get; set; }
        public Dictionary<Tuple<string, string>, Relation> Relations { get; set; }

        public string[,] WorldTiles
        {
            get { return worldTiles; }
            set { worldTiles = value; }
        }

        public int WorldWidth => world.Width;
        public int WorldHeight => world.Height;
        public string WorldName => world.Name;
        public string WorldId => world.Id;

        World world;

        string[,] worldTiles;
        string[,] biomeMap;

        public void LoadWorld(string worldId)
        {
            LoadEntities(worldId);
            LoadMap(worldId);
            LoadBorders();
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

            Biomes = new Dictionary<string, Biome>();
            Cultures = new Dictionary<string, Culture>();
            Factions = new Dictionary<string, Faction>();
            Holdings = new Dictionary<string, Holding>();
            Regions = new Dictionary<string, Region>();
            Resources = new Dictionary<string, Resource>();
            Units = new Dictionary<string, Unit>();

            biomeList.ForEach(biome => Biomes.Add(biome.Id, biome));
            cultureList.ForEach(culture => Cultures.Add(culture.Id, culture));
            factionList.ForEach(faction => Factions.Add(faction.Id, faction));
            holdingList.ForEach(holding => Holdings.Add(holding.Id, holding));
            regionList.ForEach(region => Regions.Add(region.Id, region));
            resourceList.ForEach(resource => Resources.Add(resource.Id, resource));
            unitList.ForEach(unit => Units.Add(unit.Id, unit));
        }

        void LoadMap(string worldId)
        {
            Armies = new Dictionary<Tuple<string, string>, Army>();
            Borders = new Dictionary<Tuple<string, string>, Border>();
            Relations = new Dictionary<Tuple<string, string>, Relation>();

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
            Regions.Values.ToList().ForEach(region => regionColourIds.Register(ColourTranslator.ToArgb(region.Colour), region.Id));
            Biomes.Values.ToList().ForEach(biome => biomeColourIds.Register(ColourTranslator.ToArgb(biome.Colour), biome.Id));

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

            Borders.Add(borderKey, border);
        }

        /// <summary>
        /// Checks wether the specified regions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified regions share a border, <c>false</c> otherwise.</returns>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        public bool RegionHasBorder(string region1Id, string region2Id)
        {
            return Borders.Values.Any(x => (x.Region1Id == region1Id && x.Region2Id == region2Id) ||
                                           (x.Region1Id == region2Id && x.Region2Id == region1Id));
        }

        /// <summary>
        /// Checks wether the specified regions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified regions share a border, <c>false</c> otherwise.</returns>
        /// <param name="faction1Id">First faction identifier.</param>
        /// <param name="faction2Id">Second faction identifier.</param>
        public bool FactionHasBorder(string faction1Id, string faction2Id)
        {
            List<Region> faction1Regions = Regions.Values.Where(x => x.FactionId == faction1Id).ToList();
            List<Region> faction2Regions = Regions.Values.Where(x => x.FactionId == faction2Id).ToList();

            // TODO: Optimise this!!!
            foreach (Region region1 in faction1Regions)
            {
                foreach (Region region2 in faction2Regions)
                {
                    if (RegionHasBorder(region1.Id, region2.Id))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the faction identifier at the given position.
        /// </summary>
        /// <returns>The faction identifier.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public string FactionIdAtPosition(int x, int y)
        {
            return Regions[worldTiles[x, y]].FactionId;
        }

        /// <summary>
        /// Transfers the specified region to the specified faction.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        public void TransferRegion(string regionId, string factionId)
        {
            Regions[regionId].FactionId = factionId;
        }
    }
}
