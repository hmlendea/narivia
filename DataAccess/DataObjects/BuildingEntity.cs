using NuciXNA.DataAccess.DataObjects;

namespace Narivia.DataAccess.DataObjects
{
    public class BuildingEntity : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string ProvinceId { get; set; }
    }
}
