using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    static class WorldMappingExtensions
    {
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
                StartingWealth = worldEntity.StartingWealth,
                StartingTroops = worldEntity.StartingTroops
            };

            return world;
        }

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
                StartingWealth = world.StartingWealth,
                StartingTroops = world.StartingTroops
            };

            return worldEntity;
        }

        internal static IEnumerable<World> ToDomainModels(this IEnumerable<WorldEntity> worldEntities)
        {
            IEnumerable<World> worlds = worldEntities.Select(worldEntity => worldEntity.ToDomainModel());

            return worlds;
        }

        internal static IEnumerable<WorldEntity> ToEntities(this IEnumerable<World> worlds)
        {
            IEnumerable<WorldEntity> worldEntities = worlds.Select(world => world.ToEntity());

            return worldEntities;
        }
    }
}
