using Narivia.Common.Extensions;
using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.IO;
using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TiledSharp;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// World repository implementation.
    /// </summary>
    public class WorldRepository : IRepository<string, WorldEntity>
    {
        readonly string worldsDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldRepository"/> class.
        /// </summary>
        /// <param name="worldsDirectory">File name.</param>
        public WorldRepository(string worldsDirectory)
        {
            this.worldsDirectory = worldsDirectory;
        }

        /// <summary>
        /// Adds the specified world.
        /// </summary>
        /// <param name="worldEntity">World.</param>
        public void Add(WorldEntity worldEntity)
        {
            // TODO: Implement this
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the world with the specified identifier.
        /// </summary>
        /// <returns>The world.</returns>
        /// <param name="id">Identifier.</param>
        public WorldEntity Get(string id)
        {
            WorldEntity worldEntity;
            string worldFile = Path.Combine(worldsDirectory, id, "world.xml");

            using (TextReader reader = new StreamReader(worldFile))
            {
                XmlSerializer xml = new XmlSerializer(typeof(WorldEntity));
                worldEntity = (WorldEntity)xml.Deserialize(reader);
            }

            worldEntity.Tiles = LoadWorldTiles(id);
            worldEntity.Layers = LoadWorldGeoLayers(id);
            
            return worldEntity;
        }

        /// <summary>
        /// Gets all the worlds.
        /// </summary>
        /// <returns>The worlds</returns>
        public IEnumerable<WorldEntity> GetAll()
        {
            ConcurrentBag<WorldEntity> worldEntities = new ConcurrentBag<WorldEntity>();

            Parallel.ForEach(Directory.GetDirectories(worldsDirectory),
                             worldId => worldEntities.Add(Get(worldId)));

            return worldEntities;
        }

        /// <summary>
        /// Updates the specified world.
        /// </summary>
        /// <param name="worldEntity">World.</param>
        public void Update(WorldEntity worldEntity)
        {
            string worldFile = Path.Combine(worldsDirectory, worldEntity.Id, "world.xml");

            using (TextWriter writer = new StreamWriter(worldFile))
            {
                XmlSerializer xml = new XmlSerializer(typeof(WorldEntity));
                xml.Serialize(writer, worldEntity);
            }

            // TODO: Save the ProvinceMap and TerrainMap as well
        }

        /// <summary>
        /// Removes the world with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            Directory.Delete(Path.Combine(worldsDirectory, id));
        }

        /// <summary>
        /// Removes the specified world.
        /// </summary>
        /// <param name="world">World.</param>
        public void Remove(WorldEntity worldEntity)
        {
            Remove(worldEntity.Id);
        }

        WorldTileEntity[,] LoadWorldTiles(string worldId)
        {
            ConcurrentDictionary<int, string> provinceColourIds = new ConcurrentDictionary<int, string>();
            ConcurrentDictionary<int, string> terrainColourIds = new ConcurrentDictionary<int, string>();

            IRepository<string, TerrainEntity> terrainRepository = new TerrainRepository(Path.Combine(worldsDirectory, worldId, "terrains.xml"));
            IRepository<string, ProvinceEntity> provinceRepository = new ProvinceRepository(Path.Combine(worldsDirectory, worldId, "provinces.xml"));

            Parallel.ForEach(terrainRepository.GetAll(), b => terrainColourIds.AddOrUpdate(ColorTranslator.FromHtml(b.ColourHexadecimal).ToArgb(), b.Id));
            Parallel.ForEach(provinceRepository.GetAll(), r => provinceColourIds.AddOrUpdate(ColorTranslator.FromHtml(r.ColourHexadecimal).ToArgb(), r.Id));

            FastBitmap terrainBitmap = new FastBitmap(Path.Combine(worldsDirectory, worldId, "world_terrains.png"));
            FastBitmap provinceBitmap = new FastBitmap(Path.Combine(worldsDirectory, worldId, "world_provinces.png"));
            FastBitmap riversBitmap = new FastBitmap(Path.Combine(worldsDirectory, worldId, "world_rivers.png"));
            FastBitmap heightsBitmap = new FastBitmap(Path.Combine(worldsDirectory, worldId, "world_heights.png"));

            Point worldSize = new Point(Math.Max(terrainBitmap.Width, provinceBitmap.Width),
                                        Math.Max(terrainBitmap.Height, provinceBitmap.Height));

            WorldTileEntity[,] tiles = new WorldTileEntity[provinceBitmap.Width, provinceBitmap.Height];

            Parallel.For(0, worldSize.Y, y => Parallel.For(0, worldSize.X, x => tiles[x, y] = new WorldTileEntity()));

            terrainBitmap.LockBits();
            provinceBitmap.LockBits();
            riversBitmap.LockBits();

            Parallel.For(0, worldSize.Y, y => Parallel.For(0, worldSize.X, x =>
            {
                int terrainArgb = terrainBitmap.GetPixel(x, y).ToArgb();
                int provinceArgb = provinceBitmap.GetPixel(x, y).ToArgb();
                Color riverColour = riversBitmap.GetPixel(x, y);
                Color heightColour = heightsBitmap.GetPixel(x, y);

                tiles[x, y].TerrainId = terrainColourIds[terrainArgb];
                tiles[x, y].ProvinceId = provinceColourIds[provinceArgb];
                tiles[x, y].HasRiver = riverColour == Color.Blue;

                if (heightColour == Color.Blue ||
                    heightColour == Color.Black)
                {
                    tiles[x, y].HasWater = true;
                    tiles[x, y].Altitude = 0;
                }
                else
                {
                    tiles[x, y].Altitude = (byte)((heightColour.R + heightColour.G + heightColour.B) / 3);
                }
            }));

            terrainBitmap.Dispose();
            provinceBitmap.Dispose();
            riversBitmap.Dispose();
            heightsBitmap.Dispose();

            return tiles;
        }

        List<WorldGeoLayerEntity> LoadWorldGeoLayers(string worldId)
        {
            TmxMap tmxMap = new TmxMap(Path.Combine(worldsDirectory, worldId, "world.tmx"));
            ConcurrentBag<WorldGeoLayerEntity> layers = new ConcurrentBag<WorldGeoLayerEntity>();

            Parallel.ForEach(tmxMap.Layers, tmxLayer => layers.Add(ProcessTmxLayer(tmxMap, tmxLayer)));

            return layers.OrderBy(l => tmxMap.Layers.IndexOf(tmxMap.Layers.FirstOrDefault(x => x.Name == l.Name)))
                         .ToList();
        }

        WorldGeoLayerEntity ProcessTmxLayer(TmxMap tmxMap, TmxLayer tmxLayer)
        {
            string tilesetName = tmxLayer.Properties["tileset"];

            if (string.IsNullOrWhiteSpace(tilesetName) ||
                tmxMap.Tilesets.ToList().FindIndex(t => t.Name == tilesetName) < 0)
            {
                throw new InvalidEntityFieldException(nameof(WorldGeoLayerEntity.Tileset), tmxLayer.Name, nameof(WorldGeoLayerEntity));
            }

            TmxTileset tmxTileset = tmxMap.Tilesets[tilesetName];

            WorldGeoLayerEntity layer = new WorldGeoLayerEntity
            {
                Name = tmxLayer.Name,
                Tiles = new int[tmxMap.Width, tmxMap.Height],
                Tileset = tmxTileset.Name,
                Opacity = (float)tmxLayer.Opacity,
                Visible = tmxLayer.Visible
            };

            Parallel.ForEach(tmxLayer.Tiles, tile => layer.Tiles[tile.X, tile.Y] = Math.Max(-1, tile.Gid - tmxTileset.FirstGid));

            return layer;
        }
    }
}
