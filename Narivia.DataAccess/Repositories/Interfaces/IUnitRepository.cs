using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Unit repository interface.
    /// </summary>
    public interface IUnitRepository
    {
        /// <summary>
        /// Adds the specified unit.
        /// </summary>
        /// <param name="unit">Unit.</param>
        void Add(Unit unit);

        /// <summary>
        /// Gets the unit with the specified identifier.
        /// </summary>
        /// <returns>The unit.</returns>
        /// <param name="id">Identifier.</param>
        Unit Get(string id);

        /// <summary>
        /// Gets all the units.
        /// </summary>
        /// <returns>The units.</returns>
        IEnumerable<Unit> GetAll();

        /// <summary>
        /// Updates the specified unit.
        /// </summary>
        /// <param name="unit">Unit.</param>
        void Update(Unit unit);

        /// <summary>
        /// Remove the unit with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
