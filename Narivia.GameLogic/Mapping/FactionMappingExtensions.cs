using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Infrastructure.Helpers;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    static class FactionMappingExtensions
    {
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

        internal static IEnumerable<Faction> ToDomainModels(this IEnumerable<FactionEntity> factionEntities)
        {
            IEnumerable<Faction> factions = factionEntities.Select(factionEntity => factionEntity.ToDomainModel());

            return factions;
        }

        internal static IEnumerable<FactionEntity> ToEntities(this IEnumerable<Faction> factions)
        {
            IEnumerable<FactionEntity> factionEntities = factions.Select(faction => faction.ToEntity());

            return factionEntities;
        }
    }
}
