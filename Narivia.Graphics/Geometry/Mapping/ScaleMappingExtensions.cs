using XnaVector2 = Microsoft.Xna.Framework.Vector2;

namespace Narivia.Graphics.Geometry.Mapping
{
    public static class ScaleMappingExtensions
    {
        // >>> TO NARIVIA

        /// <summary>
        /// Convers a <see cref="XnaVector2"/> into to a <see cref="Scale2D"/>.
        /// </summary>
        /// <param name="source">Source <see cref="XnaVector2"/>.</param>
        /// <returns>The <see cref="Scale2D"/>.</returns>
        public static Scale2D ToScale2D(this XnaVector2 source)
        => new Scale2D(source.X, source.Y);

        // >>> TO XNA

        /// <summary>
        /// Convers a <see cref="Scale2D"/> into to a <see cref="XnaVector2"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Scale2D"/>.</param>
        /// <returns>The <see cref="XnaVector2"/>.</returns>
        public static XnaVector2 ToXnaVector2(this Scale2D source)
        => new XnaVector2(source.Horizontal, source.Vertical);
    }
}
