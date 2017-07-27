using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.GameLogic.Generators;
using Narivia.GameLogic.Generators.Interfaces;
using Narivia.GameLogic.Mapping;
using Narivia.Graphics;
using Narivia.DataAccess.Repositories;
using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Infrastructure.Extensions;
using Narivia.Infrastructure.Helpers;
using Narivia.Logging;
using Narivia.Logging.Enumerations;
using Narivia.Models;
using Narivia.Models.Enumerations;

namespace Narivia.GameLogic.GameManagers
{
    /// <summary>
    /// World manager.
    /// </summary>
    public class WorldManager : IWorldManager
    {
        readonly Random random;

        World world;

        string[,] worldTiles;
        string[,] biomeMap;

        ConcurrentDictionary<string, Biome> biomes;
        ConcurrentDictionary<string, Culture> cultures;
        ConcurrentDictionary<string, Faction> factions;
        ConcurrentDictionary<string, Flag> flags;
        ConcurrentDictionary<string, Holding> holdings;
        ConcurrentDictionary<string, Region> regions;
        ConcurrentDictionary<string, Resource> resources;
        ConcurrentDictionary<string, Unit> units;
        ConcurrentDictionary<Tuple<string, string>, Army> armies;
        ConcurrentDictionary<Tuple<string, string>, Border> borders;
        ConcurrentDictionary<Tuple<string, string>, Relation> relations;

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
        /// Gets or sets the price of holdings.
        /// </summary>
        /// <value>The holdings price.</value>
        public int HoldingsPrice
        {
            get { return world.HoldingsPrice; }
            set { world.HoldingsPrice = value; }
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
            LogManager.Instance.Info(LogBuilder.BuildKvpMessage(Operation.WorldLoading, OperationStatus.Started));

            LoadEntities(worldId);
            LoadMap(worldId);
            LoadBorders();

            LogManager.Instance.Info(LogBuilder.BuildKvpMessage(Operation.WorldLoading, OperationStatus.Success));
            LogManager.Instance.Info(LogBuilder.BuildKvpMessage(Operation.WorldInitialisation, OperationStatus.Started));

            InitializeEntities();

            LogManager.Instance.Info(LogBuilder.BuildKvpMessage(Operation.WorldInitialisation, OperationStatus.Success));
        }

        /// <summary>
        /// Checks wether the specified regions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified regions share a border, <c>false</c> otherwise.</returns>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        public bool RegionBordersRegion(string region1Id, string region2Id)
        {
            return borders.Values.Any(x => (x.Region1Id == region1Id && x.Region2Id == region2Id) ||
                                           (x.Region1Id == region2Id && x.Region2Id == region1Id));
        }

        /// <summary>
        /// Checks wether a region has empty holding slots.
        /// </summary>
        /// <returns><c>true</c>, if the region has empty holding slots, <c>false</c> otherwise.</returns>
        /// <param name="regionId">Region identifier.</param>
        public bool RegionHasEmptyHoldingSlots(string regionId)
        {
            return holdings.Values.Count(h => h.RegionId == regionId && h.Type == HoldingType.Empty) > 0;
        }

        /// <summary>
        /// Checks wether the specified regions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified regions share a border, <c>false</c> otherwise.</returns>
        /// <param name="faction1Id">First faction identifier.</param>
        /// <param name="faction2Id">Second faction identifier.</param>
        public bool FactionBordersFaction(string faction1Id, string faction2Id)
        {
            List<Region> faction1Regions = regions.Values.Where(x => x.FactionId == faction1Id).ToList();
            List<Region> faction2Regions = regions.Values.Where(x => x.FactionId == faction2Id).ToList();

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
            return regions[worldTiles[x, y]].FactionId;
        }

        /// <summary>
        /// Transfers the specified region to the specified faction.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        public void TransferRegion(string regionId, string factionId)
        {
            regions[regionId].FactionId = factionId;
        }

        /// <summary>
        /// Gets the armies.
        /// </summary>
        /// <returns>The armies.</returns>
        public IEnumerable<Army> GetArmies()
        => armies.Values;

