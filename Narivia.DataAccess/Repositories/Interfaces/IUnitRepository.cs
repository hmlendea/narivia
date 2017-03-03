using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;

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
        /// <param name="unitEntity">Unit.</param>
        void Add(UnitEntity unitEntity);

        /// <summary>
        /// Gets the unit with the specified identifier.
        /// </summary>
        /// <returns>The unit.</returns>
        /// <param name="id">Identifier.</param>
        UnitEntity Get(string id);

        /// <summary>
        /// Gets all the units.
        /// </summary>
        /// <returns>The units.</returns>
        IEnumerable<UnitEntity> GetAll();

        /// <summary>
        /// Updates the specified unit.
        /// </summary>
        /// <param name="unitEntity">Unit.</param>
        void Update(UnitEntity unitEntity);

        /// <summary>
        /// Remove the unit with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
