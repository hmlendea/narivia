using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;
using Narivia.Models.Enumerations;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// Holding mapping extensions for converting between entities and domain models.
    /// </summary>
    static class HoldingMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="holdingEntity">Holding entity.</param>
        internal static Holding ToDomainModel(this HoldingEntity holdingEntity)
        {
            Holding holding = new Holding
            {
                Id = holdingEntity.Id,
                Name = holdingEntity.Name,
                Description = holdingEntity.Description,
                Type = (HoldingType)Enum.Parse(typeof(HoldingType), holdingEntity.Type),
                RegionId = holdingEntity.RegionId
            };

            return holding;
        }

        /// <summary>
        /// Converts the domail model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="holding">Holding.</param>
        internal static HoldingEntity ToEntity(this Holding holding)
        {
            HoldingEntity holdingEntity = new HoldingEntity
            {
                Id = holding.Id,
                Name = holding.Name,
                Description = holding.Description,
                Type = holding.Type.ToString(),
                RegionId = holding.RegionId
            };

            return holdingEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="holdingEntities">Holding entities.</param>
        internal static IEnumerable<Holding> ToDomainModels(this IEnumerable<HoldingEntity> holdingEntities)
        {
            IEnumerable<Holding> holdings = holdingEntities.Select(holdingEntity => holdingEntity.ToDomainModel());

            return holdings;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="holdings">Holdings.</param>
        internal static IEnumerable<HoldingEntity> ToEntities(this IEnumerable<Holding> holdings)
        {
            IEnumerable<HoldingEntity> holdingEntities = holdings.Select(holding => holding.ToEntity());

            return holdingEntities;
        }
    }
}
