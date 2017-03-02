using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    public class CultureRepository : ICultureRepository
    {
        readonly XmlDatabase<Culture> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.CultureRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public CultureRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<Culture>(fileName);
        }

        public void Add(Culture culture)
        {
            xmlDatabase.Add(culture);
        }

        public Culture Get(string id)
        {
            return xmlDatabase.Get(id);
        }

        public IEnumerable<Culture> GetAll()
        {
            return xmlDatabase.GetAll();
        }

        public void Update(Culture culture)
        {
            xmlDatabase.Update(culture);
        }

        public void Remove(string id)
        {
            xmlDatabase.Remove(id);
        }
    }
}
