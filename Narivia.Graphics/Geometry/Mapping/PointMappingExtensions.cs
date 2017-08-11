using SystemPoint = System.Drawing.Point;
using XnaPoint = Microsoft.Xna.Framework.Point;

namespace Narivia.Graphics.Geometry.Mapping
{
    public static class PointMappingExtensions
    {
        // >>> TO NARIVIA

        /// <summary>
        /// Convers a <see cref="SystemPoint"/> into to a <see cref="Point2D"/>.
        /// </summary>
        /// <param name="source">Source <see cref="SystemPoint"/>.</param>
        /// <returns>The <see cref="Point2D"/>.</returns>
        public static Point2D ToPoint2D(this SystemPoint source)
        => new Point2D(source.X, source.Y);

        /// <summary>
        /// Convers a <see cref="XnaPoint"/> into to a <see cref="Point2D"/>.
        /// </summary>
        /// <param name="source">Source <see cref="XnaPoint"/>.</param>
        /// <returns>The <see cref="Point2D"/>.</returns>
        public static Point2D ToPoint2D(this XnaPoint source)
        => new Point2D(source.X, source.Y);

        // >>> TO SYSTEM

        /// <summary>
        /// Convers a <see cref="Point2D"/> into to a <see cref="SystemPoint"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Point2D"/>.</param>
        /// <returns>The <see cref="SystemPoint"/>.</returns>
        public static SystemPoint ToSystemPoint(this Point2D source)
        => new SystemPoint(source.X, source.Y);

        /// <summary>
        /// Convers a <see cref="XnaPoint"/> into to a <see cref="SystemPoint"/>.
        /// </summary>
        /// <param name="source">Source <see cref="XnaPoint"/>.</param>
        /// <returns>The <see cref="SystemPoint"/>.</returns>
        public static SystemPoint ToSystemPoint(this XnaPoint source)
        => new SystemPoint(source.X, source.Y);

        // >>> TO XNA

        /// <summary>
        /// Convers a <see cref="Point2D"/> into to a <see cref="XnaPoint"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Point2D"/>.</param>
        /// <returns>The <see cref="XnaPoint"/>.</returns>
        public static XnaPoint ToXnaPoint(this Point2D source)
        => new XnaPoint(source.X, source.Y);

        /// <summary>
        /// Convers a <see cref="SystemPoint"/> into to a <see cref="XnaPoint"/>.
        /// </summary>
        /// <param name="source">Source <see cref="SystemPoint"/>.</param>
        /// <returns>The <see cref="XnaPoint"/>.</returns>
        public static XnaPoint ToXnaPoint(this SystemPoint source)
        => new XnaPoint(source.X, source.Y);
    }
}
