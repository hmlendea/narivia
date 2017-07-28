using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

using TiledSharp;

using Narivia.Common.Extensions;
using Narivia.Common.Helpers;
using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Repositories.Interfaces;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// World repository implementation.
    /// </summary>
    public class WorldRepository : IWorldRepository
    {
        string worldsDirectory;

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

            worldEntity.Tiles = new WorldTileEntity[worldEntity.Width, worldEntity.Height];

            int worldWidth = worldEntity.Tiles.GetLength(0);
            int worldHeight = worldEntity.Tiles.GetLength(1);

            for (int y = 0; y < worldWidth; y ++)
            {
                for (int x = 0; x < worldHeight; x++)
                {
                    worldEntity.Tiles[x, y] = new WorldTileEntity();
                }
            }

            ConcurrentDictionary<Color, string> regionColourIds = new ConcurrentDictionary<Color, string>();
            ConcurrentDictionary<Color, string> biomeColourIds = new ConcurrentDictionary<Color, string>();

            IBiomeRepository biomeRepository = new BiomeRepository(Path.Combine(worldsDirectory, id, "biomes.xml"));
            IRegionRepository regionRepository = new RegionRepository(Path.Combine(worldsDirectory, id, "regions.xml"));

            Parallel.ForEach(biomeRepository.GetAll(), b => biomeColourIds.AddOrUpdate(ColorTranslator.FromHtml(b.ColourHexadecimal), b.Id));
            Parallel.ForEach(regionRepository.GetAll(), r => regionColourIds.AddOrUpdate(ColorTranslator.FromHtml(r.ColourHexadecimal), r.Id));
            
            using (FastBitmap bmp = new FastBitmap(Path.Combine(worldsDirectory, id, "biomes_map.png")))
            {
                Parallel.For(0, worldEntity.Height,
                             y => Parallel.For(0, worldEntity.Width,
                                               x => worldEntity.Tiles[x, y].RegionId = biomeColourIds[bmp.GetPixel(x, y)]));
            }

            using (FastBitmap bmp = new FastBitmap(Path.Combine(worldsDirectory, id, "map.png")))
            {
                Parallel.For(0, worldEntity.Height,
                             y => Parallel.For(0, worldEntity.Width,
                                               x => worldEntity.Tiles[x, y].RegionId = regionColourIds[bmp.GetPixel(x, y)]));
            }

            TmxMap tmxMap = new TmxMap(Path.Combine(worldsDirectory, id, "world.tmx"));

            worldEntity.Layers = new List<WorldGeoLayerEntity>();

            // TODO: Consider parallelisation
            foreach (TmxLayer tmxLayer in tmxMap.Layers)
            {
                WorldGeoLayerEntity layer = new WorldGeoLayerEntity();
                layer.Tiles = new int[worldWidth, worldHeight];

                // TODO: Throw an exception for "The layer does not contain a 'tileset' property"

                string tilesetName = tmxLayer.Properties["tileset"];

                // TODO: Throw an exception for "The specified tileset does not exist"

                layer.Tileset = tilesetName;

                TmxTileset tmxTileset = tmxMap.Tilesets[tilesetName];

                Parallel.ForEach(tmxLayer.Tiles.Where(tile => tile.Gid > 0),
                                 tile => layer.Tiles[tile.X, tile.Y] = (tile.Gid - tmxTileset.FirstGid));

                worldEntity.Layers.Add(layer);
            }

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

            // TODO: Save the RegionMap and BiomeMap as well
        }

        /// <summary>
        /// Removes the world with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            Directory.Delete(Path.Combine(worldsDirectory, id));
        }
    }
}
