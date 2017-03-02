using System.Collections.Generic;
using System.Linq;

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
            List<Resource> resources = xmlDatabase.LoadEntities().ToList();
            resources.Add(resource);

            xmlDatabase.SaveEntities(resources);
        }

        /// <summary>
        /// Get the resource with the specified identifier.
        /// </summary>
        /// <returns>The resource.</returns>
        /// <param name="id">Identifier.</param>
        public Resource Get(string id)
        {
            List<Resource> resources = xmlDatabase.LoadEntities().ToList();
            Resource resource = resources.FirstOrDefault(x => x.Id == id);

            return resource;
        }

        /// <summary>
        /// Gets all the resources.
        /// </summary>
        /// <returns>The resources</returns>
        public IEnumerable<Resource> GetAll()
        {
            List<Resource> resources = xmlDatabase.LoadEntities().ToList();

            return resources;
        }

        /// <summary>
        /// Updates the specified resource.
        /// </summary>
        /// <param name="resource">Resource.</param>
        public void Update(Resource resource)
        {
            List<Resource> resources = xmlDatabase.LoadEntities().ToList();
            Resource resourceToUpdate = resources.FirstOrDefault(x => x.Id == resource.Id);

            resourceToUpdate.Name = resource.Name;
            resourceToUpdate.Description = resource.Description;
            resourceToUpdate.Type = resource.Type;
            resourceToUpdate.Output = resource.Output;

            xmlDatabase.SaveEntities(resources);
        }

        /// <summary>
        /// Removes the resource with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<Resource> resources = xmlDatabase.LoadEntities().ToList();
            resources.RemoveAll(x => x.Id == id);

            xmlDatabase.SaveEntities(resources);
        }
    }
}
