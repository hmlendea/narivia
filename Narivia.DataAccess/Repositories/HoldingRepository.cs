using System.Collections.Generic;
using System.Linq;

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
            List<Holding> holdings = xmlDatabase.LoadEntities().ToList();
            holdings.Add(holding);

            xmlDatabase.SaveEntities(holdings);
        }

        /// <summary>
        /// Get the holding with the specified identifier.
        /// </summary>
        /// <returns>The holding.</returns>
        /// <param name="id">Identifier.</param>
        public Holding Get(string id)
        {
            List<Holding> holdings = xmlDatabase.LoadEntities().ToList();
            Holding holding = holdings.FirstOrDefault(x => x.Id == id);

            return holding;
        }

        /// <summary>
        /// Gets all the holdings.
        /// </summary>
        /// <returns>The holdings</returns>
        public IEnumerable<Holding> GetAll()
        {
            List<Holding> holdings = xmlDatabase.LoadEntities().ToList();

            return holdings;
        }

        /// <summary>
        /// Updates the specified holding.
        /// </summary>
        /// <param name="holding">Holding.</param>
        public void Update(Holding holding)
        {
            List<Holding> holdings = xmlDatabase.LoadEntities().ToList();
            Holding holdingToUpdate = holdings.FirstOrDefault(x => x.Id == holding.Id);

            holdingToUpdate.Name = holding.Name;
            holdingToUpdate.Description = holding.Description;
            holdingToUpdate.Type = holding.Type;

            xmlDatabase.SaveEntities(holdings);
        }

        /// <summary>
        /// Removes the holding with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<Holding> holdings = xmlDatabase.LoadEntities().ToList();
            holdings.RemoveAll(x => x.Id == id);

            xmlDatabase.SaveEntities(holdings);
        }
    }
}
