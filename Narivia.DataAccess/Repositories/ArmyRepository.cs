using System;
using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Infrastructure.Exceptions;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Army repository implementation.
    /// </summary>
    public class ArmyRepository : IArmyRepository
    {
        /// <summary>
        /// Gets or sets the armies.
        /// </summary>
        /// <value>The armies.</value>
        readonly Dictionary<Tuple<string, string>, ArmyEntity> armyEntitiesStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.ArmyRepository"/> class.
        /// </summary>
        public ArmyRepository()
        {
            armyEntitiesStore = new Dictionary<Tuple<string, string>, ArmyEntity>();
        }

        /// <summary>
        /// Adds the specified army.
        /// </summary>
        /// <param name="army">Army.</param>
        public void Add(ArmyEntity army)
        {
            Tuple<string, string> key = new Tuple<string, string>(army.FactionId, army.UnitId);

            try
            {
                armyEntitiesStore.Add(key, army);
            }
            catch
            {
                throw new DuplicateEntityException(
                    $"{army.FactionId}-{army.UnitId}",
                    nameof(ArmyEntity).Replace("Entity", ""));
            }
        }

        /// <summary>
        /// Gets the army with the specified faction and unit identifiers.
        /// </summary>
        /// <returns>The army.</returns>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        public ArmyEntity Get(string factionId, string unitId)
        {
            Tuple<string, string> key = new Tuple<string, string>(factionId, unitId);

            if (!armyEntitiesStore.ContainsKey(key))
            {
                return null;
            }

            ArmyEntity armyEntity = armyEntitiesStore[key];

            if (armyEntity == null)
            {
                throw new EntityNotFoundException(
                    $"{factionId}-{unitId}",
                    nameof(ArmyEntity).Replace("Entity", ""));
            }

            return armyEntity;
        }

        /// <summary>
        /// Gets all the armies.
        /// </summary>
        /// <returns>The armies.</returns>
        public IEnumerable<ArmyEntity> GetAll()
        {
            return armyEntitiesStore.Values;
        }

        /// <summary>
        /// Removes the army with the specified faction and unit identifiers.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        public void Remove(string factionId, string unitId)
        {
            Tuple<string, string> key = new Tuple<string, string>(factionId, unitId);

            try
            {
                armyEntitiesStore.Remove(key);
            }
            catch
            {
                throw new DuplicateEntityException(
                    $"{factionId}-{unitId}",
                    nameof(ArmyEntity).Replace("Entity", ""));
            }
        }
    }
}
