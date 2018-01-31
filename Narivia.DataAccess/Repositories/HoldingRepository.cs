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
        /// <param name="holdingEntity">Holding.</param>
        public override void Update(HoldingEntity holdingEntity)
        {
            LoadEntitiesIfNeeded();

            HoldingEntity holdingEntityToUpdate = Get(holdingEntity.Id);

            if (holdingEntityToUpdate == null)
            {
                throw new EntityNotFoundException(holdingEntity.Id, nameof(BorderEntity));
            }

            holdingEntityToUpdate.Name = holdingEntity.Name;
            holdingEntityToUpdate.Description = holdingEntity.Description;
            holdingEntityToUpdate.Type = holdingEntity.Type;

            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
