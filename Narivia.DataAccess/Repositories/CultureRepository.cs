using System.Collections.Generic;
using System.Linq;

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
            List<Culture> cultures = xmlDatabase.LoadEntities().ToList();
            cultures.Add(culture);

            xmlDatabase.SaveEntities(cultures);
        }

        /// <summary>
        /// Get the culture with the specified identifier.
        /// </summary>
        /// <returns>The culture.</returns>
        /// <param name="id">Identifier.</param>
        public Culture Get(string id)
        {
            List<Culture> cultures = xmlDatabase.LoadEntities().ToList();
            Culture culture = cultures.FirstOrDefault(x => x.Id == id);

            return culture;
        }

        /// <summary>
        /// Gets all the cultures.
        /// </summary>
        /// <returns>The cultures</returns>
        public IEnumerable<Culture> GetAll()
        {
            List<Culture> cultures = xmlDatabase.LoadEntities().ToList();

            return cultures;
        }

        /// <summary>
        /// Updates the specified culture.
        /// </summary>
        /// <param name="culture">Culture.</param>
        public void Update(Culture culture)
        {
            List<Culture> cultures = xmlDatabase.LoadEntities().ToList();
            Culture cultureToUpdate = cultures.FirstOrDefault(x => x.Id == culture.Id);

            cultureToUpdate.Name = culture.Name;
            cultureToUpdate.Description = culture.Description;
            cultureToUpdate.TextureSet = culture.TextureSet;

            xmlDatabase.SaveEntities(cultures);
        }

        /// <summary>
        /// Removes the culture with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<Culture> cultures = xmlDatabase.LoadEntities().ToList();
            cultures.RemoveAll(x => x.Id == id);

            xmlDatabase.SaveEntities(cultures);
        }
    }
}
