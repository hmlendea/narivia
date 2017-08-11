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
        /// Gets or sets the province identifier.
        /// </summary>
        /// <value>The province identifier.</value>
        public string Tileset { get; set; }

        /// <summary>
        /// Gets or sets the layer tiles.
        /// </summary>
        /// <value>The layer tiles.</value>
        public int[,] Tiles { get; set; }

        /// <summary>
        /// Gets or sets the layer opacity.
        /// </summary>
        /// <value>The layer opacity.</value>
        public float Opacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WorldGeoLayerEntity"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible { get; set; }
    }
}
