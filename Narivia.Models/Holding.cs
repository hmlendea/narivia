using System.ComponentModel.DataAnnotations;

using Narivia.Models.Enumerations;

namespace Narivia.Models
{
    /// <summary>
    /// Holding domain model.
    /// </summary>
    public sealed class Holding : ModelBase
    {
        /// <summary>
        /// Gets or sets the identifier of the province to which this holding belongs to.
        /// </summary>
        /// <value>The province identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string ProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public HoldingType Type { get; set; }
    }
}
