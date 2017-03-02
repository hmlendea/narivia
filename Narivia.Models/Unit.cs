using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    public enum UnitType
    {
        /// <summary>
        /// Light.
        /// </summary>L
        Light = 0,

        /// <summary>
        /// Heavy.
        /// </summary>
        Heavy = 1,

        /// <summary>
        /// Cavalry.
        /// </summary>
        Cavalry = 1,

        /// <summary>
        /// Ranged.
        /// </summary>
        Ranged = 1
    }

    public class Unit : EntityBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [MaxLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public UnitType Type { get; set; }

        /// <summary>
        /// Gets or sets the power.
        /// </summary>
        /// <value>The power.</value>
        [Range(0, int.MaxValue)]
        public int Power { get; set; }

        /// <summary>
        /// Gets or sets the health.
        /// </summary>
        /// <value>The health.</value>
        [Range(0, int.MaxValue)]
        public int Health { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        /// <summary>
        /// Gets or sets the maintenance.
        /// </summary>
        /// <value>The maintenance.</value>
        [Range(0, int.MaxValue)]
        public int Maintenance { get; set; }
    }
}
