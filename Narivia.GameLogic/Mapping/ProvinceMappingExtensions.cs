using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models.Enumerations;

using Province= Narivia.Models.Province;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// Province mapping extensions for converting between entities and domain models.
    /// </summary>
    static class ProvinceMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="provinceEntity">Province entity.</param>
        internal static Province ToDomainModel(this ProvinceEntity provinceEntity)
        {
            Province province = new Province
            {
                Id = provinceEntity.Id,
                Name = provinceEntity.Name,
                Description = provinceEntity.Description,
                Colour = ColorTranslator.FromHtml(provinceEntity.ColourHexadecimal),
                Type = (ProvinceType)Enum.Parse(typeof(ProvinceType), provinceEntity.Type),
                ResourceId = provinceEntity.ResourceId,
                FactionId = provinceEntity.FactionId,
                SovereignFactionId = provinceEntity.SovereignFactionId
            };

            return province;
        }

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="province">Province.</param>
        internal static ProvinceEntity ToEntity(this Province province)
        {
            ProvinceEntity provinceEntity = new ProvinceEntity
            {
                Id = province.Id,
                Name = province.Name,
                Description = province.Description,
                ColourHexadecimal = ColorTranslator.ToHtml(province.Colour),
                Type = province.Type.ToString(),
                ResourceId = province.ResourceId,
                FactionId = province.FactionId,
                SovereignFactionId = province.SovereignFactionId
            };

            return provinceEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="provinceEntities">Province entities.</param>
        internal static IEnumerable<Province> ToDomainModels(this IEnumerable<ProvinceEntity> provinceEntities)
        {
            IEnumerable<Province> provinces = provinceEntities.Select(provinceEntity => provinceEntity.ToDomainModel());

            return provinces;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="provinces">Provinces.</param>
        internal static IEnumerable<ProvinceEntity> ToEntities(this IEnumerable<Province> provinces)
        {
            IEnumerable<ProvinceEntity> provinceEntities = provinces.Select(province => province.ToEntity());

            return provinceEntities;
        }
    }
}
