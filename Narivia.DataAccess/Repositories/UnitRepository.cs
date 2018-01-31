using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Unit repository implementation.
    /// </summary>
    public class UnitRepository : XmlRepository<UnitEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public UnitRepository(string fileName) : base(fileName)
        {

        }
        
        /// <summary>
        /// Updates the specified unit.
        /// </summary>
        /// <param name="unitEntity">Unit.</param>
        public override void Update(UnitEntity unitEntity)
        {
            LoadEntitiesIfNeeded();

            UnitEntity unitEntityToUpdate = Get(unitEntity.Id);

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

            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
