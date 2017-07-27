using Microsoft.Xna.Framework;

namespace Narivia.Gui.WorldMap
{
    /// <summary>
    /// Map tile.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>The x.</value>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>The y.</value>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the source rectangle.
        /// </summary>
        /// <value>The source rectangle.</value>
        public Rectangle SourceRectangle { get; set; }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="sourceRectangle">Source rectangle.</param>
        public void LoadContent(int x, int y, Rectangle sourceRectangle)
        {
            X = x;
            Y = y;
            SourceRectangle = sourceRectangle;
        }
    }
}
