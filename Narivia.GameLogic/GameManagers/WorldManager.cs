using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Narivia.Common.Extensions;
using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.GameLogic.Generators;
using Narivia.GameLogic.Generators.Interfaces;
using Narivia.GameLogic.Mapping;
using Narivia.DataAccess.Repositories;
using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Logging;
using Narivia.Logging.Enumerations;
using Narivia.Models;
using Narivia.Models.Enumerations;
using Narivia.Settings;

namespace Narivia.GameLogic.GameManagers
{
    /// <summary>
    /// World manager.
    /// </summary>
    public class WorldManager : IWorldManager
    {
        readonly Random random;

        World world;

        ConcurrentDictionary<Tuple<string, string>, Army> armies;
        ConcurrentDictionary<string, Biome> biomes;
        ConcurrentDictionary<Tuple<string, string>, Border> borders;
        ConcurrentDictionary<string, Culture> cultures;
        ConcurrentDictionary<string, Faction> factions;
        ConcurrentDictionary<string, Flag> flags;
        ConcurrentDictionary<string, Holding> holdings;
        ConcurrentDictionary<string, Province> provinces;
        ConcurrentDictionary<Tuple<string, string>, Relation> relations;
        ConcurrentDictionary<string, Resource> resources;
        ConcurrentDictionary<string, Unit> units;

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
            GenerateBorders();

            LogManager.Instance.Info(LogBuilder.BuildKvpMessage(Operation.WorldLoading, OperationStatus.Success));
            LogManager.Instance.Info(LogBuilder.BuildKvpMessage(Operation.WorldInitialisation, OperationStatus.Started));

            InitializeEntities();

            LogManager.Instance.Info(LogBuilder.BuildKvpMessage(Operation.WorldInitialisation, OperationStatus.Success));
        }

        /// <summary>
        /// Checks wether the specified provinces share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified provinces share a border, <c>false</c> otherwise.</returns>
        /// <param name="province1Id">First province identifier.</param>
        /// <param name="province2Id">Second province identifier.</param>
        public bool ProvinceBordersProvince(string province1Id, string province2Id)
        {
            return borders.Values.Any(x => (x.SourceProvinceId == province1Id && x.TargetProvinceId == province2Id) ||
                                           (x.SourceProvinceId == province2Id && x.TargetProvinceId == province1Id));
        }

        /// <summary>
        /// Checks wether a province has empty holding slots.
        /// </summary>
        /// <returns><c>true</c>, if the province has empty holding slots, <c>false</c> otherwise.</returns>
        /// <param name="provinceId">Province identifier.</param>
        public bool ProvinceHasEmptyHoldingSlots(string provinceId)
        {
            return holdings.Values.Count(h => h.ProvinceId == provinceId && h.Type == HoldingType.Empty) > 0;
        }

        /// <summary>
        /// Checks wether the specified provinces share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified provinces share a border, <c>false</c> otherwise.</returns>
        /// <param name="faction1Id">First faction identifier.</param>
        /// <param name="faction2Id">Second faction identifier.</param>
        public bool FactionBordersFaction(string faction1Id, string faction2Id)
        {
            List<Province> faction1Provinces = GetFactionProvinces(faction1Id).ToList();
            List<Province> faction2Provinces = GetFactionProvinces(faction2Id).ToList();
            
            return faction1Provinces.Any(r1 => faction2Provinces.Any(r2 => ProvinceBordersProvince(r1.Id, r2.Id)));
        }

        /// <summary>
        /// Checks wether the specified faction shares a border with the specified province.
        /// </summary>
        /// <returns><c>true</c>, if the faction share a border with that province, <c>false</c> otherwise.</returns>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="provinceId">Province identifier.</param>
        public bool FactionBordersProvince(string factionId, string provinceId)
        {
            return GetFactionProvinces(factionId).Any(r => ProvinceBordersProvince(r.Id, provinceId));
        }

        /// <summary>
        /// Returns the faction identifier at the given location.
        /// </summary>
        /// <returns>The faction identifier.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public string FactionIdAtLocation(int x, int y)
        {
            return provinces[world.Tiles[x, y].ProvinceId].FactionId;
        }

