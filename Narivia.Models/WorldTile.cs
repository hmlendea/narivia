using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    public class WorldTile
    {
        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>The region identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string RegionId { get; set; }

        /// <summary>
        /// Gets the biome identifier.
        /// </summary>
        /// <value>The biome identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string BiomeId { get; set; }
    }
}