        /// <summary>
        /// Gets the biomes.
        /// </summary>
        /// <returns>The biomes.</returns>
        public IEnumerable<Biome> GetBiomes()
        => biomes.Values;

        /// <summary>
        /// Gets the borders.
        /// </summary>
        /// <returns>The borders.</returns>
        public IEnumerable<Border> GetBorders()
        => borders.Values;

        /// <summary>
        /// Gets the cultures.
        /// </summary>
        /// <returns>The cultures.</returns>
        public IEnumerable<Culture> GetCultures()
        => cultures.Values;

        /// <summary>
        /// Gets the factions.
        /// </summary>
        /// <returns>The factions.</returns>
        public IEnumerable<Faction> GetFactions()
        => factions.Values;

        /// <summary>
        /// Gets the faction troops amount.
        /// </summary>
        /// <returns>The faction troops amount.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionTroopsAmount(string factionId)
        {
            return armies.Values.Where(a => a.FactionId == factionId)
                                .Sum(a => a.Size);
        }

        /// <summary>
        /// Gets the faction capital.
        /// </summary>
        /// <returns>Region.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public Region GetFactionCapital(string factionId)
        {
            return regions.Values.FirstOrDefault(r => r.FactionId == factionId &&
                                                      r.SovereignFactionId == factionId &&
                                                      r.Type == RegionType.Capital);
        }

        /// <summary>
        /// Gets or sets the X map coordinate of the centre of the faction territoriy.
        /// </summary>
        /// <value>The X coordinate.</value>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionCentreX(string factionId)
        {
            int minX = world.Width - 1;
            int maxX = 0;

            for (int y = 0; y < world.Height; y++)
            {
                for (int x = 0; x < world.Width; x++)
                {
                    if (regions[worldTiles[x, y]].FactionId != factionId)
                    {
                        continue;
                    }

                    if (x < minX)
                    {
                        minX = x;
                    }
                    else if (x > maxX)
                    {
                        maxX = x;
                    }
                }
            }

            return (minX + maxX) / 2;
        }

        /// <summary>
        /// Gets or sets the Y map coordinate of the centre of the faction territoriy.
        /// </summary>
        /// <value>The Y coordinate.</value>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionCentreY(string factionId)
        {
            int minY = world.Height - 1;
            int maxY = 0;

            for (int y = 0; y < world.Height; y++)
            {
                for (int x = 0; x < world.Width; x++)
                {
                    if (regions[worldTiles[x, y]].FactionId != factionId)
                    {
                        continue;
                    }

                    if (y < minY)
                    {
                        minY = y;
                    }
                    else if (y > maxY)
                    {
                        maxY = y;
                    }
                }
            }

            return (minY + maxY) / 2;
        }

        /// <summary>
        /// Gets the relation between two factions.
        /// </summary>
        /// <returns>The faction relation.</returns>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        public int GetFactionRelation(string sourceFactionId, string targetFactionId)
        {
            return relations.Values.FirstOrDefault(r => r.SourceFactionId == sourceFactionId &&
                                                        r.TargetFactionId == targetFactionId).Value;
        }

        /// <summary>
        /// Gets the armies of a faction.
        /// </summary>
        /// <returns>The armies.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Army> GetFactionArmies(string factionId)
        {
            return armies.Values.Where(a => a.FactionId == factionId);
        }

        /// <summary>
        /// Gets the holdings of a faction.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Holding> GetFactionHoldings(string factionId)
        {
            return holdings.Values.Where(h => h.Type != HoldingType.Empty &&
                                              regions[h.RegionId].FactionId == factionId);
        }

        /// <summary>
        /// Gets the regions of a faction.
        /// </summary>
        /// <returns>The regions.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Region> GetFactionRegions(string factionId)
        {
            return regions.Values.Where(r => r.FactionId == factionId);
        }

