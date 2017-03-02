using System.Collections.Generic;

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
            xmlDatabase.Add(faction);
        }

        /// <summary>
        /// Gets the faction with the specified identifier.
        /// </summary>
        /// <returns>The faction.</returns>
        /// <param name="id">Identifier.</param>
        public Faction Get(string id)
        {
            return xmlDatabase.Get(id);
        }

        /// <summary>
        /// Gets all the factions.
        /// </summary>
        /// <returns>The factions.</returns>
        public IEnumerable<Faction> GetAll()
        {
            return xmlDatabase.GetAll();
        }

        /// <summary>
        /// Updates the specified faction.
        /// </summary>
        /// <param name="faction">Faction.</param>
        public void Update(Faction faction)
        {
            xmlDatabase.Update(faction);
        }

        /// <summary>
        /// Remove the faction with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            xmlDatabase.Remove(id);
        }
    }
}
