using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// Army mapping extensions for converting between entities and domain models.
    /// </summary>
    static class ArmyMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="armyEntity">Army entity.</param>
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

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="army">Army.</param>
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

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="armyEntities">Army entities.</param>
        internal static IEnumerable<Army> ToDomainModels(this IEnumerable<ArmyEntity> armyEntities)
        {
            IEnumerable<Army> armies = armyEntities.Select(armyEntity => armyEntity.ToDomainModel());

            return armies;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="armies">Armies.</param>
        internal static IEnumerable<ArmyEntity> ToEntities(this IEnumerable<Army> armies)
        {
            IEnumerable<ArmyEntity> armyEntities = armies.Select(army => army.ToEntity());

            return armyEntities;
        }
    }
}
