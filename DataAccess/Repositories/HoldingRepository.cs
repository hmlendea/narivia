using NuciDAL.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Holding repository implementation.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="HoldingRepository"/> class.
    /// </remarks>
    /// <param name="fileName">File name.</param>
    public class HoldingRepository(string fileName) : XmlRepository<HoldingEntity>(fileName)
    {

        /// <summary>
        /// Updates the specified holding.
        /// </summary>
        /// <param name="entity">Holding.</param>
        public override void Update(HoldingEntity entity)
        {
            LoadEntities();

            HoldingEntity holdingEntityToUpdate = Get(entity.Id) ?? throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            holdingEntityToUpdate.Name = entity.Name;
            holdingEntityToUpdate.Description = entity.Description;
            holdingEntityToUpdate.Type = entity.Type;

            SaveChanges();
        }
    }
}
