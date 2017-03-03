using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.BusinessLogic.Mapping
{
    internal static class UnitMappingExtensions
    {
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

        internal static IEnumerable<Unit> ToDomainModels(this IEnumerable<UnitEntity> unitEntities)
        {
            IEnumerable<Unit> units = unitEntities.Select(unitEntity => unitEntity.ToDomainModel());

            return units;
        }

        internal static IEnumerable<UnitEntity> ToEntities(this IEnumerable<Unit> units)
        {
            IEnumerable<UnitEntity> unitEntities = units.Select(unit => unit.ToEntity());

            return unitEntities;
        }
    }
}
