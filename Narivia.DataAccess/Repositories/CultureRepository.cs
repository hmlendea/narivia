using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Culture repository implementation.
    /// </summary>
    public class CultureRepository : XmlRepository<CultureEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CultureRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public CultureRepository(string fileName) : base(fileName)
        {

        }

        /// <summary>
        /// Updates the specified culture.
        /// </summary>
        /// <param name="entity">Culture.</param>
        public override void Update(CultureEntity entity)
        {
            LoadEntitiesIfNeeded();

            CultureEntity cultureEntityToUpdate = Get(entity.Id);

            if (cultureEntityToUpdate == null)
            {
                throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            }

            cultureEntityToUpdate.Name = entity.Name;
            cultureEntityToUpdate.Description = entity.Description;
            cultureEntityToUpdate.TextureSet = entity.TextureSet;

            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
