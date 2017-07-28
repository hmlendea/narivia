using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// World Tile mapping extensions for converting between entities and domain models.
    /// </summary>
    static class WorldTileMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="worldTileEntity">World Tile entity.</param>
        internal static WorldTile ToDomainModel(this WorldTileEntity worldTileEntity)
        {
            WorldTile worldTile = new WorldTile
            {
                BiomeId = worldTileEntity.BiomeId,
                RegionId = worldTileEntity.RegionId
            };

            return worldTile;
        }

        /// <summary>
        /// Converts the domail model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="worldTile">World Tile.</param>
        internal static WorldTileEntity ToEntity(this WorldTile worldTile)
        {
            WorldTileEntity worldEntity = new WorldTileEntity
            {
                BiomeId = worldTile.BiomeId,
                RegionId = worldTile.RegionId
            };

            return worldEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="worldTileEntities">World Tile entities.</param>
        internal static IEnumerable<WorldTile> ToDomainModels(this IEnumerable<WorldTileEntity> worldTileEntities)
        {
            IEnumerable<WorldTile> worldTiles = worldTileEntities.Select(worldTileEntity => worldTileEntity.ToDomainModel());

            return worldTiles;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="worldTiles">World Tiles.</param>
        internal static IEnumerable<WorldTileEntity> ToEntities(this IEnumerable<WorldTile> worldTiles)
        {
            IEnumerable<WorldTileEntity> worldTileEntities = worldTiles.Select(worldTile => worldTile.ToEntity());

            return worldTileEntities;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="worldTileEntities">World Tile entities.</param>
        internal static WorldTile[,] ToDomainModels(this WorldTileEntity[,] worldTileEntities)
        {
            int w = worldTileEntities.GetLength(0);
            int h = worldTileEntities.GetLength(1);

            WorldTile[,] worldTiles = new WorldTile[w, h];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    worldTiles[x, y] = worldTileEntities[x, y].ToDomainModel();
                }
            }
            
            return worldTiles;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="worldTiles">World Tiles.</param>
        internal static WorldTileEntity[,] ToEntities(this WorldTile[,] worldTiles)
        {
            int w = worldTiles.GetLength(0);
            int h = worldTiles.GetLength(1);

            WorldTileEntity[,] worldTileEntities = new WorldTileEntity[w, h];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    worldTileEntities[x, y] = worldTiles[x, y].ToEntity();
                }
            }

            return worldTileEntities;
        }
    }
}
