using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    public enum HoldingType
    {
        /// <summary>
        /// Fortress.
        /// </summary>
        Fortress = 0,

        /// <summary>
        /// City.
        /// </summary>L
        City = 1,

        /// <summary>
        /// Temple.
        /// </summary>
        Temple = 2,

        /// <summary>
        /// Village.
        /// </summary>
        Village = 3,

        /// <summary>
        /// Port.
        /// </summary>
        Port = 4
    }

    public class Holding : EntityBase
    {
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
        public HoldingType Type { get; set; }
    }
}
