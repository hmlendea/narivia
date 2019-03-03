using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using NuciXNA.DataAccess.IO;
using NuciXNA.DataAccess.Repositories;
using NuciXNA.Primitives;

using Narivia.DataAccess.DataObjects;

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

            return worldEntity;
        }

        /// <summary>
        /// Gets all the worlds.
        /// </summary>
        /// <returns>The worlds</returns>
        public IEnumerable<WorldEntity> GetAll()
        {
            ConcurrentBag<WorldEntity> worldEntities = new ConcurrentBag<WorldEntity>();

            foreach (string worldId in Directory.GetDirectories(worldsDirectory))
            {
                // TODO: Don't load if the world.xml file is not present
                worldEntities.Add(Get(worldId));
            }

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
        /// <param name="worldEntity">World.</param>
        public void Remove(WorldEntity worldEntity)
        {
            Remove(worldEntity.Id);
        }

        WorldTileEntity[,] LoadWorldTiles(string worldId)
        {
            Dictionary<Colour, string> provinceColourIds = new Dictionary<Colour, string>();
            Dictionary<Colour, string> terrainColourIds = new Dictionary<Colour, string>();

            string provincesPath = Path.Combine(worldsDirectory, worldId, "provinces.xml");
            string terrainsPath = Path.Combine(worldsDirectory, worldId, "terrains.xml");

            IRepository<string, ProvinceEntity> provinceRepository = new ProvinceRepository(provincesPath);
            IRepository<string, TerrainEntity> terrainRepository = new TerrainRepository(terrainsPath);

            Dictionary<string, ProvinceEntity> provinces = provinceRepository.GetAll().ToDictionary(x => x.Id, x => x);
            Dictionary<string, TerrainEntity> terrains = terrainRepository.GetAll().ToDictionary(x => x.Id, x => x);

            foreach (ProvinceEntity province in provinces.Values)
            {
                provinceColourIds.Add(Colour.FromHexadecimal(province.ColourHexadecimal), province.Id);
            }

            foreach (TerrainEntity terrain in terrains.Values)
            {
                terrainColourIds.Add(Colour.FromHexadecimal(terrain.ColourHexadecimal), terrain.Id);
            }

            Bitmap heightsBitmap = Bitmap.Load(Path.Combine(worldsDirectory, worldId, "world_heights.png"));
            Bitmap provinceBitmap = Bitmap.Load(Path.Combine(worldsDirectory, worldId, "world_provinces.png"));
            Bitmap riversBitmap = Bitmap.Load(Path.Combine(worldsDirectory, worldId, "world_rivers.png"));
            Bitmap terrainBitmap = Bitmap.Load(Path.Combine(worldsDirectory, worldId, "world_terrains.png"));

            Size2D worldSize = terrainBitmap.Size;

            if (worldSize.Width != heightsBitmap.Size.Width ||
                worldSize.Width != provinceBitmap.Size.Width ||
                worldSize.Width != riversBitmap.Size.Width ||
                worldSize.Width != terrainBitmap.Size.Width ||
                worldSize.Height != heightsBitmap.Size.Height ||
                worldSize.Height != provinceBitmap.Size.Height ||
                worldSize.Height != riversBitmap.Size.Height ||
                worldSize.Height != terrainBitmap.Size.Height)
            {
                // TODO: Dedicated exception type
                throw new Exception("World bitmaps sizes do not match!");
            }

            WorldTileEntity[,] tiles = new WorldTileEntity[provinceBitmap.Size.Width, provinceBitmap.Size.Height];

            DateTime start1 = DateTime.Now;
            for (int y = 0; y < worldSize.Height; y++)
            {
                for (int x = 0; x < worldSize.Width; x++)
                {
                    Colour heightColour = heightsBitmap.GetPixel(x, y);
                    Colour riverColour = riversBitmap.GetPixel(x, y);
                    Colour provinceColour = provinceBitmap.GetPixel(x, y);
                    Colour terrainColour = terrainBitmap.GetPixel(x, y);

                    tiles[x, y] = new WorldTileEntity();
                    tiles[x, y].ProvinceId = provinceColourIds[provinceColour];
                    tiles[x, y].TerrainId = terrainColourIds[terrainColour];
                    tiles[x, y].TerrainIds.Add(terrainColourIds[terrainColour]);
                    tiles[x, y].HasRiver = riverColour.Equals(0, 0, 255);

                    if (heightColour.B == 255 || heightColour.B == 0)
                    {
                        tiles[x, y].HasWater = true;
                        tiles[x, y].Altitude = 0;
                    }
                    else
                    {
                        tiles[x, y].Altitude = (byte)((heightColour.R + heightColour.G + heightColour.B) / 3);
                    }

                    if (tiles[x, y].HasRiver && tiles[x, y].HasWater)
                    {
                        // TODO: Dedicated exception type
                        throw new Exception("A tile cannot have both a river and water at the same time!");
                    }
                }
            }

            // TODO: Optimise all this
            for (int y = 0; y < worldSize.Height; y++)
            {
                for (int x = 0; x < worldSize.Width; x++)
                {
                    TerrainEntity terrain = terrains[tiles[x, y].TerrainId];

                    // TODO: Don't grow by 2 unnecessarily. 1 would be ideal
                    int order = 2;

                    // TODO: REMOVE THIS !!!
                    if (terrain.Id == "water")
                    {
                        continue;
                    }

                    for (int dy = -order; dy <= order; dy++)
                    {
                        for (int dx = -order; dx <= order; dx++)
                        {
                            if (dx == dy || dx + dy == 0)
                            {
                                continue;
                            }

                            int destX = dx + x;
                            int destY = dy + y;

                            if (destX >= 0 && destX < worldSize.Width &&
                                destY >= 0 && destY < worldSize.Height &&
                                !tiles[destX, destY].TerrainIds.Contains(terrain.Id))
                            {
                                tiles[destX, destY].TerrainIds.Add(terrain.Id);
                                tiles[destX, destY].TerrainIds.Sort((id1, id2) => terrains[id1].ZIndex.CompareTo(terrains[id2].ZIndex));
                            }
                        }
                    }
                }
            }

            provinceBitmap.Dispose();
            riversBitmap.Dispose();
            terrainBitmap.Dispose();

            return tiles;
        }
    }
}
