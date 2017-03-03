using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Infrastructure.Helpers;
using Narivia.Models;

namespace Narivia.BusinessLogic.Mapping
{
    internal static class BiomeMappingExtensions
    {
        internal static Biome ToDomainModel(this BiomeEntity biomeEntity)
        {
            Biome biome = new Biome
            {
                Id = biomeEntity.Id,
                Name = biomeEntity.Name,
                Description = biomeEntity.Description,
                Colour = ColourTranslator.FromHexadecimal(biomeEntity.ColourHexadecimal)
            };

            return biome;
        }

        internal static BiomeEntity ToEntity(this Biome biome)
        {
            BiomeEntity biomeEntity = new BiomeEntity
            {
                Id = biome.Id,
                Name = biome.Name,
                Description = biome.Description,
                ColourHexadecimal = ColourTranslator.ToHexadecimal(biome.Colour)
            };

            return biomeEntity;
        }

        internal static IEnumerable<Biome> ToDomainModels(this IEnumerable<BiomeEntity> biomeEntities)
        {
            IEnumerable<Biome> biomes = biomeEntities.Select(biomeEntity => biomeEntity.ToDomainModel());

            return biomes;
        }

        internal static IEnumerable<BiomeEntity> ToEntities(this IEnumerable<Biome> biomes)
        {
            IEnumerable<BiomeEntity> biomeEntities = biomes.Select(biome => biome.ToEntity());

            return biomeEntities;
        }
    }
}
