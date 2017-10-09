using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Exceptions;
using Narivia.DataAccess.Repositories.Interfaces;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Army repository implementation.
    /// </summary>
    public class ArmyRepository : IRepository<string, ArmyEntity>
    {
        /// <summary>
        /// Gets or sets the armies.
        /// </summary>
        /// <value>The armies.</value>
        readonly Dictionary<string, ArmyEntity> armyEntitiesStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArmyRepository"/> class.
        /// </summary>
        public ArmyRepository()
        {
            armyEntitiesStore = new Dictionary<string, ArmyEntity>();
        }

        /// <summary>
        /// Adds the specified army.
        /// </summary>
        /// <param name="armyEntity">Army.</param>
        public void Add(ArmyEntity armyEntity)
        {
            try
            {
                armyEntitiesStore.Add(armyEntity.Id, armyEntity);
            }
            catch
            {
                throw new DuplicateEntityException(armyEntity.Id, nameof(ArmyEntity));
            }
        }

        /// <summary>
        /// Gets the army with the specified identifier.
        /// </summary>
        /// <returns>The army.</returns>
        /// <param name="id">Identifier.</param>
        public ArmyEntity Get(string id)
        {
            if (!armyEntitiesStore.ContainsKey(id))
            {
                return null;
            }

            ArmyEntity armyEntity = armyEntitiesStore[id];

            if (armyEntity == null)
            {
                throw new EntityNotFoundException(id, nameof(ArmyEntity));
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
        /// Removes the army with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            try
            {
                armyEntitiesStore.Remove(id);
            }
            catch
            {
                throw new DuplicateEntityException(id, nameof(ArmyEntity));
            }
        }
    }
}
