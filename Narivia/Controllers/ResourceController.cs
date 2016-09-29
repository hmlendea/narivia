using System.Collections.Generic;

using Narivia.Models;
using Narivia.Repositories;

namespace Narivia.Controllers
{
    public class ResourceController
    {
        readonly RepositoryXml<Resource> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Controllers.ResourceController"/> class.
        /// </summary>
        /// <param name="repository">Resource repository.</param>
        public ResourceController(RepositoryXml<Resource> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Adds the resource.
        /// </summary>
        /// <returns>The resource.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <param name="type">Type.</param>
        /// <param name="output">Output.</param>
        public void Create(string id, string name, string description, ResourceType type, int output)
        {
            Resource resource = new Resource();

            resource.Id = id;
            resource.Name = name;
            resource.Description = description;
            resource.Type = type;
            resource.Output = output;

            repository.Add(resource);
        }

        /// <summary>
        /// Gets the resource by identifier.
        /// </summary>
        /// <returns>The resource by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public Resource Get(string id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Gets all resources.
        /// </summary>
        /// <returns>The resources.</returns>
        public List<Resource> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Modifies the name and description.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <param name="type">Type.</param>
        /// <param name="output">Output.</param>
        public void Modify(string id, string name, string description, ResourceType type, int output)
        {
            Resource resource = Get(id);
            resource.Name = name;
            resource.Description = description;
            resource.Type = type;
            resource.Output = output;
        }

        /// <summary>
        /// Removes the resource.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Delete(string id)
        {
            repository.Remove(id);
        }
    }
}
