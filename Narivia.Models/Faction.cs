using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Xml.Serialization;

namespace Narivia.Models
{
    /// <summary>
    /// Faction domain model.
    /// </summary>
    public sealed class Faction : ModelBase
    {
        /// <summary>
        /// Gets or sets the flag identifier.
        /// </summary>
        /// <value>The flag identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string FlagId { get; set; }

        /// <summary>
        /// Gets or sets the culture identifier.
        /// </summary>
        /// <value>The culture identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string CultureId { get; set; }

        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        /// <value>The colour.</value>
        [XmlIgnore]
        public Color Colour { get; set; }

        /// <summary>
        /// Gets or sets the wealth.
        /// </summary>
        /// <value>The wealth.</value>
        [Range(0, int.MaxValue)]
        public int Wealth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Faction"/> is alive.
        /// </summary>
        /// <value><c>true</c> if alive; otherwise, <c>false</c>.</value>
        public bool Alive { get; set; }
    }
}
