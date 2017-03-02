using System.Collections.Generic;

using Narivia.Models;

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
        /// <param name="faction">Faction.</param>
        void Add(Faction faction);

        /// <summary>
        /// Gets the faction with the specified identifier.
        /// </summary>
        /// <returns>The faction.</returns>
        /// <param name="id">Identifier.</param>
        Faction Get(string id);

        /// <summary>
        /// Gets all the factions.
        /// </summary>
        /// <returns>The factions.</returns>
        IEnumerable<Faction> GetAll();

        /// <summary>
        /// Updates the specified faction.
        /// </summary>
        /// <param name="faction">Faction.</param>
        void Update(Faction faction);

        /// <summary>
        /// Remove the faction with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