        /// <summary>
        /// Transfers the specified province to the specified faction.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        public void TransferProvince(string provinceId, string factionId)
        {
            provinces[provinceId].FactionId = factionId;
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
        => armies.Values.Where(a => a.FactionId == factionId)
                        .Sum(a => a.Size);

        /// <summary>
        /// Gets the faction capital.
        /// </summary>
        /// <returns>Province.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public Province GetFactionCapital(string factionId)
        => provinces.Values.FirstOrDefault(r => r.FactionId == factionId &&
                                              r.SovereignFactionId == factionId &&
                                              r.Type == ProvinceType.Capital);

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
                    if (provinces[world.Tiles[x, y].ProvinceId].FactionId != factionId)
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
                    if (provinces[world.Tiles[x, y].ProvinceId].FactionId != factionId)
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
        /// Gets the armies of a faction.
        /// </summary>
        /// <returns>The armies.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Army> GetFactionArmies(string factionId)
        => armies.Values.Where(a => a.FactionId == factionId);

        /// <summary>
        /// Gets the holdings of a faction.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Holding> GetFactionHoldings(string factionId)
        => holdings.Values.Where(h => h.Type != HoldingType.Empty &&
                                      provinces[h.ProvinceId].FactionId == factionId);

        /// <summary>
        /// Gets the provinces of a faction.
        /// </summary>
        /// <returns>The provinces.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Province> GetFactionProvinces(string factionId)
        => provinces.Values.Where(r => r.FactionId == factionId);

        /// <summary>
        /// Gets the relations of a faction.
        /// </summary>
        /// <returns>The relations of a faction.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Relation> GetFactionRelations(string factionId)
        => relations.Values.Where(r => r.SourceFactionId == factionId &&
                                       r.SourceFactionId != r.TargetFactionId);

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
        /// Gets the provinces.
        /// </summary>
        /// <returns>The provinces.</returns>
        public IEnumerable<Province> GetProvinces()
        => provinces.Values;

        /// <summary>
        /// Gets the holdings of a province.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="provinceId">Province identifier.</param>
        public IEnumerable<Holding> GetProvinceHoldings(string provinceId)
        => holdings.Values.Where(h => h.Type != HoldingType.Empty &&
                                      h.ProvinceId == provinceId);

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
        /// Gets the world.
        /// </summary>
        /// <returns>The world.</returns>
        public World GetWorld()
        => world;

        /// <summary>
        /// Adds the specified holding type in a province.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        /// <param name="holdingType">Holding type.</param>
        public void AddHolding(string provinceId, HoldingType holdingType)
        {
            Holding emptySlot = holdings.Values.FirstOrDefault(h => h.ProvinceId == provinceId &&
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
            IProvinceRepository provinceRepository = new ProvinceRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "provinces.xml"));
            IResourceRepository resourceRepository = new ResourceRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "resources.xml"));
            IUnitRepository unitRepository = new UnitRepository(Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "units.xml"));
            IWorldRepository worldRepository = new WorldRepository(ApplicationPaths.WorldsDirectory);

            IEnumerable<Biome> biomeList = biomeRepository.GetAll().ToDomainModels();
            IEnumerable<Border> borderList = borderRepository.GetAll().ToDomainModels();
            IEnumerable<Culture> cultureList = cultureRepository.GetAll().ToDomainModels();
            IEnumerable<Faction> factionList = factionRepository.GetAll().ToDomainModels();
            IEnumerable<Flag> flagList = flagRepository.GetAll().ToDomainModels();
            IEnumerable<Province> provinceList = provinceRepository.GetAll().ToDomainModels();
            IEnumerable<Resource> resourceList = resourceRepository.GetAll().ToDomainModels();
            IEnumerable<Unit> unitList = unitRepository.GetAll().ToDomainModels();

            armies = new ConcurrentDictionary<Tuple<string, string>, Army>();
            biomes = new ConcurrentDictionary<string, Biome>(biomeList.ToDictionary(biome => biome.Id, biome => biome));
            borders = new ConcurrentDictionary<Tuple<string, string>, Border>(borderList.ToDictionary(border => new Tuple<string, string>(border.SourceProvinceId, border.TargetProvinceId), border => border));
            cultures = new ConcurrentDictionary<string, Culture>(cultureList.ToDictionary(culture => culture.Id, culture => culture));
            factions = new ConcurrentDictionary<string, Faction>(factionList.ToDictionary(faction => faction.Id, faction => faction));
            flags = new ConcurrentDictionary<string, Flag>(flagList.ToDictionary(flag => flag.Id, flag => flag));
            holdings = new ConcurrentDictionary<string, Holding>();
            provinces = new ConcurrentDictionary<string, Province>(provinceList.ToDictionary(province => province.Id, province => province));
            relations = new ConcurrentDictionary<Tuple<string, string>, Relation>();
            resources = new ConcurrentDictionary<string, Resource>(resourceList.ToDictionary(resource => resource.Id, resource => resource));
            units = new ConcurrentDictionary<string, Unit>(unitList.ToDictionary(unit => unit.Id, unit => unit));
            world = worldRepository.Get(worldId).ToDomainModel();
        }

        // TODO: Parallelise this
        void GenerateBorders()
        {
            for (int x = 0; x < world.Width; x += 5)
            {
                for (int y = 0; y < world.Height; y += 5)
                {
                    List<string> province2IdVisited = new List<string>();
                    string province1Id = world.Tiles[x, y].ProvinceId;

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

                            string province2Id = world.Tiles[x + dx, y + dy].ProvinceId;

                            if (!province2IdVisited.Contains(province2Id) &&
                                province1Id != province2Id)
                            {
                                SetBorder(province1Id, province2Id);
                                province2IdVisited.Add(province2Id);
                            }
                        }
                    }
                }
            }
        }

        void SetBorder(string province1Id, string province2Id)
        {
            if (ProvinceBordersProvince(province1Id, province2Id))
            {
                return;
            }

            Tuple<string, string> borderKey = new Tuple<string, string>(province1Id, province2Id);
            Border border = new Border
            {
                SourceProvinceId = province1Id,
                TargetProvinceId = province2Id
            };

            borders.AddOrUpdate(borderKey, border);
        }

        void InitializeEntities()
        {
            // Order is important
            provinces.Values.ToList().ForEach(r => InitialiseProvince(r.Id));
            factions.Values.ToList().ForEach(f => InitialiseFaction(f.Id));
        }

        void InitialiseFaction(string factionId)
        {
            Faction faction = factions[factionId];

            if (faction.Id == GameDefines.GAIA_FACTION)
            {
                faction.Alive = false;
                return;
            }

            faction.Wealth = world.StartingWealth;
            faction.Alive = true;

            Parallel.ForEach(units.Values,
                             unit =>
            {
                Tuple<string, string> armyKey = new Tuple<string, string>(faction.Id, unit.Id);
                Army army = new Army
                {
                    FactionId = faction.Id,
                    UnitId = unit.Id,
                    Size = world.StartingTroops
                };

                armies.AddOrUpdate(armyKey, army);
            });

            Parallel.ForEach(factions.Values, f => InitialiseRelation(factionId, f.Id));

            GenerateHoldings(faction.Id);
        }

        void InitialiseProvince(string provinceId)
        {
            Province province = provinces[provinceId];

            if (string.IsNullOrWhiteSpace(province.SovereignFactionId))
            {
                province.SovereignFactionId = province.FactionId;
            }
        }

        void InitialiseRelation(string sourceFactionId, string targetFactionId)
        {
            if (sourceFactionId == targetFactionId ||
                sourceFactionId == GameDefines.GAIA_FACTION ||
                targetFactionId == GameDefines.GAIA_FACTION)
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
            Province capitalProvince = GetFactionCapital(faction.Id);

            int holdingSlotsLeft = world.HoldingSlotsPerFaction;

            INameGenerator nameGenerator = CreateNameGenerator(faction.CultureId);
            nameGenerator.ExcludedStrings.AddRange(factions.Values.Select(f => f.Name));
            nameGenerator.ExcludedStrings.AddRange(holdings.Values.Select(h => h.Name));
            nameGenerator.ExcludedStrings.AddRange(provinces.Values.Select(r => r.Name));

            List<Province> ownedProvinces = GetFactionProvinces(faction.Id).ToList();

            foreach (Province province in ownedProvinces)
            {
                Holding holding = GenerateHolding(nameGenerator, province.Id);

                if (province.Id == capitalProvince.Id)
                {
                    holding.Name = province.Name;
                    holding.Description = $"The government seat castle of {faction.Name}";
                    holding.Type = HoldingType.Castle;
                }

                holdings.AddOrUpdate(holding.Id, holding);
                holdingSlotsLeft -= 1;
            }

            while (holdingSlotsLeft > 0)
            {
                Province province = ownedProvinces.GetRandomElement();
                Holding holding = GenerateHolding(nameGenerator, province.Id);

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

        Holding GenerateHolding(INameGenerator generator, string provinceId)
        {
            Province province = provinces[provinceId];
            Array holdingTypes = Enum.GetValues(typeof(HoldingType));

            HoldingType holdingType = (HoldingType)holdingTypes.GetValue(random.Next(1, holdingTypes.Length));
            string name = generator.GenerateName();

            Holding holding = new Holding
            {
                Id = $"h_{name.Replace(" ", "_").ToLower()}",
                ProvinceId = province.Id,
                Name = name,
                Description = $"The {name} {holdingType.ToString().ToLower()}", // TODO: Better description
                Type = holdingType
            };

            // TODO: Make sure this never happens and then remove this workaround
            while (holdings.Values.Any(h => h.Id == holding.Id))
            {
                return GenerateHolding(generator, province.Id);
            }

            return holding;
        }
    }
}
