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
        /// <param name="entity">Resource.</param>
        public override void Update(ResourceEntity entity)
        {
            LoadEntitiesIfNeeded();

            ResourceEntity resourceEntityToUpdate = Get(entity.Id);

            if (resourceEntityToUpdate == null)
            {
                throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            }

            resourceEntityToUpdate.Name = entity.Name;
            resourceEntityToUpdate.Description = entity.Description;
            resourceEntityToUpdate.Type = entity.Type;
            resourceEntityToUpdate.Output = entity.Output;

            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
