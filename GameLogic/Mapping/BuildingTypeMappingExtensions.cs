using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    static class BuildingTypeMappingExtensions
    {
        internal static BuildingType ToDomainModel(this BuildingTypeEntity entity)
        => new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                RequiredResourceId = entity.RequiredResourceId,
                Price = entity.Price,
                MaintenanceCost = entity.MaintenanceCost,
                Income = entity.Income,
                AttackBonus = entity.AttackBonus,
                DefenceBonus = entity.DefenceBonus,
                RecruitmentBonus = entity.RecruitmentBonus,
                ReligionInfluence = entity.ReligionInfluence
            };

        internal static BuildingTypeEntity ToEntity(this BuildingType domainModel)
        => new()
            {
                Id = domainModel.Id,
                Name = domainModel.Name,
                Description = domainModel.Description,
                RequiredResourceId = domainModel.RequiredResourceId,
                Price = domainModel.Price,
                MaintenanceCost = domainModel.MaintenanceCost,
                Income = domainModel.Income,
                AttackBonus = domainModel.AttackBonus,
                DefenceBonus = domainModel.DefenceBonus,
                RecruitmentBonus = domainModel.RecruitmentBonus,
                ReligionInfluence = domainModel.ReligionInfluence
            };

        internal static IEnumerable<BuildingType> ToDomainModels(this IEnumerable<BuildingTypeEntity> entities)
        => entities.Select(entity => entity.ToDomainModel());

        internal static IEnumerable<BuildingTypeEntity> ToEntities(this IEnumerable<BuildingType> domainModels)
        => domainModels.Select(domainModel => domainModel.ToEntity());
    }
}
