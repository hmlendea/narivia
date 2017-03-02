using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    public enum ResourceType
    {
        /// <summary>
        /// Wealth.
        /// </summary>L
        Wealth = 0,

        /// <summary>
        /// Military.
        /// </summary>
        Military = 1,
    }

    /// <summary>
    /// Resource domain model.
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [StringLength(20, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [StringLength(300, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public ResourceType Type { get; set; }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>The output.</value>
        [Range(0, 100)]
        public int Output { get; set; }
    }
}
