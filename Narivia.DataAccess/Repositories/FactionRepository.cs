using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Faction repository implementation.
    /// </summary>
    public class FactionRepository : IFactionRepository
    {
        readonly XmlDatabase<Faction> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.FactionRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public FactionRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<Faction>(fileName);
        }

        /// <summary>
        /// Adds the specified faction.
        /// </summary>
        /// <param name="faction">Faction.</param>
        public void Add(Faction faction)
        {
            List<Faction> factions = xmlDatabase.LoadEntities().ToList();
            factions.Add(faction);

            xmlDatabase.SaveEntities(factions);
        }

        /// <summary>
        /// Get the faction with the specified identifier.
        /// </summary>
        /// <returns>The faction.</returns>
        /// <param name="id">Identifier.</param>
        public Faction Get(string id)
        {
            List<Faction> factions = xmlDatabase.LoadEntities().ToList();
            Faction faction = factions.FirstOrDefault(x => x.Id == id);

            return faction;
        }

        /// <summary>
        /// Gets all the factions.
        /// </summary>
        /// <returns>The factions</returns>
        public IEnumerable<Faction> GetAll()
        {
            List<Faction> factions = xmlDatabase.LoadEntities().ToList();

            return factions;
        }

        /// <summary>
        /// Updates the specified faction.
        /// </summary>
        /// <param name="faction">Faction.</param>
        public void Update(Faction faction)
        {
            List<Faction> factions = xmlDatabase.LoadEntities().ToList();
            Faction factionToUpdate = factions.FirstOrDefault(x => x.Id == faction.Id);

            factionToUpdate.Name = faction.Name;
            factionToUpdate.Description = faction.Description;
            factionToUpdate.Colour = faction.Colour;
            factionToUpdate.Wealth = faction.Wealth;

            xmlDatabase.SaveEntities(factions);
        }

        /// <summary>
        /// Removes the faction with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<Faction> factions = xmlDatabase.LoadEntities().ToList();
            factions.RemoveAll(x => x.Id == id);

            xmlDatabase.SaveEntities(factions);
        }
    }
}
