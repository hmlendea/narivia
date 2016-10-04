using Narivia.Models;

namespace Narivia.Repositories
{
    public class CultureRepository : RepositoryXml<Culture>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.CultureRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public CultureRepository(string fileName)
            : base(fileName)
        {

        }

        /// <summary>
        /// Adds the culture.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        public void Create(string id, string name, string description)
        {
            Culture culture = new Culture();

            culture.Id = id;
            culture.Name = name;
            culture.Description = description;

            Add(culture);
        }

        /// <summary>
        /// Modifies the culture.
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
    }
}
