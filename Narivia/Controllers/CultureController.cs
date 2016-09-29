using System.Collections.Generic;

using Narivia.Models;
using Narivia.Repositories;

namespace Narivia.Controllers
{
    public class CultureController
    {
        readonly RepositoryXml<Culture> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Controllers.CultureController"/> class.
        /// </summary>
        /// <param name="repository">Culture repository.</param>
        public CultureController(RepositoryXml<Culture> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Adds the culture.
        /// </summary>
        /// <returns>The culture.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        public void Create(string id, string name, string description)
        {
            Culture culture = new Culture();

            culture.Id = id;
            culture.Name = name;
            culture.Description = description;

            repository.Add(culture);
        }

        /// <summary>
        /// Gets the culture by identifier.
        /// </summary>
        /// <returns>The culture by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public Culture Get(string id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Gets all cultures.
        /// </summary>
        /// <returns>The cultures.</returns>
        public List<Culture> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Modifies the name and description.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        public void Modify(string id, string name, string description)
        {
            Culture culture = Get(id);
            culture.Name = name;
            culture.Description = description;
        }

        /// <summary>
        /// Removes the culture.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Delete(string id)
        {
            repository.Remove(id);
        }
    }
}
