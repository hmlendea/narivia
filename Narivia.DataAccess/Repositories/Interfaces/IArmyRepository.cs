using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Army repository interface.
    /// </summary>
    public interface IArmyRepository
    {
        /// <summary>
        /// Adds the specified army.
        /// </summary>
        /// <param name="army">Army.</param>
        void Add(ArmyEntity army);

        /// <summary>
        /// Gets the army with the specified faction and unit identifiers.
        /// </summary>
        /// <returns>The army.</returns>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        ArmyEntity Get(string factionId, string unitId);

        /// <summary>
        /// Gets all the armies.
        /// </summary>
        /// <returns>The armies.</returns>
        IEnumerable<ArmyEntity> GetAll();

        /// <summary>
        /// Removes the army with the specified faction and unit identifiers.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        void Remove(string factionId, string unitId);
    }
}
