using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Infrastructure.Helpers;
using Narivia.Models;

namespace Narivia.BusinessLogic.Mapping
{
    internal static class RegionMappingExtensions
    {
        internal static Region ToDomainModel(this RegionEntity regionEntity)
        {
            Region region = new Region
            {
                Id = regionEntity.Id,
                Name = regionEntity.Name,
                Description = regionEntity.Description,
                Colour = ColourTranslator.FromHexadecimal(regionEntity.ColourHexadecimal),
                Type = (RegionType)Enum.Parse(typeof(RegionType), regionEntity.Type),
                ResourceId = regionEntity.ResourceId,
                FactionId = regionEntity.FactionId,
                SovereignFactionId = regionEntity.SovereignFactionId
            };

            return region;
        }

        internal static RegionEntity ToEntity(this Region region)
        {
            RegionEntity regionEntity = new RegionEntity
            {
                Id = region.Id,
                Name = region.Name,
                Description = region.Description,
                ColourHexadecimal = ColourTranslator.ToHexadecimal(region.Colour),
                Type = region.Type.ToString(),
                ResourceId = region.ResourceId,
                FactionId = region.FactionId,
                SovereignFactionId = region.SovereignFactionId
            };

            return regionEntity;
        }

        internal static IEnumerable<Region> ToDomainModels(this IEnumerable<RegionEntity> regionEntities)
        {
            IEnumerable<Region> regions = regionEntities.Select(regionEntity => regionEntity.ToDomainModel());

            return regions;
        }

        internal static IEnumerable<RegionEntity> ToEntities(this IEnumerable<Region> regions)
        {
            IEnumerable<RegionEntity> regionEntities = regions.Select(region => region.ToEntity());

            return regionEntities;
        }
    }
}
