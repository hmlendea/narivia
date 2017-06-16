using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.GameLogic.Generators;
using Narivia.GameLogic.Generators.Interfaces;
using Narivia.GameLogic.Mapping;
using Narivia.DataAccess.Repositories;
using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Infrastructure.Extensions;
using Narivia.Infrastructure.Helpers;
using Narivia.Models;
using Narivia.Models.Enumerations;

namespace Narivia.GameLogic.GameManagers
{
    /// <summary>
    /// World manager.
    /// </summary>
    public class WorldManager : IWorldManager
    {
        Random random;
        World world;

        string[,] worldTiles;
        string[,] biomeMap;

        /// <summary>
        /// Gets or sets the biomes.
        /// </summary>
        /// <value>The biomes.</value>
        public Dictionary<string, Biome> Biomes { get; set; }

        /// <summary>
        /// Gets or sets the cultures.
        /// </summary>
        /// <value>The cultures.</value>
        public Dictionary<string, Culture> Cultures { get; set; }

        /// <summary>
        /// Gets or sets the factions.
        /// </summary>
        /// <value>The factions.</value>
        public Dictionary<string, Faction> Factions { get; set; }

        /// <summary>
        /// Gets or sets the holdings.
        /// </summary>
        /// <value>The holdings.</value>
        public Dictionary<string, Holding> Holdings { get; set; }

        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        /// <value>The regions.</value>
        public Dictionary<string, Region> Regions { get; set; }

        /// <summary>
        /// Gets or sets the resources.
        /// </summary>
        /// <value>The resources.</value>
        public Dictionary<string, Resource> Resources { get; set; }

        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>The units.</value>
        public Dictionary<string, Unit> Units { get; set; }

        /// <summary>
        /// Gets or sets the armies.
        /// </summary>
        /// <value>The armies.</value>
        public Dictionary<Tuple<string, string>, Army> Armies { get; set; }

        /// <summary>
        /// Gets or sets the borders.
        /// </summary>
        /// <value>The borders.</value>
        public Dictionary<Tuple<string, string>, Border> Borders { get; set; }

        /// <summary>
        /// Gets or sets the relations.
        /// </summary>
        /// <value>The relations.</value>
        public Dictionary<Tuple<string, string>, Relation> Relations { get; set; }

        /// <summary>
        /// Gets or sets the world tiles.
        /// </summary>
        /// <value>The world tiles.</value>
        public string[,] WorldTiles
        {
            get { return worldTiles; }
            set { worldTiles = value; }
        }

        /// <summary>
        /// Gets the width of the world.
        /// </summary>
        /// <value>The width of the world.</value>
        public int WorldWidth => world.Width;

        /// <summary>
        /// Gets the height of the world.
        /// </summary>
        /// <value>The height of the world.</value>
        public int WorldHeight => world.Height;

        /// <summary>
        /// Gets the name of the world.
        /// </summary>
        /// <value>The name of the world.</value>
        public string WorldName => world.Name;

        /// <summary>
        /// Gets the world identifier.
        /// </summary>
        /// <value>The world identifier.</value>
        public string WorldId => world.Id;

        /// <summary>
        /// Gets or sets the base region income.
        /// </summary>
        /// <value>The base region income.</value>
        public int BaseRegionIncome
        {
            get { return world.BaseRegionIncome; }
            set { world.BaseRegionIncome = value; }
        }

        /// <summary>
        /// Gets or sets the base region recruitment.
        /// </summary>
        /// <value>The base region recruitment.</value>
        public int BaseRegionRecruitment
        {
            get { return world.BaseRegionRecruitment; }
            set { world.BaseRegionRecruitment = value; }
        }

        /// <summary>
        /// Gets or sets the base faction recruitment.
        /// </summary>
        /// <value>The base faction recruitment.</value>
        public int BaseFactionRecruitment
        {
            get { return world.BaseFactionRecruitment; }
            set { world.BaseFactionRecruitment = value; }
        }

        /// <summary>
        /// Gets or sets the minimum number of troops required to attack.
        /// </summary>
        /// <value>The minimum troops to attack.</value>
        public int MinTroopsPerAttack
        {
            get { return world.MinTroopsPerAttack; }
            set { world.MinTroopsPerAttack = value; }
        }

