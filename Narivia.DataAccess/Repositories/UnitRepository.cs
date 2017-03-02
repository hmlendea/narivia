using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Unit repository implementation.
    /// </summary>
    public class UnitRepository : IUnitRepository
    {
        readonly XmlDatabase<Unit> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.UnitRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public UnitRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<Unit>(fileName);
        }

        /// <summary>
        /// Adds the specified unit.
        /// </summary>
        /// <param name="unit">Unit.</param>
        public void Add(Unit unit)
        {
            xmlDatabase.Add(unit);
        }

        /// <summary>
        /// Gets the unit with the specified identifier.
        /// </summary>
        /// <returns>The unit.</returns>
        /// <param name="id">Identifier.</param>
        public Unit Get(string id)
        {
            return xmlDatabase.Get(id);
        }

        /// <summary>
        /// Gets all the units.
        /// </summary>
        /// <returns>The units.</returns>
        public IEnumerable<Unit> GetAll()
        {
            return xmlDatabase.GetAll();
        }

        /// <summary>
        /// Updates the specified unit.
        /// </summary>
        /// <param name="unit">Unit.</param>
        public void Update(Unit unit)
        {
            xmlDatabase.Update(unit);
        }

        /// <summary>
        /// Remove the unit with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            xmlDatabase.Remove(id);
        }
    }
}
