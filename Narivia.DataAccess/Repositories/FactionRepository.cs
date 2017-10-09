using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Exceptions;
using Narivia.DataAccess.Repositories.Interfaces;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Faction repository implementation.
    /// </summary>
    public class FactionRepository : IRepository<string, FactionEntity>
    {
        readonly XmlDatabase<FactionEntity> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="FactionRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public FactionRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<FactionEntity>(fileName);
        }

        /// <summary>
        /// Adds the specified faction.
        /// </summary>
        /// <param name="factionEntity">Faction.</param>
        public void Add(FactionEntity factionEntity)
        {
            List<FactionEntity> factionEntities = xmlDatabase.LoadEntities().ToList();
            factionEntities.Add(factionEntity);

            try
            {
                xmlDatabase.SaveEntities(factionEntities);
            }
            catch
            {
                throw new DuplicateEntityException(factionEntity.Id, nameof(FactionEntity));
            }
        }

        /// <summary>
        /// Get the faction with the specified identifier.
        /// </summary>
        /// <returns>The faction.</returns>
        /// <param name="id">Identifier.</param>
        public FactionEntity Get(string id)
        {
            List<FactionEntity> factionEntities = xmlDatabase.LoadEntities().ToList();
            FactionEntity factionEntity = factionEntities.FirstOrDefault(x => x.Id == id);

            if (factionEntity == null)
            {
                throw new EntityNotFoundException(id, nameof(BorderEntity));
            }

            return factionEntity;
        }

        /// <summary>
        /// Gets all the factions.
        /// </summary>
        /// <returns>The factions</returns>
        public IEnumerable<FactionEntity> GetAll()
        {
            List<FactionEntity> factionEntities = xmlDatabase.LoadEntities().ToList();

            return factionEntities;
        }

        /// <summary>
        /// Updates the specified faction.
        /// </summary>
        /// <param name="factionEntity">Faction.</param>
        public void Update(FactionEntity factionEntity)
        {
            List<FactionEntity> factionEntities = xmlDatabase.LoadEntities().ToList();
            FactionEntity factionEntityToUpdate = factionEntities.FirstOrDefault(x => x.Id == factionEntity.Id);

            if (factionEntityToUpdate == null)
            {
                throw new EntityNotFoundException(factionEntity.Id, nameof(BorderEntity));
            }

            factionEntityToUpdate.Name = factionEntity.Name;
            factionEntityToUpdate.Description = factionEntity.Description;
            factionEntityToUpdate.ColourHexadecimal = factionEntity.ColourHexadecimal;
            factionEntityToUpdate.FlagId = factionEntity.FlagId;
            factionEntityToUpdate.CultureId = factionEntity.CultureId;

            xmlDatabase.SaveEntities(factionEntities);
        }

        /// <summary>
        /// Removes the faction with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<FactionEntity> factionEntities = xmlDatabase.LoadEntities().ToList();
            factionEntities.RemoveAll(x => x.Id == id);

            try
            {
                xmlDatabase.SaveEntities(factionEntities);
            }
            catch
            {
                throw new DuplicateEntityException(id, nameof(FactionEntity));
            }
        }
    }
}
