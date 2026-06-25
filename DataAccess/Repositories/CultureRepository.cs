using NuciDAL.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Culture repository implementation.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CultureRepository"/> class.
    /// </remarks>
    /// <param name="fileName">File name.</param>
    public class CultureRepository(string fileName) : XmlRepository<CultureEntity>(fileName)
    {

        /// <summary>
        /// Updates the specified culture.
        /// </summary>
        /// <param name="entity">Culture.</param>
        public override void Update(CultureEntity entity)
        {
            LoadEntities();

            CultureEntity cultureEntityToUpdate = Get(entity.Id) ?? throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            cultureEntityToUpdate.Name = entity.Name;
            cultureEntityToUpdate.Description = entity.Description;
            cultureEntityToUpdate.TextureSet = entity.TextureSet;

            SaveChanges();
        }
    }
}
