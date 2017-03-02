using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Holding repository implementation.
    /// </summary>
    public class HoldingRepository : IHoldingRepository
    {
        readonly XmlDatabase<Holding> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.HoldingRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public HoldingRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<Holding>(fileName);
        }

        /// <summary>
        /// Adds the specified holding.
        /// </summary>
        /// <param name="holding">Holding.</param>
        public void Add(Holding holding)
        {
            xmlDatabase.Add(holding);
        }

        /// <summary>
        /// Gets the holding with the specified id.
        /// </summary>
        /// <returns>The holding.</returns>
        /// <param name="id">Identifier.</param>
        public Holding Get(string id)
        {
            return xmlDatabase.Get(id);
        }

        /// <summary>
        /// Gets all the holdings.
        /// </summary>
        /// <returns>The holdings.</returns>
        public IEnumerable<Holding> GetAll()
        {
            return xmlDatabase.GetAll();
        }

        /// <summary>
        /// Updates the specified holding.
        /// </summary>
        /// <param name="holding">Holding.</param>
        public void Update(Holding holding)
        {
            xmlDatabase.Update(holding);
        }

        /// <summary>
        /// Remove the holding with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            xmlDatabase.Remove(id);
        }
    }
}
