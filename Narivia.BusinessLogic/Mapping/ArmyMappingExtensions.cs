using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.BusinessLogic.Mapping
{
    internal static class ArmyMappingExtensions
    {
        internal static Army ToDomainModel(this ArmyEntity armyEntity)
        {
            Army army = new Army
            {
                FactionId = armyEntity.FactionId,
                UnitId = armyEntity.UnitId,
                Size = armyEntity.Size
            };

            return army;
        }

        internal static ArmyEntity ToEntity(this Army army)
        {
            ArmyEntity armyEntity = new ArmyEntity
            {
                FactionId = army.FactionId,
                UnitId = army.UnitId,
                Size = army.Size
            };

            return armyEntity;
        }

        internal static IEnumerable<Army> ToDomainModels(this IEnumerable<ArmyEntity> armyEntities)
        {
            IEnumerable<Army> armies = armyEntities.Select(armyEntity => armyEntity.ToDomainModel());

            return armies;
        }

        internal static IEnumerable<ArmyEntity> ToEntities(this IEnumerable<Army> armies)
        {
            IEnumerable<ArmyEntity> armyEntities = armies.Select(army => army.ToEntity());

            return armyEntities;
        }
    }
}
