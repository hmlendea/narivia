using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Repositories.Interfaces;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Faction repository implementation.
    /// </summary>
    public class FactionRepository : IFactionRepository
    {
        readonly XmlDatabase<FactionEntity> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.FactionRepository"/> class.
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

            xmlDatabase.SaveEntities(factionEntities);
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

            factionEntityToUpdate.Name = factionEntity.Name;
            factionEntityToUpdate.Description = factionEntity.Description;
            factionEntityToUpdate.Colour = factionEntity.Colour;

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

            xmlDatabase.SaveEntities(factionEntities);
        }
    }
}
