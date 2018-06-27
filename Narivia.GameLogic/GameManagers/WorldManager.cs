using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

using NuciXNA.DataAccess.Repositories;
using NuciLog;
using NuciLog.Enumerations;

using Narivia.Common.Extensions;
using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.GameLogic.Mapping;
using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Repositories;
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

        string worldId;
        World world;

        ConcurrentDictionary<string, Border> borders;
        ConcurrentDictionary<string, Culture> cultures;
        ConcurrentDictionary<string, Faction> factions;
        ConcurrentDictionary<string, Flag> flags;
        ConcurrentDictionary<string, Province> provinces;
        ConcurrentDictionary<string, Resource> resources;
        ConcurrentDictionary<string, Terrain> terrains;

        public int HoldingSlotsPerFaction
            => world.HoldingSlotsPerFaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldManager"/> class.
        /// </summary>
        /// <param name="worldId">World identifier.</param>
        public WorldManager(string worldId)
        {
            random = new Random();
            this.worldId = worldId;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            LogManager.Instance.Info(Operation.WorldLoading, OperationStatus.Started);

            string bordersPath = Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "borders.xml");
            string culturesPath = Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "cultures.xml");
            string factionsPath = Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "factions.xml");
            string flagsPath = Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "flags.xml");
            string provincesPath = Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "provinces.xml");
            string resourcesPath = Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "resources.xml");
            string terrainsPath = Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "terrains.xml");

            IRepository<string, BorderEntity> borderRepository = new BorderRepository(bordersPath);
            IRepository<string, CultureEntity> cultureRepository = new CultureRepository(culturesPath);
            IRepository<string, FactionEntity> factionRepository = new FactionRepository(factionsPath);
            IRepository<string, FlagEntity> flagRepository = new FlagRepository(flagsPath);
            IRepository<string, ProvinceEntity> provinceRepository = new ProvinceRepository(provincesPath);
            IRepository<string, ResourceEntity> resourceRepository = new ResourceRepository(resourcesPath);
            IRepository<string, TerrainEntity> terrainRepository = new TerrainRepository(terrainsPath);
            IRepository<string, WorldEntity> worldRepository = new WorldRepository(ApplicationPaths.WorldsDirectory);

            IEnumerable<Border> borderList = borderRepository.GetAll().ToDomainModels();
            IEnumerable<Culture> cultureList = cultureRepository.GetAll().ToDomainModels();
            IEnumerable<Faction> factionList = factionRepository.GetAll().ToDomainModels();
            IEnumerable<Flag> flagList = flagRepository.GetAll().ToDomainModels();
            IEnumerable<Province> provinceList = provinceRepository.GetAll().ToDomainModels();
            IEnumerable<Resource> resourceList = resourceRepository.GetAll().ToDomainModels();
            IEnumerable<Terrain> terrainList = terrainRepository.GetAll().ToDomainModels();

            borders = new ConcurrentDictionary<string, Border>(borderList.ToDictionary(border => $"{border.SourceProvinceId}:{border.TargetProvinceId}", border => border));
            cultures = new ConcurrentDictionary<string, Culture>(cultureList.ToDictionary(culture => culture.Id, culture => culture));
            factions = new ConcurrentDictionary<string, Faction>(factionList.ToDictionary(faction => faction.Id, faction => faction));
            flags = new ConcurrentDictionary<string, Flag>(flagList.ToDictionary(flag => flag.Id, flag => flag));
            provinces = new ConcurrentDictionary<string, Province>(provinceList.ToDictionary(province => province.Id, province => province));
            resources = new ConcurrentDictionary<string, Resource>(resourceList.ToDictionary(resource => resource.Id, resource => resource));
            terrains = new ConcurrentDictionary<string, Terrain>(terrainList.ToDictionary(terrain => terrain.Id, terrain => terrain));
            world = worldRepository.Get(worldId).ToDomainModel();

            GenerateBorders();

            LogManager.Instance.Info(Operation.WorldLoading, OperationStatus.Success);
        }

        public void UnloadContent()
        {

        }

        /// <summary>
        /// Checks wether the specified provinces share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified provinces share a border, <c>false</c> otherwise.</returns>
        /// <param name="province1Id">First province identifier.</param>
        /// <param name="province2Id">Second province identifier.</param>
        public bool ProvinceBordersProvince(string province1Id, string province2Id)
        {
            return borders.Values.Any(x =>
                (x.SourceProvinceId == province1Id && x.TargetProvinceId == province2Id) ||
                (x.SourceProvinceId == province2Id && x.TargetProvinceId == province1Id));
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
            => GetFactionProvinces(factionId).Any(r => ProvinceBordersProvince(r.Id, provinceId));

        /// <summary>
        /// Returns the faction identifier at the given location.
        /// </summary>
        /// <returns>The faction identifier.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public string FactionIdAtLocation(int x, int y)
            => provinces[world.Tiles[x, y].ProvinceId].FactionId;

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
        /// Gets the borders.
        /// </summary>
        /// <returns>The borders.</returns>
        public IEnumerable<Border> GetBorders()
            => borders.Values;

        public Culture GetCulture(string cultureId)
            => cultures[cultureId];

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

        public Faction GetFaction(string factionId)
            => factions[factionId];

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
        /// Gets the provinces of a faction.
        /// </summary>
        /// <returns>The provinces.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Province> GetFactionProvinces(string factionId)
            => provinces.Values.Where(r => r.FactionId == factionId);

        /// <summary>
        /// Gets the flags.
        /// </summary>
        /// <returns>The flags.</returns>
        public IEnumerable<Flag> GetFlags()
            => flags.Values;

        public Province GetProvince(string provinceId)
            => provinces[provinceId];

        /// <summary>
        /// Gets the provinces.
        /// </summary>
        /// <returns>The provinces.</returns>
        public IEnumerable<Province> GetProvinces()
            => provinces.Values;

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <returns>The resource.</returns>
        /// <param name="resourceId">Resource identifier.</param>
        public Resource GetResource(string resourceId)
            => resources[resourceId];

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <returns>The resources.</returns>
        public IEnumerable<Resource> GetResources()
            => resources.Values;

        /// <summary>
        /// Gets the terrains.
        /// </summary>
        /// <returns>The terrains.</returns>
        public IEnumerable<Terrain> GetTerrains()
            => terrains.Values;

        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <returns>The world.</returns>
        public World GetWorld()
            => world;

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

            Border border = new Border
            {
                Id = $"{province1Id}:{province2Id}",
                SourceProvinceId = province1Id,
                TargetProvinceId = province2Id
            };

            borders.AddOrUpdate(border.Id, border);
        }

        public void InitialiseProvince(string provinceId)
        {
            Province province = provinces[provinceId];

            if (string.IsNullOrWhiteSpace(province.SovereignFactionId))
            {
                province.SovereignFactionId = province.FactionId;
            }
        }

        public void InitialiseFaction(string factionId)
        {
            Faction faction = factions[factionId];

            if (faction.Id == GameDefines.GaiaFactionIdentifier)
            {
                faction.Alive = false;
                return;
            }

            faction.Wealth = world.StartingWealth;
            faction.Alive = true;
        }
    }
}
