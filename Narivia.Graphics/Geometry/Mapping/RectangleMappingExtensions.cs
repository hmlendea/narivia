using SystemRectangle = System.Drawing.Rectangle;
using XnaRectangle = Microsoft.Xna.Framework.Rectangle;

namespace Narivia.Graphics.Geometry.Mapping
{
    public static class RectangleMappingExtensions
    {
        // >>> TO NARIVIA

        /// <summary>
        /// Convers a <see cref="SystemRectangle"/> into to a <see cref="Rectangle2D"/>.
        /// </summary>
        /// <param name="source">Source <see cref="SystemRectangle"/>.</param>
        /// <returns>The <see cref="Rectangle2D"/>.</returns>
        public static Rectangle2D ToRectangle2D(this SystemRectangle source)
        => new Rectangle2D(source.X, source.Y, source.Width, source.Height);

        /// <summary>
        /// Convers a <see cref="XnaRectangle"/> into to a <see cref="Rectangle2D"/>.
        /// </summary>
        /// <param name="source">Source <see cref="XnaRectangle"/>.</param>
        /// <returns>The <see cref="Rectangle2D"/>.</returns>
        public static Rectangle2D ToRectangle2D(this XnaRectangle source)
        => new Rectangle2D(source.X, source.Y, source.Width, source.Height);

        // >>> TO SYSTEM

        /// <summary>
        /// Convers a <see cref="Rectangle2D"/> into to a <see cref="SystemRectangle"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Rectangle2D"/>.</param>
        /// <returns>The <see cref="SystemRectangle"/>.</returns>
        public static SystemRectangle ToSystemRectangle(this Rectangle2D source)
        => new SystemRectangle(source.X, source.Y, source.Width, source.Height);

        /// <summary>
        /// Convers a <see cref="XnaRectangle"/> into to a <see cref="SystemRectangle"/>.
        /// </summary>
        /// <param name="source">Source <see cref="XnaRectangle"/>.</param>
        /// <returns>The <see cref="SystemRectangle"/>.</returns>
        public static SystemRectangle ToSystemRectangle(this XnaRectangle source)
        => new SystemRectangle(source.X, source.Y, source.Width, source.Height);

        // >>> TO XNA

        /// <summary>
        /// Convers a <see cref="Rectangle2D"/> into to a <see cref="XnaRectangle"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Rectangle2D"/>.</param>
        /// <returns>The <see cref="XnaRectangle"/>.</returns>
        public static XnaRectangle ToXnaRectangle(this Rectangle2D source)
        => new XnaRectangle(source.X, source.Y, source.Width, source.Height);

        /// <summary>
        /// Convers a <see cref="SystemRectangle"/> into to a <see cref="XnaRectangle"/>.
        /// </summary>
        /// <param name="source">Source <see cref="SystemRectangle"/>.</param>
        /// <returns>The <see cref="XnaRectangle"/>.</returns>
        public static XnaRectangle ToXnaRectangle(this SystemRectangle source)
        => new XnaRectangle(source.X, source.Y, source.Width, source.Height);
    }
}
