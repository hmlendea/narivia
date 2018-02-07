using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// Terrain mapping extensions for converting between entities and domain models.
    /// </summary>
    static class TerrainMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="terrainEntity">Terrain entity.</param>
        internal static Terrain ToDomainModel(this TerrainEntity terrainEntity)
        {
            Terrain terrain = new Terrain
            {
                Id = terrainEntity.Id,
                Name = terrainEntity.Name,
                Description = terrainEntity.Description,
                Colour = ColorTranslator.FromHtml(terrainEntity.ColourHexadecimal)
            };

            return terrain;
        }

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="terrain">Terrain.</param>
        internal static TerrainEntity ToEntity(this Terrain terrain)
        {
            TerrainEntity terrainEntity = new TerrainEntity
            {
                Id = terrain.Id,
                Name = terrain.Name,
                Description = terrain.Description,
                ColourHexadecimal = ColorTranslator.ToHtml(terrain.Colour)
            };

            return terrainEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="terrainEntities">Terrain entities.</param>
        internal static IEnumerable<Terrain> ToDomainModels(this IEnumerable<TerrainEntity> terrainEntities)
        {
            IEnumerable<Terrain> terrains = terrainEntities.Select(terrainEntity => terrainEntity.ToDomainModel());

            return terrains;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="terrains">Terrains.</param>
        internal static IEnumerable<TerrainEntity> ToEntities(this IEnumerable<Terrain> terrains)
        {
            IEnumerable<TerrainEntity> terrainEntities = terrains.Select(terrain => terrain.ToEntity());

            return terrainEntities;
        }
    }
}
