using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.BusinessLogic.Mapping
{
    internal static class CultureMappingExtensions
    {
        internal static Culture ToDomainModel(this CultureEntity cultureEntity)
        {
            Culture culture = new Culture
            {
                Id = cultureEntity.Id,
                Name = cultureEntity.Name,
                Description = cultureEntity.Description,
                TextureSet = cultureEntity.TextureSet
            };

            return culture;
        }

        internal static CultureEntity ToEntity(this Culture culture)
        {
            CultureEntity cultureEntity = new CultureEntity
            {
                Id = culture.Id,
                Name = culture.Name,
                Description = culture.Description,
                TextureSet = culture.TextureSet
            };

            return cultureEntity;
        }

        internal static IEnumerable<Culture> ToDomainModels(this IEnumerable<CultureEntity> cultureEntities)
        {
            IEnumerable<Culture> cultures = cultureEntities.Select(cultureEntity => cultureEntity.ToDomainModel());

            return cultures;
        }

        internal static IEnumerable<CultureEntity> ToEntities(this IEnumerable<Culture> cultures)
        {
            IEnumerable<CultureEntity> cultureEntities = cultures.Select(culture => culture.ToEntity());

            return cultureEntities;
        }
    }
}
