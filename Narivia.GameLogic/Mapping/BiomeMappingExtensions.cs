using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// Biome mapping extensions for converting between entities and domain models.
    /// </summary>
    static class BiomeMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="biomeEntity">Biome entity.</param>
        internal static Biome ToDomainModel(this BiomeEntity biomeEntity)
        {
            Biome biome = new Biome
            {
                Id = biomeEntity.Id,
                Name = biomeEntity.Name,
                Description = biomeEntity.Description,
                Colour = ColorTranslator.FromHtml(biomeEntity.ColourHexadecimal)
            };

            return biome;
        }

        /// <summary>
        /// Converts the domail model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="biome">Biome.</param>
        internal static BiomeEntity ToEntity(this Biome biome)
        {
            BiomeEntity biomeEntity = new BiomeEntity
            {
                Id = biome.Id,
                Name = biome.Name,
                Description = biome.Description,
                ColourHexadecimal = ColorTranslator.ToHtml(biome.Colour)
            };

            return biomeEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="biomeEntities">Biome entities.</param>
        internal static IEnumerable<Biome> ToDomainModels(this IEnumerable<BiomeEntity> biomeEntities)
        {
            IEnumerable<Biome> biomes = biomeEntities.Select(biomeEntity => biomeEntity.ToDomainModel());

            return biomes;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="biomes">Biomes.</param>
        internal static IEnumerable<BiomeEntity> ToEntities(this IEnumerable<Biome> biomes)
        {
            IEnumerable<BiomeEntity> biomeEntities = biomes.Select(biome => biome.ToEntity());

            return biomeEntities;
        }
    }
}
