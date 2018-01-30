using System.Linq;

using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Resource repository implementation.
    /// </summary>
    public class ResourceRepository : XmlRepository<ResourceEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public ResourceRepository(string fileName) : base(fileName)
        {

        }
        
        /// <summary>
        /// Updates the specified resource.
        /// </summary>
        /// <param name="resourceEntity">Resource.</param>
        public override void Update(ResourceEntity resourceEntity)
        {
            LoadEntitiesIfNeeded();

            ResourceEntity resourceEntityToUpdate = Entities.FirstOrDefault(x => x.Id == resourceEntity.Id);

            if (resourceEntityToUpdate == null)
            {
                throw new EntityNotFoundException(resourceEntity.Id, nameof(BorderEntity));
            }

            resourceEntityToUpdate.Name = resourceEntity.Name;
            resourceEntityToUpdate.Description = resourceEntity.Description;
            resourceEntityToUpdate.Type = resourceEntity.Type;
            resourceEntityToUpdate.Output = resourceEntity.Output;

            XmlFile.SaveEntities(Entities);
        }
    }
}
