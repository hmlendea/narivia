using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    public class ResourceRepository : RepositoryXml<Resource>, IResourceRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.ResourceRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public ResourceRepository(string fileName)
            : base(fileName)
        {

        }

        /// <summary>
        /// Adds the resource.
        /// </summary>
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

            Add(resource);
        }

        /// <summary>
        /// Modifies the resource.
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
    }
}
