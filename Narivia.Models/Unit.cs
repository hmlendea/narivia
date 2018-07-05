using System;
using System.ComponentModel.DataAnnotations;

using Narivia.Models.Enumerations;

namespace Narivia.Models
{
    /// <summary>
    /// Unit domain model.
    /// </summary>
    public sealed class Unit : ModelBase
    {
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
