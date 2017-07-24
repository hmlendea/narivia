using System.ComponentModel.DataAnnotations;

using Narivia.Infrastructure.Helpers;

namespace Narivia.Models
{
    /// <summary>
    /// Faction flag.
    /// </summary>
    public class Flag
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the background.
        /// </summary>
        /// <value>The background.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Background { get; set; }

        /// <summary>
        /// Gets or sets the emblem.
        /// </summary>
        /// <value>The emblem.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Emblem { get; set; }

        /// <summary>
        /// Gets or sets the skin.
        /// </summary>
        /// <value>The skin.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Skin { get; set; }

        /// <summary>
        /// Gets or sets the primary colour.
        /// </summary>
        /// <value>The primary colour.</value>
        public Colour PrimaryColour { get; set; }

        /// <summary>
        /// Gets or sets the secondary colour.
        /// </summary>
        /// <value>The secondary colour.</value>
        public Colour SecondaryColour { get; set; }
    }
}
