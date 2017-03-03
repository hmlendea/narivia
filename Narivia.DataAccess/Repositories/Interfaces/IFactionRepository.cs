using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Faction repository interface.
    /// </summary>
    public interface IFactionRepository
    {
        /// <summary>
        /// Adds the specified faction.
        /// </summary>
        /// <param name="factionEntity">Faction.</param>
        void Add(FactionEntity factionEntity);

        /// <summary>
        /// Gets the faction with the specified identifier.
        /// </summary>
        /// <returns>The faction.</returns>
        /// <param name="id">Identifier.</param>
        FactionEntity Get(string id);

        /// <summary>
        /// Gets all the factions.
        /// </summary>
        /// <returns>The factions.</returns>
        IEnumerable<FactionEntity> GetAll();

        /// <summary>
        /// Updates the specified faction.
        /// </summary>
        /// <param name="factionEntity">Faction.</param>
        void Update(FactionEntity factionEntity);

        /// <summary>
        /// Remove the faction with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
