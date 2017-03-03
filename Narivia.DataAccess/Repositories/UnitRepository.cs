using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Repositories.Interfaces;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Unit repository implementation.
    /// </summary>
    public class UnitRepository : IUnitRepository
    {
        readonly XmlDatabase<UnitEntity> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.UnitRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public UnitRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<UnitEntity>(fileName);
        }

        /// <summary>
        /// Adds the specified unit.
        /// </summary>
        /// <param name="unitEntity">Unit.</param>
        public void Add(UnitEntity unitEntity)
        {
            List<UnitEntity> unitEntities = xmlDatabase.LoadEntities().ToList();
            unitEntities.Add(unitEntity);

            xmlDatabase.SaveEntities(unitEntities);
        }

        /// <summary>
        /// Get the unit with the specified identifier.
        /// </summary>
        /// <returns>The unit.</returns>
        /// <param name="id">Identifier.</param>
        public UnitEntity Get(string id)
        {
            List<UnitEntity> unitEntities = xmlDatabase.LoadEntities().ToList();
            UnitEntity unitEntity = unitEntities.FirstOrDefault(x => x.Id == id);

            return unitEntity;
        }

        /// <summary>
        /// Gets all the units.
        /// </summary>
        /// <returns>The units</returns>
        public IEnumerable<UnitEntity> GetAll()
        {
            List<UnitEntity> unitEntities = xmlDatabase.LoadEntities().ToList();

            return unitEntities;
        }

        /// <summary>
        /// Updates the specified unit.
        /// </summary>
        /// <param name="unitEntity">Unit.</param>
        public void Update(UnitEntity unitEntity)
        {
            List<UnitEntity> unitEntities = xmlDatabase.LoadEntities().ToList();
            UnitEntity unitEntityToUpdate = unitEntities.FirstOrDefault(x => x.Id == unitEntity.Id);

            unitEntityToUpdate.Name = unitEntity.Name;
            unitEntityToUpdate.Description = unitEntity.Description;
            unitEntityToUpdate.Type = unitEntity.Type;
            unitEntityToUpdate.Power = unitEntity.Power;
            unitEntityToUpdate.Health = unitEntity.Health;
            unitEntityToUpdate.Price = unitEntity.Price;
            unitEntityToUpdate.Maintenance = unitEntity.Maintenance;

            xmlDatabase.SaveEntities(unitEntities);
        }

        /// <summary>
        /// Removes the unit with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<UnitEntity> unitEntities = xmlDatabase.LoadEntities().ToList();
            unitEntities.RemoveAll(x => x.Id == id);

            xmlDatabase.SaveEntities(unitEntities);
        }
    }
}
