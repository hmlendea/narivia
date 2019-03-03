using System.Collections.Generic;
using System.Linq;

using NuciXNA.Primitives;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// Flag mapping extensions for converting between entities and domain models.
    /// </summary>
    static class FlagMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="flagEntity">Flag entity.</param>
        internal static Flag ToDomainModel(this FlagEntity flagEntity)
        {
            Flag flag = new Flag
            {
                Id = flagEntity.Id,
                Layer1 = flagEntity.Layer1,
                Layer2 = flagEntity.Layer2,
                Emblem = flagEntity.Emblem,
                Skin = flagEntity.Skin,
                BackgroundColour = Colour.FromHexadecimal(flagEntity.BackgroundColourHexadecimal),
                Layer1Colour = Colour.FromHexadecimal(flagEntity.Layer1ColourHexadecimal),
                Layer2Colour = Colour.FromHexadecimal(flagEntity.Layer2ColourHexadecimal),
                EmblemColour = Colour.FromHexadecimal(flagEntity.EmblemColourHexadecimal)
            };

            return flag;
        }

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="flag">Flag.</param>
        internal static FlagEntity ToEntity(this Flag flag)
        {
            FlagEntity flagEntity = new FlagEntity
            {
                Id = flag.Id,
                Layer1 = flag.Layer1,
                Layer2 = flag.Layer2,
                Emblem = flag.Emblem,
                Skin = flag.Skin,
                BackgroundColourHexadecimal = flag.BackgroundColour.ToHexadecimal(),
                Layer1ColourHexadecimal = flag.Layer1Colour.ToHexadecimal(),
                Layer2ColourHexadecimal = flag.Layer2Colour.ToHexadecimal(),
                EmblemColourHexadecimal = flag.EmblemColour.ToHexadecimal()
            };

            return flagEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="flagEntities">Flag entities.</param>
        internal static IEnumerable<Flag> ToDomainModels(this IEnumerable<FlagEntity> flagEntities)
        {
            IEnumerable<Flag> flags = flagEntities.Select(flagEntity => flagEntity.ToDomainModel());

            return flags;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="flags">Flags.</param>
        internal static IEnumerable<FlagEntity> ToEntities(this IEnumerable<Flag> flags)
        {
            IEnumerable<FlagEntity> flagEntities = flags.Select(flag => flag.ToEntity());

            return flagEntities;
        }
    }
}
