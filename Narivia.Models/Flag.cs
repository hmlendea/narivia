using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Narivia.Models
{
    /// <summary>
    /// Faction flag.
    /// </summary>
    public sealed class Flag : ModelBase
    {
        /// <summary>
        /// Gets or sets the first layer.
        /// </summary>
        /// <value>The first layer.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Layer1 { get; set; }

        /// <summary>
        /// Gets or sets the second layer.
        /// </summary>
        /// <value>The second layer.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Layer2 { get; set; }

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
        /// Gets or sets the background colour.
        /// </summary>
        /// <value>The background colour.</value>
        public Color BackgroundColour { get; set; }

        /// <summary>
        /// Gets or sets the first layer's colour.
        /// </summary>
        /// <value>The first layer's colour.</value>
        public Color Layer1Colour { get; set; }

        /// <summary>
        /// Gets or sets the second layer's colour.
        /// </summary>
        /// <value>The second layer's colour.</value>
        public Color Layer2Colour { get; set; }

        /// <summary>
        /// Gets or sets the emblem colour.
        /// </summary>
        /// <value>The emblem colour.</value>
        public Color EmblemColour { get; set; }
    }
}
