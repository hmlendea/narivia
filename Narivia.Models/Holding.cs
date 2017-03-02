using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    public enum HoldingType
    {
        /// <summary>
        /// City.
        /// </summary>L
        City = 0,

        /// <summary>
        /// Port.
        /// </summary>
        Port = 1,

        /// <summary>
        /// Village.
        /// </summary>
        Village = 1,

        /// <summary>
        /// Fortress.
        /// </summary>
        Fortress = 1,

        /// <summary>
        /// Temple.
        /// </summary>
        Temple = 1
    }

    public class Holding : EntityBase
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
        public HoldingType Type { get; set; }
    }
}
