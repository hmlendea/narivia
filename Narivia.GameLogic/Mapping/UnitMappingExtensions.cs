using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;
using Narivia.Models.Enumerations;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// Unit mapping extensions for converting between entities and domain models.
    /// </summary>
    static class UnitMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="unitEntity">Unit entity.</param>
        internal static Unit ToDomainModel(this UnitEntity unitEntity)
        {
            Unit unit = new Unit
            {
                Id = unitEntity.Id,
                Name = unitEntity.Name,
                Description = unitEntity.Description,
                Type = (UnitType)Enum.Parse(typeof(UnitType), unitEntity.Type),
                Power = unitEntity.Power,
                Health = unitEntity.Health,
                Price = unitEntity.Price,
                Maintenance = unitEntity.Maintenance
            };

            return unit;
        }

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="unit">Unit.</param>
        internal static UnitEntity ToEntity(this Unit unit)
        {
            UnitEntity unitEntity = new UnitEntity
            {
                Id = unit.Id,
                Name = unit.Name,
                Description = unit.Description,
                Type = unit.Type.ToString(),
                Power = unit.Power,
                Health = unit.Health,
                Price = unit.Price,
                Maintenance = unit.Maintenance
            };

            return unitEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="unitEntities">Unit entities.</param>
        internal static IEnumerable<Unit> ToDomainModels(this IEnumerable<UnitEntity> unitEntities)
        {
            IEnumerable<Unit> units = unitEntities.Select(unitEntity => unitEntity.ToDomainModel());

            return units;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="units">Units.</param>
        internal static IEnumerable<UnitEntity> ToEntities(this IEnumerable<Unit> units)
        {
            IEnumerable<UnitEntity> unitEntities = units.Select(unit => unit.ToEntity());

            return unitEntities;
        }
    }
}
