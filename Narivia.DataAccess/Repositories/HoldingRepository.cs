using System.Collections.Generic;
using System.Linq;

using NuciXNA.DataAccess.Exceptions;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Repositories.Interfaces;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Holding repository implementation.
    /// </summary>
    public class HoldingRepository : IRepository<string, HoldingEntity>
    {
        readonly XmlDatabase<HoldingEntity> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="HoldingRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public HoldingRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<HoldingEntity>(fileName);
        }

        /// <summary>
        /// Adds the specified holding.
        /// </summary>
        /// <param name="holdingEntity">Holding.</param>
        public void Add(HoldingEntity holdingEntity)
        {
            List<HoldingEntity> holdingEntities = xmlDatabase.LoadEntities().ToList();
            holdingEntities.Add(holdingEntity);

            try
            {
                xmlDatabase.SaveEntities(holdingEntities);
            }
            catch
            {
                throw new DuplicateEntityException(holdingEntity.Id, nameof(HoldingEntity));
            }
        }

        /// <summary>
        /// Get the holding with the specified identifier.
        /// </summary>
        /// <returns>The holding.</returns>
        /// <param name="id">Identifier.</param>
        public HoldingEntity Get(string id)
        {
            List<HoldingEntity> holdingEntities = xmlDatabase.LoadEntities().ToList();
            HoldingEntity holdingEntity = holdingEntities.FirstOrDefault(x => x.Id == id);

            if (holdingEntity == null)
            {
                throw new EntityNotFoundException(id, nameof(BorderEntity));
            }

            return holdingEntity;
        }

        /// <summary>
        /// Gets all the holdings.
        /// </summary>
        /// <returns>The holdings</returns>
        public IEnumerable<HoldingEntity> GetAll()
        {
            List<HoldingEntity> holdingEntities = xmlDatabase.LoadEntities().ToList();

            return holdingEntities;
        }

        /// <summary>
        /// Updates the specified holding.
        /// </summary>
        /// <param name="holdingEntity">Holding.</param>
        public void Update(HoldingEntity holdingEntity)
        {
            List<HoldingEntity> holdingEntities = xmlDatabase.LoadEntities().ToList();
            HoldingEntity holdingEntityToUpdate = holdingEntities.FirstOrDefault(x => x.Id == holdingEntity.Id);

            if (holdingEntityToUpdate == null)
            {
                throw new EntityNotFoundException(holdingEntity.Id, nameof(BorderEntity));
            }

            holdingEntityToUpdate.Name = holdingEntity.Name;
            holdingEntityToUpdate.Description = holdingEntity.Description;
            holdingEntityToUpdate.Type = holdingEntity.Type;

            xmlDatabase.SaveEntities(holdingEntities);
        }

        /// <summary>
        /// Removes the holding with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<HoldingEntity> holdingEntities = xmlDatabase.LoadEntities().ToList();
            holdingEntities.RemoveAll(x => x.Id == id);

            try
            {
                xmlDatabase.SaveEntities(holdingEntities);
            }
            catch
            {
                throw new DuplicateEntityException(id, nameof(HoldingEntity));
            }
        }
    }
}
