using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;

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
        /// <param name="holdingEntity">Holding.</param>
        void Add(HoldingEntity holdingEntity);

        /// <summary>
        /// Gets the holding with the specified identifier.
        /// </summary>
        /// <returns>The holding.</returns>
        /// <param name="id">Identifier.</param>
        HoldingEntity Get(string id);

        /// <summary>
        /// Gets all the holdings.
        /// </summary>
        /// <returns>The holdings.</returns>
        IEnumerable<HoldingEntity> GetAll();

        /// <summary>
        /// Updates the specified holding.
        /// </summary>
        /// <param name="holdingEntity">Holding.</param>
        void Update(HoldingEntity holdingEntity);

        /// <summary>
        /// Remove the holding with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
