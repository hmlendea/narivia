using System.Collections.Generic;
using System.Linq;

using NuciXNA.DataAccess.Exceptions;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Repositories.Interfaces;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Unit repository implementation.
    /// </summary>
    public class UnitRepository : IRepository<string, UnitEntity>
    {
        readonly XmlDatabase<UnitEntity> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitRepository"/> class.
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

            try
            {
                xmlDatabase.SaveEntities(unitEntities);
            }
            catch
            {
                throw new DuplicateEntityException(unitEntity.Id, nameof(UnitEntity));
            }
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

            if (unitEntity == null)
            {
                throw new EntityNotFoundException(id, nameof(BorderEntity));
            }

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

            if (unitEntityToUpdate == null)
            {
                throw new EntityNotFoundException(unitEntity.Id, nameof(BorderEntity));
            }

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

            try
            {
                xmlDatabase.SaveEntities(unitEntities);
            }
            catch
            {
                throw new DuplicateEntityException(id, nameof(UnitEntity));
            }
        }
    }
}
