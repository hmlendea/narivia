using NuciDAL.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Unit repository implementation.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UnitRepository"/> class.
    /// </remarks>
    /// <param name="fileName">File name.</param>
    public class UnitRepository(string fileName) : XmlRepository<UnitEntity>(fileName)
    {
        /// <summary>
        /// Updates the specified unit.
        /// </summary>
        /// <param name="entity">Unit.</param>
        public override void Update(UnitEntity entity)
        {
            LoadEntities();

            UnitEntity unitEntityToUpdate = Get(entity.Id) ?? throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            unitEntityToUpdate.Name = entity.Name;
            unitEntityToUpdate.Description = entity.Description;
            unitEntityToUpdate.Type = entity.Type;
            unitEntityToUpdate.Power = entity.Power;
            unitEntityToUpdate.Health = entity.Health;
            unitEntityToUpdate.Price = entity.Price;
            unitEntityToUpdate.Maintenance = entity.Maintenance;

            SaveChanges();
        }
    }
}
