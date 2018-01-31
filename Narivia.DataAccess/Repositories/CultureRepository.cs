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
        /// <param name="cultureEntity">Culture.</param>
        public override void Update(CultureEntity cultureEntity)
        {
            LoadEntitiesIfNeeded();

            CultureEntity cultureEntityToUpdate = Get(cultureEntity.Id);

            if (cultureEntityToUpdate == null)
            {
                throw new EntityNotFoundException(cultureEntity.Id, nameof(BorderEntity));
            }

            cultureEntityToUpdate.Name = cultureEntity.Name;
            cultureEntityToUpdate.Description = cultureEntity.Description;
            cultureEntityToUpdate.TextureSet = cultureEntity.TextureSet;

            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
