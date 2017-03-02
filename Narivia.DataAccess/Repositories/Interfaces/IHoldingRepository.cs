using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Holding repository interface.
    /// </summary>
    public interface IHoldingRepository
    {
        /// <summary>
        /// Adds the specified holding.
        /// </summary>
        /// <param name="holding">Holding.</param>
        void Add(Holding holding);

        /// <summary>
        /// Gets the holding with the specified identifier.
        /// </summary>
        /// <returns>The holding.</returns>
        /// <param name="id">Identifier.</param>
        Holding Get(string id);

        /// <summary>
        /// Gets all the holdings.
        /// </summary>
        /// <returns>The holdings.</returns>
        IEnumerable<Holding> GetAll();

        /// <summary>
        /// Updates the specified holding.
        /// </summary>
        /// <param name="holding">Holding.</param>
        void Update(Holding holding);

        /// <summary>
        /// Remove the holding with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
