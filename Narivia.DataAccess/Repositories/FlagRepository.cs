using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Infrastructure.Exceptions;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Flag repository implementation.
    /// </summary>
    public class FlagRepository : IFlagRepository
    {
        readonly XmlDatabase<FlagEntity> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlagRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public FlagRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<FlagEntity>(fileName);
        }

        /// <summary>
        /// Adds the specified flag.
        /// </summary>
        /// <param name="flagEntity">Flag.</param>
        public void Add(FlagEntity flagEntity)
        {
            List<FlagEntity> flagEntities = xmlDatabase.LoadEntities().ToList();
            flagEntities.Add(flagEntity);

            try
            {
                xmlDatabase.SaveEntities(flagEntities);
            }
            catch
            {
                throw new DuplicateEntityException(flagEntity.Id, nameof(FlagEntity).Replace("Entity", ""));
            }
        }

        /// <summary>
        /// Get the flag with the specified identifier.
        /// </summary>
        /// <returns>The flag.</returns>
        /// <param name="id">Identifier.</param>
        public FlagEntity Get(string id)
        {
            List<FlagEntity> flagEntities = xmlDatabase.LoadEntities().ToList();
            FlagEntity flagEntity = flagEntities.FirstOrDefault(x => x.Id == id);

            if (flagEntity == null)
            {
                throw new EntityNotFoundException(id, nameof(BorderEntity).Replace("Entity", ""));
            }

            return flagEntity;
        }

        /// <summary>
        /// Gets all the flags.
        /// </summary>
        /// <returns>The flags</returns>
        public IEnumerable<FlagEntity> GetAll()
        {
            List<FlagEntity> flagEntities = xmlDatabase.LoadEntities().ToList();

            return flagEntities;
        }

        /// <summary>
        /// Updates the specified flag.
        /// </summary>
        /// <param name="flagEntity">Flag.</param>
        public void Update(FlagEntity flagEntity)
        {
            List<FlagEntity> flagEntities = xmlDatabase.LoadEntities().ToList();
            FlagEntity flagEntityToUpdate = flagEntities.FirstOrDefault(x => x.Id == flagEntity.Id);

            if (flagEntityToUpdate == null)
            {
                throw new EntityNotFoundException(flagEntity.Id, nameof(BorderEntity).Replace("Entity", ""));
            }

            flagEntityToUpdate.Background = flagEntity.Background;
            flagEntityToUpdate.Emblem = flagEntity.Emblem;
            flagEntityToUpdate.Skin = flagEntity.Skin;
            flagEntityToUpdate.PrimaryColourHexadecimal = flagEntity.PrimaryColourHexadecimal;
            flagEntityToUpdate.SecondaryColourHexadecimal = flagEntity.SecondaryColourHexadecimal;

            xmlDatabase.SaveEntities(flagEntities);
        }

        /// <summary>
        /// Removes the flag with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<FlagEntity> flagEntities = xmlDatabase.LoadEntities().ToList();
            flagEntities.RemoveAll(x => x.Id == id);

            try
            {
                xmlDatabase.SaveEntities(flagEntities);
            }
            catch
            {
                throw new DuplicateEntityException(id, nameof(FlagEntity).Replace("Entity", ""));
            }
        }
    }
}
