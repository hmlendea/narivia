using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    internal static class ResourceMappingExtensions
    {
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

        internal static IEnumerable<Resource> ToDomainModels(this IEnumerable<ResourceEntity> resourceEntities)
        {
            IEnumerable<Resource> resources = resourceEntities.Select(resourceEntity => resourceEntity.ToDomainModel());

            return resources;
        }

        internal static IEnumerable<ResourceEntity> ToEntities(this IEnumerable<Resource> resources)
        {
            IEnumerable<ResourceEntity> resourceEntities = resources.Select(resource => resource.ToEntity());

            return resourceEntities;
        }
    }
}
