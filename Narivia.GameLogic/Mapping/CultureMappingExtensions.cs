using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;
using Narivia.Models.Enumerations;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// Culture mapping extensions for converting between entities and domain models.
    /// </summary>
    static class CultureMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="cultureEntity">Culture entity.</param>
        internal static Culture ToDomainModel(this CultureEntity cultureEntity)
        {
            Culture culture = new Culture
            {
                Id = cultureEntity.Id,
                Name = cultureEntity.Name,
                Description = cultureEntity.Description,
                TextureSet = cultureEntity.TextureSet,
                PlaceNameGenerator = (NameGenerator)Enum.Parse(typeof(NameGenerator), cultureEntity.PlaceNameGenerator),
                PlaceNameSchema = cultureEntity.PlaceNameSchema
            };

            return culture;
        }

        /// <summary>
        /// Converts the domail model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="culture">Culture.</param>
        internal static CultureEntity ToEntity(this Culture culture)
        {
            CultureEntity cultureEntity = new CultureEntity
            {
                Id = culture.Id,
                Name = culture.Name,
                Description = culture.Description,
                TextureSet = culture.TextureSet,
                PlaceNameGenerator = culture.PlaceNameGenerator.ToString(),
                PlaceNameSchema = culture.PlaceNameSchema
            };

            return cultureEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="cultureEntities">Culture entities.</param>
        internal static IEnumerable<Culture> ToDomainModels(this IEnumerable<CultureEntity> cultureEntities)
        {
            IEnumerable<Culture> cultures = cultureEntities.Select(cultureEntity => cultureEntity.ToDomainModel());

            return cultures;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="cultures">Cultures.</param>
        internal static IEnumerable<CultureEntity> ToEntities(this IEnumerable<Culture> cultures)
        {
            IEnumerable<CultureEntity> cultureEntities = cultures.Select(culture => culture.ToEntity());

            return cultureEntities;
        }
    }
}
