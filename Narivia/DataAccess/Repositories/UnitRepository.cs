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
        /// <param name="entity">Unit.</param>
        public override void Update(UnitEntity entity)
        {
            LoadEntitiesIfNeeded();

            UnitEntity unitEntityToUpdate = Get(entity.Id);

            if (unitEntityToUpdate == null)
            {
                throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            }

            unitEntityToUpdate.Name = entity.Name;
            unitEntityToUpdate.Description = entity.Description;
            unitEntityToUpdate.Type = entity.Type;
            unitEntityToUpdate.Power = entity.Power;
            unitEntityToUpdate.Health = entity.Health;
            unitEntityToUpdate.Price = entity.Price;
            unitEntityToUpdate.Maintenance = entity.Maintenance;

            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
