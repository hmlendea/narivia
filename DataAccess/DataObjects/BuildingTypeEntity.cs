using NuciDAL.DataObjects;

namespace Narivia.DataAccess.DataObjects
{
    public class BuildingTypeEntity : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string RequiredResourceId { get; set; }

        public int Price { get; set; }

        public int MaintenanceCost { get; set; }

        public int Income { get; set; }

        public int AttackBonus { get; set; }

        public int DefenceBonus { get; set; }

        public int RecruitmentBonus { get; set; }

        public int ReligionInfluence { get; set; }
    }
}
