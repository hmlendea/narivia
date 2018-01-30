using NuciXNA.DataAccess.DataObjects;

namespace Narivia.DataAccess.DataObjects
{
    /// <summary>
    /// Army data entity.
    /// </summary>
    public class ArmyEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the faction identifier.
        /// </summary>
        /// <value>The faction identifier.</value>
        public string FactionId { get; set; }

        /// <summary>
        /// Gets or sets the unit identifier.
        /// </summary>
        /// <value>The unit identifier.</value>
        public string UnitId { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public int Size { get; set; }
    }
}
