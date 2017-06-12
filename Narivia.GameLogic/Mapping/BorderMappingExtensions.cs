using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    internal static class BorderMappingExtensions
    {
        internal static Border ToDomainModel(this BorderEntity borderEntity)
        {
            Border border = new Border
            {
                Region1Id = borderEntity.Region1Id,
                Region2Id = borderEntity.Region2Id
            };

            return border;
        }

        internal static BorderEntity ToEntity(this Border border)
        {
            BorderEntity borderEntity = new BorderEntity
            {
                Region1Id = border.Region1Id,
                Region2Id = border.Region2Id
            };

            return borderEntity;
        }

        internal static IEnumerable<Border> ToDomainModels(this IEnumerable<BorderEntity> borderEntities)
        {
            IEnumerable<Border> borders = borderEntities.Select(borderEntity => borderEntity.ToDomainModel());

            return borders;
        }

        internal static IEnumerable<BorderEntity> ToEntities(this IEnumerable<Border> borders)
        {
            IEnumerable<BorderEntity> borderEntities = borders.Select(border => border.ToEntity());

            return borderEntities;
        }
    }
}
