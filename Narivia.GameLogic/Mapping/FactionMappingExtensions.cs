using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Infrastructure.Helpers;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// Faction mapping extensions for converting between entities and domain models.
    /// </summary>
    static class FactionMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="factionEntity">Faction entity.</param>
        internal static Faction ToDomainModel(this FactionEntity factionEntity)
        {
            Faction faction = new Faction
            {
                Id = factionEntity.Id,
                Name = factionEntity.Name,
                Description = factionEntity.Description,
                Colour = ColourTranslator.FromHexadecimal(factionEntity.ColourHexadecimal),
                CultureId = factionEntity.CultureId
            };

            return faction;
        }

        /// <summary>
        /// Converts the domail model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="faction">Faction.</param>
        internal static FactionEntity ToEntity(this Faction faction)
        {
            FactionEntity factionEntity = new FactionEntity
            {
                Id = faction.Id,
                Name = faction.Name,
                Description = faction.Description,
                ColourHexadecimal = ColourTranslator.ToHexadecimal(faction.Colour),
                CultureId = faction.CultureId
            };

            return factionEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="factionEntities">Faction entities.</param>
        internal static IEnumerable<Faction> ToDomainModels(this IEnumerable<FactionEntity> factionEntities)
        {
            IEnumerable<Faction> factions = factionEntities.Select(factionEntity => factionEntity.ToDomainModel());

            return factions;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="factions">Factions.</param>
        internal static IEnumerable<FactionEntity> ToEntities(this IEnumerable<Faction> factions)
        {
            IEnumerable<FactionEntity> factionEntities = factions.Select(faction => faction.ToEntity());

            return factionEntities;
        }
    }
}
