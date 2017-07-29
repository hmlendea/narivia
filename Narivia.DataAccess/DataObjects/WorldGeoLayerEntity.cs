namespace Narivia.DataAccess.DataObjects
{
    /// <summary>
    /// World georaphic layer.
    /// </summary>
    public class WorldGeoLayerEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>The region identifier.</value>
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
