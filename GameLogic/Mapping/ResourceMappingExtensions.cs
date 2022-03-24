using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;
using Narivia.Models.Enumerations;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// Resource mapping extensions for converting between entities and domain models.
    /// </summary>
    static class ResourceMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="resourceEntity">Resource entity.</param>
        internal static Resource ToDomainModel(this ResourceEntity resourceEntity)
        {
            Resource resource = new Resource
            {
                Id = resourceEntity.Id,
                Name = resourceEntity.Name,
                Description = resourceEntity.Description,
                Type = (ResourceType)Enum.Parse(typeof(ResourceType), resourceEntity.Type),
                Output = resourceEntity.Output
            };

            return resource;
        }

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="resource">Resource.</param>
        internal static ResourceEntity ToEntity(this Resource resource)
        {
            ResourceEntity resourceEntity = new ResourceEntity
            {
                Id = resource.Id,
                Name = resource.Name,
                Description = resource.Description,
                Type = resource.Type.ToString(),
                Output = resource.Output
            };

            return resourceEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="resourceEntities">Resource entities.</param>
        internal static IEnumerable<Resource> ToDomainModels(this IEnumerable<ResourceEntity> resourceEntities)
        {
            IEnumerable<Resource> resources = resourceEntities.Select(resourceEntity => resourceEntity.ToDomainModel());

            return resources;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="resources">Resources.</param>
        internal static IEnumerable<ResourceEntity> ToEntities(this IEnumerable<Resource> resources)
        {
            IEnumerable<ResourceEntity> resourceEntities = resources.Select(resource => resource.ToEntity());

            return resourceEntities;
        }
    }
}
