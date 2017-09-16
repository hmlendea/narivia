using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// Border mapping extensions for converting between entities and domain models.
    /// </summary>
    static class BorderMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="borderEntity">Border entity.</param>
        internal static Border ToDomainModel(this BorderEntity borderEntity)
        {
            Border border = new Border
            {
                SourceProvinceId = borderEntity.Province1Id,
                TargetProvinceId = borderEntity.Province2Id
            };

            return border;
        }

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="border">Border.</param>
        internal static BorderEntity ToEntity(this Border border)
        {
            BorderEntity borderEntity = new BorderEntity
            {
                Province1Id = border.SourceProvinceId,
                Province2Id = border.TargetProvinceId
            };

            return borderEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="borderEntities">Border entities.</param>
        internal static IEnumerable<Border> ToDomainModels(this IEnumerable<BorderEntity> borderEntities)
        {
            IEnumerable<Border> borders = borderEntities.Select(borderEntity => borderEntity.ToDomainModel());

            return borders;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="borders">Borders.</param>
        internal static IEnumerable<BorderEntity> ToEntities(this IEnumerable<Border> borders)
        {
            IEnumerable<BorderEntity> borderEntities = borders.Select(border => border.ToEntity());

            return borderEntities;
        }
    }
}
