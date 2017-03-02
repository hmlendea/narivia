using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    public class HoldingRepository : IHoldingRepository
    {
        readonly XmlDatabase<Holding> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.HoldingRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public HoldingRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<Holding>(fileName);
        }

        public void Add(Holding holding)
        {
            xmlDatabase.Add(holding);
        }

        public Holding Get(string id)
        {
            return xmlDatabase.Get(id);
        }

        public IEnumerable<Holding> GetAll()
        {
            return xmlDatabase.GetAll();
        }

        public void Update(Holding holding)
        {
            xmlDatabase.Update(holding);
        }

        public void Remove(string id)
        {
            xmlDatabase.Remove(id);
        }
    }
}
