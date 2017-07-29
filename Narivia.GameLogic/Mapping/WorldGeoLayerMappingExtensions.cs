using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// World geographic layer mapping extensions for converting between entities and domain models.
    /// </summary>
    static class WorldGeoLayerMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="worldGeoLayerEntity">World geographic layer entity.</param>
        internal static WorldGeoLayer ToDomainModel(this WorldGeoLayerEntity worldGeoLayerEntity)
        {
            WorldGeoLayer worldGeoLayer = new WorldGeoLayer
            {
                Name = worldGeoLayerEntity.Name,
                Tileset = worldGeoLayerEntity.Tileset,
                Tiles = worldGeoLayerEntity.Tiles
            };

            return worldGeoLayer;
        }

        /// <summary>
        /// Converts the domail model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="worldGeoLayer">World geographic layer.</param>
        internal static WorldGeoLayerEntity ToEntity(this WorldGeoLayer worldGeoLayer)
        {
            WorldGeoLayerEntity worldGeoLayerEntity = new WorldGeoLayerEntity
            {
                Name = worldGeoLayer.Name,
                Tileset = worldGeoLayer.Tileset,
                Tiles = worldGeoLayer.Tiles
            };

            return worldGeoLayerEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="worldGeoLayerEntities">World geographic layer entities.</param>
        internal static IEnumerable<WorldGeoLayer> ToDomainModels(this IEnumerable<WorldGeoLayerEntity> worldGeoLayerEntities)
        {
            IEnumerable<WorldGeoLayer> worldGeoLayers = worldGeoLayerEntities.Select(worldTileEntity => worldTileEntity.ToDomainModel());

            return worldGeoLayers;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="worldGeoLayers">World geographic layer.</param>
        internal static IEnumerable<WorldGeoLayerEntity> ToEntities(this IEnumerable<WorldGeoLayer> worldGeoLayers)
        {
            IEnumerable<WorldGeoLayerEntity> worldGeoLayerEntities = worldGeoLayers.Select(worldTile => worldTile.ToEntity());

            return worldGeoLayerEntities;
        }
    }
}
