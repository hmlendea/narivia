using NuciXNA.DataAccess.DataObjects;

namespace Narivia.DataAccess.DataObjects
{
    /// <summary>
    /// Unit data entity.
    /// </summary>
    public class UnitEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the power.
        /// </summary>
        /// <value>The power.</value>
        public int Power { get; set; }

        /// <summary>
        /// Gets or sets the health.
        /// </summary>
        /// <value>The health.</value>
        public int Health { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        public int Price { get; set; }

        /// <summary>
        /// Gets or sets the maintenance.
        /// </summary>
        /// <value>The maintenance.</value>
        public int Maintenance { get; set; }
    }
}
