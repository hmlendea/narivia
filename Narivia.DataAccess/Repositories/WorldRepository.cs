using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

            worldEntity.BiomeMap = new string[worldEntity.Width, worldEntity.Height];
            worldEntity.RegionMap = new string[worldEntity.Width, worldEntity.Height];

            ConcurrentDictionary<Color, string> regionColourIds = new ConcurrentDictionary<Color, string>();
            ConcurrentDictionary<Color, string> biomeColourIds = new ConcurrentDictionary<Color, string>();

            IBiomeRepository biomeRepository = new BiomeRepository(Path.Combine(worldsDirectory, id, "biomes.xml"));
            IRegionRepository regionRepository = new RegionRepository(Path.Combine(worldsDirectory, id, "regions.xml"));

            Parallel.ForEach(biomeRepository.GetAll(), b => biomeColourIds.AddOrUpdate(ColorTranslator.FromHtml(b.ColourHexadecimal), b.Id));
            Parallel.ForEach(regionRepository.GetAll(), r => regionColourIds.AddOrUpdate(ColorTranslator.FromHtml(r.ColourHexadecimal), r.Id));

            using (FastBitmap bmp = new FastBitmap(Path.Combine(worldsDirectory, id, "map.png")))
            {
                Parallel.For(0, worldEntity.Height,
                             y => Parallel.For(0, worldEntity.Width,
                                               x => worldEntity.RegionMap[x, y] = regionColourIds[bmp.GetPixel(x, y)]));
            }
            
            using (FastBitmap bmp = new FastBitmap(Path.Combine(worldsDirectory, id, "biomes_map.png")))
            {
                Parallel.For(0, worldEntity.Height,
                             y => Parallel.For(0, worldEntity.Width,
                                               x => worldEntity.BiomeMap[x, y] = biomeColourIds[bmp.GetPixel(x, y)]));
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
