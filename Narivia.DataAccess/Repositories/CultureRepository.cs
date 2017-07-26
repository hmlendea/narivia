using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Exceptions;
using Narivia.DataAccess.Repositories.Interfaces;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Culture repository implementation.
    /// </summary>
    public class CultureRepository : ICultureRepository
    {
        readonly XmlDatabase<CultureEntity> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.CultureRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public CultureRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<CultureEntity>(fileName);
        }

        /// <summary>
        /// Adds the specified culture.
        /// </summary>
        /// <param name="cultureEntity">Culture.</param>
        public void Add(CultureEntity cultureEntity)
        {
            List<CultureEntity> cultureEntities = xmlDatabase.LoadEntities().ToList();
            cultureEntities.Add(cultureEntity);

            try
            {
                xmlDatabase.SaveEntities(cultureEntities);
            }
            catch
            {
                throw new DuplicateEntityException(cultureEntity.Id, nameof(CultureEntity).Replace("Entity", ""));
            }
        }

        /// <summary>
        /// Get the culture with the specified identifier.
        /// </summary>
        /// <returns>The culture.</returns>
        /// <param name="id">Identifier.</param>
        public CultureEntity Get(string id)
        {
            List<CultureEntity> cultureEntities = xmlDatabase.LoadEntities().ToList();
            CultureEntity cultureEntity = cultureEntities.FirstOrDefault(x => x.Id == id);

            if (cultureEntity == null)
            {
                throw new EntityNotFoundException(id, nameof(BorderEntity).Replace("Entity", ""));
            }

            return cultureEntity;
        }

        /// <summary>
        /// Gets all the cultures.
        /// </summary>
        /// <returns>The cultures</returns>
        public IEnumerable<CultureEntity> GetAll()
        {
            List<CultureEntity> cultureEntities = xmlDatabase.LoadEntities().ToList();

            return cultureEntities;
        }

        /// <summary>
        /// Updates the specified culture.
        /// </summary>
        /// <param name="cultureEntity">Culture.</param>
        public void Update(CultureEntity cultureEntity)
        {
            List<CultureEntity> cultureEntities = xmlDatabase.LoadEntities().ToList();
            CultureEntity cultureEntityToUpdate = cultureEntities.FirstOrDefault(x => x.Id == cultureEntity.Id);

            if (cultureEntityToUpdate == null)
            {
                throw new EntityNotFoundException(cultureEntity.Id, nameof(BorderEntity).Replace("Entity", ""));
            }

            cultureEntityToUpdate.Name = cultureEntity.Name;
            cultureEntityToUpdate.Description = cultureEntity.Description;
            cultureEntityToUpdate.TextureSet = cultureEntity.TextureSet;

            xmlDatabase.SaveEntities(cultureEntities);
        }

        /// <summary>
        /// Removes the culture with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<CultureEntity> cultureEntities = xmlDatabase.LoadEntities().ToList();
            cultureEntities.RemoveAll(x => x.Id == id);

            try
            {
                xmlDatabase.SaveEntities(cultureEntities);
            }
            catch
            {
                throw new DuplicateEntityException(id, nameof(CultureEntity).Replace("Entity", ""));
            }
        }
    }
}
