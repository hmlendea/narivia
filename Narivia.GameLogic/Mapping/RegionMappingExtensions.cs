using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Infrastructure.Helpers;
using Narivia.Models;
using Narivia.Models.Enumerations;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// Region mapping extensions for converting between entities and domain models.
    /// </summary>
    static class RegionMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="regionEntity">Region entity.</param>
        internal static Region ToDomainModel(this RegionEntity regionEntity)
        {
            Region region = new Region
            {
                Id = regionEntity.Id,
                Name = regionEntity.Name,
                Description = regionEntity.Description,
                Colour = Colour.FromHexadecimal(regionEntity.ColourHexadecimal),
                Type = (RegionType)Enum.Parse(typeof(RegionType), regionEntity.Type),
                ResourceId = regionEntity.ResourceId,
                FactionId = regionEntity.FactionId,
                SovereignFactionId = regionEntity.SovereignFactionId
            };

            return region;
        }

        /// <summary>
        /// Converts the domail model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="region">Region.</param>
        internal static RegionEntity ToEntity(this Region region)
        {
            RegionEntity regionEntity = new RegionEntity
            {
                Id = region.Id,
                Name = region.Name,
                Description = region.Description,
                ColourHexadecimal = region.Colour.ToHexadecimal(),
                Type = region.Type.ToString(),
                ResourceId = region.ResourceId,
                FactionId = region.FactionId,
                SovereignFactionId = region.SovereignFactionId
            };

            return regionEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="regionEntities">Region entities.</param>
        internal static IEnumerable<Region> ToDomainModels(this IEnumerable<RegionEntity> regionEntities)
        {
            IEnumerable<Region> regions = regionEntities.Select(regionEntity => regionEntity.ToDomainModel());

            return regions;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="regions">Regions.</param>
        internal static IEnumerable<RegionEntity> ToEntities(this IEnumerable<Region> regions)
        {
            IEnumerable<RegionEntity> regionEntities = regions.Select(region => region.ToEntity());

            return regionEntities;
        }
    }
}
