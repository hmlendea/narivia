using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    public class Culture : EntityBase
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
        /// Gets or sets the texture set.
        /// </summary>
        /// <value>The texture set.</value>
        [MaxLength(20)]
        public string TextureSet { get; set; }
    }
}
