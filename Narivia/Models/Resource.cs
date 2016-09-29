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

    public class Resource : EntityBase
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
        public ResourceType Type { get; set; }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>The output.</value>
        [Range(0, 100)]
        public int Output { get; set; }
    }
}
