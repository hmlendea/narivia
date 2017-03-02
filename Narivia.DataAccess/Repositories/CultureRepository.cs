using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Culture repository implementation.
    /// </summary>
    public class CultureRepository : ICultureRepository
    {
        readonly XmlDatabase<Culture> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.CultureRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public CultureRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<Culture>(fileName);
        }

        /// <summary>
        /// Adds the specified culture.
        /// </summary>
        /// <param name="culture">Culture.</param>
        public void Add(Culture culture)
        {
            xmlDatabase.Add(culture);
        }

        /// <summary>
        /// Gets the culture with the specified identifier.
        /// </summary>
        /// <returns>The culture.</returns>
        /// <param name="id">Identifier.</param>
        public Culture Get(string id)
        {
            return xmlDatabase.Get(id);
        }

        /// <summary>
        /// Gets all the cultures.
        /// </summary>
        /// <returns>The cultures.</returns>
        public IEnumerable<Culture> GetAll()
        {
            return xmlDatabase.GetAll();
        }

        /// <summary>
        /// Updates the specified culture.
        /// </summary>
        /// <param name="culture">Culture.</param>
        public void Update(Culture culture)
        {
            xmlDatabase.Update(culture);
        }

        /// <summary>
        /// Remove the culture with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            xmlDatabase.Remove(id);
        }
    }
}
