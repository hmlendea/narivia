using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    /// <summary>
    /// World georaphic layer.
    /// </summary>
    public class WorldGeoLayer
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>The region identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Tileset { get; set; }

        /// <summary>
        /// Gets or sets the layer tiles.
        /// </summary>
        /// <value>The layer tiles.</value>
        public int[,] Tiles { get; set; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }
    }
}
