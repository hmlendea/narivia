using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.BusinessLogic.Mapping
{
    internal static class HoldingMappingExtensions
    {
        internal static Holding ToDomainModel(this HoldingEntity holdingEntity)
        {
            Holding holding = new Holding
            {
                Id = holdingEntity.Id,
                Name = holdingEntity.Name,
                Description = holdingEntity.Description,
                Type = (HoldingType)Enum.Parse(typeof(HoldingType), holdingEntity.Type)
            };

            return holding;
        }

        internal static HoldingEntity ToEntity(this Holding holding)
        {
            HoldingEntity holdingEntity = new HoldingEntity
            {
                Id = holding.Id,
                Name = holding.Name,
                Description = holding.Description,
                Type = holding.Type.ToString()
            };

            return holdingEntity;
        }

        internal static IEnumerable<Holding> ToDomainModels(this IEnumerable<HoldingEntity> holdingEntities)
        {
            IEnumerable<Holding> holdings = holdingEntities.Select(holdingEntity => holdingEntity.ToDomainModel());

            return holdings;
        }

        internal static IEnumerable<HoldingEntity> ToEntities(this IEnumerable<Holding> holdings)
        {
            IEnumerable<HoldingEntity> holdingEntities = holdings.Select(holding => holding.ToEntity());

            return holdingEntities;
        }
    }
}
