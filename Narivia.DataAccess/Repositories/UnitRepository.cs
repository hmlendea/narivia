using System.Collections.Generic;
using System.Linq;

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
            List<Unit> units = xmlDatabase.LoadEntities().ToList();
            units.Add(unit);

            xmlDatabase.SaveEntities(units);
        }

        /// <summary>
        /// Get the unit with the specified identifier.
        /// </summary>
        /// <returns>The unit.</returns>
        /// <param name="id">Identifier.</param>
        public Unit Get(string id)
        {
            List<Unit> units = xmlDatabase.LoadEntities().ToList();
            Unit unit = units.FirstOrDefault(x => x.Id == id);

            return unit;
        }

        /// <summary>
        /// Gets all the units.
        /// </summary>
        /// <returns>The units</returns>
        public IEnumerable<Unit> GetAll()
        {
            List<Unit> units = xmlDatabase.LoadEntities().ToList();

            return units;
        }

        /// <summary>
        /// Updates the specified unit.
        /// </summary>
        /// <param name="unit">Unit.</param>
        public void Update(Unit unit)
        {
            List<Unit> units = xmlDatabase.LoadEntities().ToList();
            Unit unitToUpdate = units.FirstOrDefault(x => x.Id == unit.Id);

            unitToUpdate.Name = unit.Name;
            unitToUpdate.Description = unit.Description;
            unitToUpdate.Type = unit.Type;
            unitToUpdate.Power = unit.Power;
            unitToUpdate.Health = unit.Health;
            unitToUpdate.Price = unit.Price;
            unitToUpdate.Maintenance = unit.Maintenance;

            xmlDatabase.SaveEntities(units);
        }

        /// <summary>
        /// Removes the unit with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<Unit> units = xmlDatabase.LoadEntities().ToList();
            units.RemoveAll(x => x.Id == id);

            xmlDatabase.SaveEntities(units);
        }
    }
}
