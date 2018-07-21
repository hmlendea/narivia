using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Holding repository implementation.
    /// </summary>
    public class HoldingRepository : XmlRepository<HoldingEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HoldingRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public HoldingRepository(string fileName) : base(fileName)
        {

        }

        /// <summary>
        /// Updates the specified holding.
        /// </summary>
        /// <param name="entity">Holding.</param>
        public override void Update(HoldingEntity entity)
        {
            LoadEntitiesIfNeeded();

            HoldingEntity holdingEntityToUpdate = Get(entity.Id);

            if (holdingEntityToUpdate == null)
            {
                throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            }

            holdingEntityToUpdate.Name = entity.Name;
            holdingEntityToUpdate.Description = entity.Description;
            holdingEntityToUpdate.Type = entity.Type;

            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