        /// <summary>
        /// Gets the relations of a faction.
        /// </summary>
        /// <returns>The relations of a faction.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Relation> GetFactionRelations(string factionId)
        {
            return relations.Values.Where(r => r.SourceFactionId == factionId &&
                                               r.SourceFactionId != r.TargetFactionId);
        }

        /// <summary>
        /// Gets the flags.
        /// </summary>
        /// <returns>The flags.</returns>
        public IEnumerable<Flag> GetFlags()
        => flags.Values;

        /// <summary>
        /// Gets the holdings.
        /// </summary>
        /// <returns>The holdings.</returns>
        public IEnumerable<Holding> GetHoldings()
        => holdings.Values;

        /// <summary>
        /// Gets the regions.
        /// </summary>
        /// <returns>The regions.</returns>
        public IEnumerable<Region> GetRegions()
        => regions.Values;

        /// <summary>
        /// Gets the holdings of a region.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="regionId">Region identifier.</param>
        public IEnumerable<Holding> GetRegionHoldings(string regionId)
        => holdings.Values
                   .Where(h => h.Type != HoldingType.Empty &&
                               h.RegionId == regionId);

        /// <summary>
        /// Gets the relations.
        /// </summary>
        /// <returns>The relations.</returns>
        public IEnumerable<Relation> GetRelations()
        => relations.Values;

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <returns>The resources.</returns>
        public IEnumerable<Resource> GetResources()
        => resources.Values;

        /// <summary>
        /// Gets the units.
        /// </summary>
        /// <returns>The units.</returns>
        public IEnumerable<Unit> GetUnits()
        => units.Values;

        /// <summary>
        /// Adds the specified holding type in a region.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="holdingType">Holding type.</param>
        public void AddHolding(string regionId, HoldingType holdingType)
        {
            Holding emptySlot = holdings.Values.FirstOrDefault(h => h.RegionId == regionId &&
                                                                    h.Type == HoldingType.Empty);

            if (emptySlot != null)
            {
                emptySlot.Type = holdingType;
            }
        }

        /// <summary>
        /// Changes the relations between two factions.
        /// </summary>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        /// <param name="delta">Relations value delta.</param>
        public void ChangeRelations(string sourceFactionId, string targetFactionId, int delta)
        {
            Relation sourceRelation = relations.Values.FirstOrDefault(r => r.SourceFactionId == sourceFactionId &&
                                                                           r.TargetFactionId == targetFactionId);
            Relation targetRelation = relations.Values.FirstOrDefault(r => r.SourceFactionId == targetFactionId &&
                                                                           r.TargetFactionId == sourceFactionId);

            int oldRelations = sourceRelation.Value;
            sourceRelation.Value = Math.Max(-100, Math.Min(sourceRelation.Value + delta, 100));
            targetRelation.Value = sourceRelation.Value;
        }

        /// <summary>
        /// Sets the relations between two factions.
        /// </summary>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        /// <param name="value">Relations value.</param>
        public void SetRelations(string sourceFactionId, string targetFactionId, int value)
        {
            Relation sourceRelation = relations.Values.FirstOrDefault(r => r.SourceFactionId == sourceFactionId &&
                                                                           r.TargetFactionId == targetFactionId);
            Relation targetRelation = relations.Values.FirstOrDefault(r => r.SourceFactionId == targetFactionId &&
                                                                           r.TargetFactionId == sourceFactionId);

            sourceRelation.Value = Math.Max(-100, Math.Min(value, 100));
            targetRelation.Value = Math.Max(-100, Math.Min(value, 100));
        }

