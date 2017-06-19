using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// World mapping extensions for converting between entities and domain models.
    /// </summary>
    static class WorldMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="worldEntity">World entity.</param>
        internal static World ToDomainModel(this WorldEntity worldEntity)
        {
            World world = new World
            {
                Id = worldEntity.Id,
                Name = worldEntity.Name,
                Description = worldEntity.Description,
                Author = worldEntity.Author,
                ResourcePack = worldEntity.ResourcePack,
                Version = worldEntity.Version,
                Width = worldEntity.Width,
                Height = worldEntity.Height,
                BaseRegionIncome = worldEntity.BaseRegionIncome,
                BaseRegionRecruitment = worldEntity.BaseRegionRecruitment,
                BaseFactionRecruitment = worldEntity.BaseFactionRecruitment,
                MinTroopsPerAttack = worldEntity.MinTroopsPerAttack,
                HoldingSlotsPerFaction = worldEntity.HoldingSlotsPerFaction,
                StartingWealth = worldEntity.StartingWealth,
                StartingTroops = worldEntity.StartingTroops,
                HoldingsPrice = worldEntity.HoldingsPrice
            };

            return world;
        }

        /// <summary>
        /// Converts the domail model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="world">World.</param>
        internal static WorldEntity ToEntity(this World world)
        {
            WorldEntity worldEntity = new WorldEntity
            {
                Id = world.Id,
                Name = world.Name,
                Description = world.Description,
                Author = world.Author,
                ResourcePack = world.ResourcePack,
                Version = world.Version,
                Width = world.Width,
                Height = world.Height,
                BaseRegionIncome = world.BaseRegionIncome,
                BaseRegionRecruitment = world.BaseRegionRecruitment,
                BaseFactionRecruitment = world.BaseFactionRecruitment,
                MinTroopsPerAttack = world.MinTroopsPerAttack,
                HoldingSlotsPerFaction = world.HoldingSlotsPerFaction,
                StartingWealth = world.StartingWealth,
                StartingTroops = world.StartingTroops,
                HoldingsPrice = world.HoldingsPrice
            };

            return worldEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="worldEntities">World entities.</param>
        internal static IEnumerable<World> ToDomainModels(this IEnumerable<WorldEntity> worldEntities)
        {
            IEnumerable<World> worlds = worldEntities.Select(worldEntity => worldEntity.ToDomainModel());

            return worlds;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="worlds">Worlds.</param>
        internal static IEnumerable<WorldEntity> ToEntities(this IEnumerable<World> worlds)
        {
            IEnumerable<WorldEntity> worldEntities = worlds.Select(world => world.ToEntity());

            return worldEntities;
        }
    }
}
