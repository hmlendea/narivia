using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Infrastructure.Exceptions;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Resource repository implementation.
    /// </summary>
    public class ResourceRepository : IResourceRepository
    {
        readonly XmlDatabase<ResourceEntity> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.ResourceRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public ResourceRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<ResourceEntity>(fileName);
        }

        /// <summary>
        /// Adds the specified resource.
        /// </summary>
        /// <param name="resourceEntity">Resource.</param>
        public void Add(ResourceEntity resourceEntity)
        {
            List<ResourceEntity> resourceEntities = xmlDatabase.LoadEntities().ToList();
            resourceEntities.Add(resourceEntity);

            try
            {
                xmlDatabase.SaveEntities(resourceEntities);
            }
            catch
            {
                throw new DuplicateEntityException(resourceEntity.Id, nameof(ResourceEntity).Replace("Entity", ""));
            }
        }

        /// <summary>
        /// Get the resource with the specified identifier.
        /// </summary>
        /// <returns>The resource.</returns>
        /// <param name="id">Identifier.</param>
        public ResourceEntity Get(string id)
        {
            List<ResourceEntity> resourceEntities = xmlDatabase.LoadEntities().ToList();
            ResourceEntity resourceEntity = resourceEntities.FirstOrDefault(x => x.Id == id);

            if (resourceEntity == null)
            {
                throw new EntityNotFoundException(id, nameof(BorderEntity).Replace("Entity", ""));
            }

            return resourceEntity;
        }

        /// <summary>
        /// Gets all the resources.
        /// </summary>
        /// <returns>The resources</returns>
        public IEnumerable<ResourceEntity> GetAll()
        {
            List<ResourceEntity> resourceEntities = xmlDatabase.LoadEntities().ToList();

            return resourceEntities;
        }

        /// <summary>
        /// Updates the specified resource.
        /// </summary>
        /// <param name="resourceEntity">Resource.</param>
        public void Update(ResourceEntity resourceEntity)
        {
            List<ResourceEntity> resourceEntities = xmlDatabase.LoadEntities().ToList();
            ResourceEntity resourceEntityToUpdate = resourceEntities.FirstOrDefault(x => x.Id == resourceEntity.Id);

            if (resourceEntityToUpdate == null)
            {
                throw new EntityNotFoundException(resourceEntity.Id, nameof(BorderEntity).Replace("Entity", ""));
            }

            resourceEntityToUpdate.Name = resourceEntity.Name;
            resourceEntityToUpdate.Description = resourceEntity.Description;
            resourceEntityToUpdate.Type = resourceEntity.Type;
            resourceEntityToUpdate.Output = resourceEntity.Output;

            xmlDatabase.SaveEntities(resourceEntities);
        }

        /// <summary>
        /// Removes the resource with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<ResourceEntity> resourceEntities = xmlDatabase.LoadEntities().ToList();
            resourceEntities.RemoveAll(x => x.Id == id);

            try
            {
                xmlDatabase.SaveEntities(resourceEntities);
            }
            catch
            {
                throw new DuplicateEntityException(id, nameof(ResourceEntity).Replace("Entity", ""));
            }
        }
    }
}
