using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    public class FactionRepository : IFactionRepository
    {
        readonly XmlDatabase<Faction> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.FactionRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public FactionRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<Faction>(fileName);
        }

        public void Add(Faction faction)
        {
            xmlDatabase.Add(faction);
        }

        public Faction Get(string id)
        {
            return xmlDatabase.Get(id);
        }

        public IEnumerable<Faction> GetAll()
        {
            return xmlDatabase.GetAll();
        }

        public void Update(Faction faction)
        {
            xmlDatabase.Update(faction);
        }

        public void Remove(string id)
        {
            xmlDatabase.Remove(id);
        }
    }
}
