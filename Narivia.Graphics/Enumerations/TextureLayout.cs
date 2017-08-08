namespace Narivia.Graphics.Enumerations
{
    /// <summary>
    /// Specifies how the <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/> is sized within the <see cref="Sprite"/>
    /// </summary>
    public enum TextureLayout
    {
        /// <summary>
        /// The image is centered within the client rectangle.
        /// </summary>
        Centre,

        /// <summary>
        /// The image is left-aligned at the top across the client rectangle.
        /// </summary>
        None,

        /// <summary>
        /// The image is streched across the client rectangle.
        /// </summary>
        Stretch,

        /// <summary>
        /// The image is tiled across the client rectangle.
        /// </summary>
        Tile,

        /// <summary>
        /// The image is enlarged within the client rectangle.
        /// </summary>
        Zoom
    }
}