        void LoadEntities(string worldId)
        {
            IBiomeRepository biomeRepository = new BiomeRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "biomes.xml"));
            IBorderRepository borderRepository = new BorderRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "borders.xml"));
            ICultureRepository cultureRepository = new CultureRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "cultures.xml"));
            IFactionRepository factionRepository = new FactionRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "factions.xml"));
            IFlagRepository flagRepository = new FlagRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "flags.xml"));
            IHoldingRepository holdingRepository = new HoldingRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "holdings.xml"));
            IRegionRepository regionRepository = new RegionRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "regions.xml"));
            IResourceRepository resourceRepository = new ResourceRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "resources.xml"));
            IUnitRepository unitRepository = new UnitRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "units.xml"));

            IEnumerable<Biome> biomeList = biomeRepository.GetAll().ToDomainModels();
            IEnumerable<Border> borderList = borderRepository.GetAll().ToDomainModels();
            IEnumerable<Culture> cultureList = cultureRepository.GetAll().ToDomainModels();
            IEnumerable<Faction> factionList = factionRepository.GetAll().ToDomainModels();
            IEnumerable<Flag> flagList = flagRepository.GetAll().ToDomainModels();
            IEnumerable<Region> regionList = regionRepository.GetAll().ToDomainModels();
            IEnumerable<Resource> resourceList = resourceRepository.GetAll().ToDomainModels();
            IEnumerable<Unit> unitList = unitRepository.GetAll().ToDomainModels();

            biomes = new ConcurrentDictionary<string, Biome>(biomeList.ToDictionary(biome => biome.Id, biome => biome));
            borders = new ConcurrentDictionary<Tuple<string, string>, Border>(borderList.ToDictionary(border => new Tuple<string, string>(border.Region1Id, border.Region2Id), border => border));
            cultures = new ConcurrentDictionary<string, Culture>(cultureList.ToDictionary(culture => culture.Id, culture => culture));
            factions = new ConcurrentDictionary<string, Faction>(factionList.ToDictionary(faction => faction.Id, faction => faction));
            flags = new ConcurrentDictionary<string, Flag>(flagList.ToDictionary(flag => flag.Id, flag => flag));
            holdings = new ConcurrentDictionary<string, Holding>();
            regions = new ConcurrentDictionary<string, Region>(regionList.ToDictionary(region => region.Id, region => region));
            resources = new ConcurrentDictionary<string, Resource>(resourceList.ToDictionary(resource => resource.Id, resource => resource));
            units = new ConcurrentDictionary<string, Unit>(unitList.ToDictionary(unit => unit.Id, unit => unit));
        }

        void LoadMap(string worldId)
        {
            armies = new ConcurrentDictionary<Tuple<string, string>, Army>();
            relations = new ConcurrentDictionary<Tuple<string, string>, Relation>();

            ConcurrentDictionary<int, string> regionColourIds = new ConcurrentDictionary<int, string>();
            ConcurrentDictionary<int, string> biomeColourIds = new ConcurrentDictionary<int, string>();

            XmlSerializer xs = new XmlSerializer(typeof(World));

            using (FileStream fs = new FileStream(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "world.xml"), FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                world = (World)xs.Deserialize(sr);
            }

            worldTiles = new string[world.Width, world.Height];
            biomeMap = new string[world.Width, world.Height];

            // Mapping the colours
            Parallel.ForEach(regions.Values, r => regionColourIds.AddOrUpdate(r.Colour.ToArgb(), r.Id));
            Parallel.ForEach(biomes.Values, b => biomeColourIds.AddOrUpdate(b.Colour.ToArgb(), b.Id));

            // Reading the map pixel by pixel
            using (FastBitmap bmp = new FastBitmap(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "map.png")))
            {
                Parallel.For(0, world.Height,
                             y => Parallel.For(0, world.Width,
                                               x => worldTiles[x, y] = regionColourIds[bmp.GetPixel(x, y).ToArgb()]));
            }

            // Reading the biome map pixel by pixel
            using (FastBitmap bmp = new FastBitmap(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "biomes_map.png")))
            {
                Parallel.For(0, world.Height,
                             y => Parallel.For(0, world.Width,
                                               x => biomeMap[x, y] = biomeColourIds[bmp.GetPixel(x, y).ToArgb()]));
            }
        }

        // TODO: Parallelise this
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

            borders.AddOrUpdate(borderKey, border);
        }

        void InitializeEntities()
        {
            // Order is important
            regions.Values.ToList().ForEach(r => InitialiseRegion(r.Id));
            factions.Values.ToList().ForEach(f => InitialiseFaction(f.Id));
        }

        void InitialiseFaction(string factionId)
        {
            Faction faction = factions[factionId];

            if (faction.Id == "gaia")
            {
                faction.Alive = false;
                return;
            }

            faction.Wealth = StartingWealth;
            faction.Alive = true;

            Parallel.ForEach(units.Values,
                             unit =>
            {
                Tuple<string, string> armyKey = new Tuple<string, string>(faction.Id, unit.Id);
                Army army = new Army
                {
                    FactionId = faction.Id,
                    UnitId = unit.Id,
                    Size = StartingTroops
                };

                armies.AddOrUpdate(armyKey, army);
            });

            Parallel.ForEach(factions.Values, f => InitialiseRelation(factionId, f.Id));

            GenerateHoldings(faction.Id);
        }

        void InitialiseRegion(string regionId)
        {
            Region region = regions[regionId];

            if (string.IsNullOrWhiteSpace(region.SovereignFactionId))
            {
                region.SovereignFactionId = region.FactionId;
            }
        }

        void InitialiseRelation(string sourceFactionId, string targetFactionId)
        {
            if (sourceFactionId == targetFactionId ||
                sourceFactionId == "gaia" ||
                targetFactionId == "gaia")
            {
                return;
            }

            Relation relation = new Relation
            {
                SourceFactionId = sourceFactionId,
                TargetFactionId = targetFactionId,
                Value = 0
            };

            Tuple<string, string> relationKey = new Tuple<string, string>(relation.SourceFactionId, relation.TargetFactionId);

            relations.AddOrUpdate(relationKey, relation);
        }

        void GenerateHoldings(string factionId)
        {
            Faction faction = factions[factionId];
            Region capitalRegion = GetFactionCapital(faction.Id);

            int holdingSlotsLeft = world.HoldingSlotsPerFaction;

            INameGenerator nameGenerator = CreateNameGenerator(faction.CultureId);
            nameGenerator.ExcludedStrings.AddRange(factions.Values.Select(f => f.Name));
            nameGenerator.ExcludedStrings.AddRange(holdings.Values.Select(h => h.Name));
            nameGenerator.ExcludedStrings.AddRange(regions.Values.Select(r => r.Name));

            List<Region> ownedRegions = GetFactionRegions(faction.Id).ToList();

            foreach (Region region in ownedRegions)
            {
                Holding holding = GenerateHolding(nameGenerator, region.Id);

                if (region.Id == capitalRegion.Id)
                {
                    holding.Name = region.Name;
                    holding.Description = $"The government seat castle of {faction.Name}";
                    holding.Type = HoldingType.Castle;
                }

                holdings.AddOrUpdate(holding.Id, holding);
                holdingSlotsLeft -= 1;
            }

            while (holdingSlotsLeft > 0)
            {
                Region region = ownedRegions.RandomElement();
                Holding holding = GenerateHolding(nameGenerator, region.Id);

                holding.Description = string.Empty;
                holding.Type = HoldingType.Empty;

                holdings.AddOrUpdate(holding.Id, holding);
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
            Culture culture = cultures[cultureId];

            List<List<string>> wordLists = culture.PlaceNameSchema.Split(' ').ToList()
                                                  .Select(x => File.ReadAllLines(Path.Combine(ApplicationPaths.WordListsDirectory,
                                                                                              $"{x}.txt")).ToList())
                                                  .ToList();

            if (culture.PlaceNameGenerator == Models.Enumerations.NameGenerator.RandomMixerNameGenerator && wordLists.Count == 2)
            {
                nameGenerator = new RandomMixerNameGenerator(wordLists[0], wordLists[1]);
            }
            else if (culture.PlaceNameGenerator == Models.Enumerations.NameGenerator.RandomMixerNameGenerator && wordLists.Count == 3)
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
            Region region = regions[regionId];
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
            while (holdings.Values.Any(h => h.Id == holding.Id))
            {
                return GenerateHolding(generator, region.Id);
            }

            return holding;
        }
    }
}
