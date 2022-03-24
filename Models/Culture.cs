using System.ComponentModel.DataAnnotations;

using Narivia.Models.Enumerations;

namespace Narivia.Models
{
    /// <summary>
    /// Culture domain model.
    /// </summary>
    public sealed class Culture : ModelBase
    {
        /// <summary>
        /// Gets or sets the texture set.
        /// </summary>
        /// <value>The texture set.</value>
        [StringLength(20, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string TextureSet { get; set; }

        /// <summary>
        /// Gets or sets the place name generator.
        /// </summary>
        /// <value>The place name generator.</value>
        public NameGenerator PlaceNameGenerator { get; set; }

        /// <summary>
        /// Gets or sets the place name generator schema.
        /// </summary>
        /// <value>The place name generator schema.</value>
        public string PlaceNameSchema { get; set; }
    }
}