        /// <summary>
        /// Gets or sets the starting wealth.
        /// </summary>
        /// <value>The starting wealth.</value>
        public int StartingWealth
        {
            get { return world.StartingWealth; }
            set { world.StartingWealth = value; }
        }

        /// <summary>
        /// Gets or sets the starting troops.
        /// </summary>
        /// <value>The starting troops.</value>
        public int StartingTroops
        {
            get { return world.StartingTroops; }
            set { world.StartingTroops = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldManager"/> class.
        /// </summary>
        public WorldManager()
        {
            random = new Random();
        }

        /// <summary>
        /// Loads the world.
        /// </summary>
        /// <param name="worldId">World identifier.</param>
        public void LoadWorld(string worldId)
        {
            LoadEntities(worldId);
            LoadMap(worldId);
            LoadBorders();

            InitializeEntities();
        }

        /// <summary>
        /// Checks wether the specified regions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified regions share a border, <c>false</c> otherwise.</returns>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        public bool RegionBordersRegion(string region1Id, string region2Id)
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
        public bool FactionBordersFaction(string faction1Id, string faction2Id)
        {
            List<Region> faction1Regions = Regions.Values.Where(x => x.FactionId == faction1Id).ToList();
            List<Region> faction2Regions = Regions.Values.Where(x => x.FactionId == faction2Id).ToList();

            // TODO: Optimise this!!!
            foreach (Region region1 in faction1Regions)
            {
                foreach (Region region2 in faction2Regions)
                {
                    if (RegionBordersRegion(region1.Id, region2.Id))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks wether the specified faction shares a border with the specified region.
        /// </summary>
        /// <returns><c>true</c>, if the faction share a border with that region, <c>false</c> otherwise.</returns>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="regionId">Region identifier.</param>
        public bool FactionBordersRegion(string factionId, string regionId)
        {
            return GetFactionRegions(factionId).Any(r => RegionBordersRegion(r.Id, regionId));
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

        /// <summary>
        /// Gets the faction troops count.
        /// </summary>
        /// <returns>The faction troops count.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionTroopsCount(string factionId)
        {
            return Armies.Values.Where(a => a.FactionId == factionId)
                                .Sum(a => a.Size);
        }

        /// <summary>
        /// Gets the faction capital.
        /// </summary>
        /// <returns>Region identifier.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public string GetFactionCapital(string factionId)
        {
            return Regions.Values.FirstOrDefault(r => r.FactionId == factionId &&
                                                      r.SovereignFactionId == factionId &&
                                                      r.Type == RegionType.Capital).Id;
        }

        /// <summary>
        /// Gets the armies of a faction.
        /// </summary>
        /// <returns>The armies.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Army> GetFactionArmies(string factionId)
        {
            return Armies.Values.Where(a => a.FactionId == factionId);
        }

        /// <summary>
        /// Gets the holdings of a faction.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Holding> GetFactionHoldings(string factionId)
        {
            return Holdings.Values.Where(h => h.Type != HoldingType.Empty &&
                                              Regions[h.RegionId].FactionId == factionId);
        }

        /// <summary>
        /// Gets the regions of a faction.
        /// </summary>
        /// <returns>The regions.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Region> GetFactionRegions(string factionId)
        {
            return Regions.Values.Where(r => r.FactionId == factionId);
        }

        /// <summary>
        /// Gets the holdings of a region.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="regionId">Region identifier.</param>
        public IEnumerable<Holding> GetRegionHoldings(string regionId)
        {
            return Holdings.Values.Where(h => h.Type != HoldingType.Empty &&
                                              h.RegionId == regionId);
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
            //holdingList.ForEach(holding => Holdings.Add(holding.Id, holding));
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
            if (RegionBordersRegion(region1Id, region2Id))
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

        void InitializeEntities()
        {
            // Order is important
            Regions.Values.ToList().ForEach(r => InitialiseRegion(r.Id));
            Factions.Values.ToList().ForEach(f => InitialiseFaction(f.Id));
        }

        void InitialiseFaction(string factionId)
        {
            Faction faction = Factions[factionId];

            if (faction.Id == "gaia")
            {
                faction.Alive = false;
                return;
            }

            faction.Wealth = StartingWealth;
            faction.Alive = true;

            foreach (Unit unit in Units.Values.ToList())
            {
                Tuple<string, string> armyKey = new Tuple<string, string>(faction.Id, unit.Id);
                Army army = new Army
                {
                    FactionId = faction.Id,
                    UnitId = unit.Id,
                    Size = StartingTroops
                };

                Armies.Add(armyKey, army);
            }

            GenerateHoldings(faction.Id);
        }

        void InitialiseRegion(string regionId)
        {
            Region region = Regions[regionId];

            if (string.IsNullOrWhiteSpace(region.SovereignFactionId))
            {
                region.SovereignFactionId = region.FactionId;
            }
        }

        void GenerateHoldings(string factionId)
        {
            Faction faction = Factions[factionId];

            int holdingSlotsLeft = world.HoldingSlotsPerFaction;

            string capitalRegionId = GetFactionCapital(faction.Id);

            INameGenerator nameGenerator = CreateNameGenerator(faction.CultureId);
            nameGenerator.ExcludedSubstrings.AddRange(Factions.Values.Select(f => f.Name));
            nameGenerator.ExcludedSubstrings.AddRange(Holdings.Values.Select(h => h.Name));
            nameGenerator.ExcludedSubstrings.AddRange(Regions.Values.Select(r => r.Name));

            List<Region> ownedRegions = GetFactionRegions(faction.Id).ToList();

            foreach (Region region in ownedRegions)
            {
                Holding holding = GenerateHolding(nameGenerator, region.Id);

                if (region.Id == capitalRegionId)
                {
                    holding.Name = region.Name;
                    holding.Description = $"The government seat castle of {faction.Name}";
                    holding.Type = HoldingType.Castle;
                }

                Holdings.Add(holding.Id, holding);
                holdingSlotsLeft -= 1;
            }

            while (holdingSlotsLeft > 0)
            {
                Region region = ownedRegions.RandomElement();
                Holding holding = GenerateHolding(nameGenerator, region.Id);

                holding.Description = string.Empty;
                holding.Type = HoldingType.Empty;

                Holdings.Add(holding.Id, holding);
                holdingSlotsLeft -= 1;
            }
        }

        /// <summary>
        /// Creates a name generator.
        /// </summary>
        /// <returns>The name generator.</returns>
        /// <param name="cultureId">Culture identifier.</param>
        INameGenerator CreateNameGenerator(string cultureId)
        {
            INameGenerator nameGenerator;
            Culture culture = Cultures[cultureId];

            List<List<string>> wordLists = culture.PlaceNameSchema.Split(' ').ToList()
                                                  .Select(x => File.ReadAllLines(Path.Combine(ApplicationPaths.WordListsDirectory,
                                                                                              $"{x}.txt")).ToList())
                                                  .ToList();

            if (culture.PlaceNameGenerator == NameGenerator.RandomMixerNameGenerator && wordLists.Count == 2)
            {
                nameGenerator = new RandomMixerNameGenerator(wordLists[0], wordLists[1]);
            }
            else if (culture.PlaceNameGenerator == NameGenerator.RandomMixerNameGenerator && wordLists.Count == 3)
            {
                nameGenerator = new RandomMixerNameGenerator(wordLists[0], wordLists[1], wordLists[2]);
            }
            else // Default: Markov
            {
                nameGenerator = new MarkovNameGenerator(wordLists[0]);
            }

            return nameGenerator;
        }

        Holding GenerateHolding(INameGenerator generator, string regionId)
        {
            Region region = Regions[regionId];
            Array holdingTypes = Enum.GetValues(typeof(HoldingType));

            HoldingType holdingType = (HoldingType)holdingTypes.GetValue(random.Next(1, holdingTypes.Length));
            string name = generator.GenerateName();

            Holding holding = new Holding
            {
                Id = $"h_{name.Replace(" ", "_").ToLower()}",
                RegionId = region.Id,
                Name = name,
                Description = $"The {name} {holdingType.ToString().ToLower()}", // TODO: Better description
                Type = holdingType
            };

            // TODO: Make sure this never happens and then remove this workaround
            while (Holdings.Values.Any(h => h.Id == holding.Id))
            {
                return GenerateHolding(generator, region.Id);
            }

            return holding;
        }
    }
}
