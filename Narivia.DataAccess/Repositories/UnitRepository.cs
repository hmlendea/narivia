using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        readonly XmlDatabase<Unit> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.UnitRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public UnitRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<Unit>(fileName);
        }

        public void Add(Unit unit)
        {
            xmlDatabase.Add(unit);
        }

        public Unit Get(string id)
        {
            return xmlDatabase.Get(id);
        }

        public IEnumerable<Unit> GetAll()
        {
            return xmlDatabase.GetAll();
        }

        public void Update(Unit unit)
        {
            xmlDatabase.Update(unit);
        }

        public void Remove(string id)
        {
            xmlDatabase.Remove(id);
        }
    }
}
