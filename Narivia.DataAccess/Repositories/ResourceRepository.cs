using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Resource repository implementation.
    /// </summary>
    public class ResourceRepository : IResourceRepository
    {
        readonly XmlDatabase<Resource> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.ResourceRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public ResourceRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<Resource>(fileName);
        }

        /// <summary>
        /// Adds the specified resource.
        /// </summary>
        /// <param name="resource">Resource.</param>
        public void Add(Resource resource)
        {
            xmlDatabase.Add(resource);
        }

        /// <summary>
        /// Gets the resource with the specified identifier.
        /// </summary>
        /// <returns>The resource.</returns>
        /// <param name="id">Identifier.</param>
        public Resource Get(string id)
        {
            return xmlDatabase.Get(id);
        }

        /// <summary>
        /// Gets all the resources.
        /// </summary>
        /// <returns>The resources.</returns>
        public IEnumerable<Resource> GetAll()
        {
            return xmlDatabase.GetAll();
        }

        /// <summary>
        /// Updates the specified resource.
        /// </summary>
        /// <param name="resource">Resource.</param>
        public void Update(Resource resource)
        {
            xmlDatabase.Update(resource);
        }

        /// <summary>
        /// Remove the resource with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            xmlDatabase.Remove(id);
        }
    }
}
