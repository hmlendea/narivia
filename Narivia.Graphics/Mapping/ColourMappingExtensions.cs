using System.Linq;

using SystemColor = System.Drawing.Color;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Narivia.Graphics.Mapping
{
    /// <summary>
    /// Colour mapping extensions.
    /// </summary>
    public static class ColourMappingExtensions
    {
        /// <summary>
        /// Convers a <see cref="SystemColor"/> into to a <see cref="Colour"/>.
        /// </summary>
        /// <returns>The <see cref="Colour"/>.</returns>
        /// <param name="source">Source <see cref="SystemColor"/>.</param>
        public static Colour ToColour(this SystemColor source)
        => Colour.FromArgb(source.A, source.R, source.G, source.B);

        /// <summary>
        /// Convers a <see cref="XnaColor"/> into to a <see cref="Colour"/>.
        /// </summary>
        /// <returns>The <see cref="Colour"/>.</returns>
        /// <param name="source">Source <see cref="XnaColor"/>.</param>
        public static Colour ToColour(this XnaColor source)
        => Colour.FromArgb(source.A, source.R, source.G, source.B);

        /// <summary>
        /// Convers a <see cref="Colour"/> into to a <see cref="SystemColor"/>.
        /// </summary>
        /// <returns>The <see cref="SystemColor"/>.</returns>
        /// <param name="source">Source <see cref="Colour"/>.</param>
        public static SystemColor ToSystemColor(this Colour source)
        => SystemColor.FromArgb(source.A, source.R, source.G, source.B);

        /// <summary>
        /// Convers a <see cref="XnaColor"/> into to a <see cref="SystemColor"/>.
        /// </summary>
        /// <returns>The <see cref="SystemColor"/>.</returns>
        /// <param name="source">Source <see cref="XnaColor"/>.</param>
        public static SystemColor ToSystemColor(this XnaColor source)
        => SystemColor.FromArgb(source.A, source.R, source.G, source.B);

        /// <summary>
        /// Convers a <see cref="Colour"/> into to a <see cref="XnaColor"/>.
        /// </summary>
        /// <returns>The <see cref="XnaColor"/>.</returns>
        /// <param name="source">Source <see cref="Colour"/>.</param>
        public static XnaColor ToXnaColor(this Colour source)
        => new XnaColor(source.R, source.G, source.B, source.A);

        /// <summary>
        /// Convers a <see cref="SystemColor"/> into to a <see cref="SystemColor"/>.
        /// </summary>
        /// <returns>The <see cref="SystemColor"/>.</returns>
        /// <param name="source">Source <see cref="SystemColor"/>.</param>
        public static XnaColor ToXnaColor(this SystemColor source)
        => new XnaColor(source.R, source.G, source.B, source.A);

        /// <summary>
        /// Convers an array of <see cref="Colour"/> into to an array of <see cref="SystemColor"/>.
        /// </summary>
        /// <returns>The <see cref="SystemColor"/> array.</returns>
        /// <param name="source">Source <see cref="Colour"/> array.</param>
        public static Colour[] ToColours(this SystemColor[] source)
        => source.Select(x => x.ToColour()).ToArray();

        /// <summary>
        /// Convers an array of <see cref="XnaColor"/> into to an array of <see cref="Colour"/>.
        /// </summary>
        /// <returns>The <see cref="Colour"/> array.</returns>
        /// <param name="source">Source <see cref="XnaColor"/> array.</param>
        public static Colour[] ToColours(this XnaColor[] source)
        => source.Select(x => x.ToColour()).ToArray();

        /// <summary>
        /// Convers an array of <see cref="XnaColor"/> into to an array of <see cref="SystemColor"/>.
        /// </summary>
        /// <returns>The <see cref="SystemColor"/> array.</returns>
        /// <param name="source">Source <see cref="XnaColor"/> array.</param>
        public static SystemColor[] ToSystemColors(this XnaColor[] source)
        => source.Select(x => x.ToSystemColor()).ToArray();

        /// <summary>
        /// Convers an array of <see cref="Colour"/> into to an array of <see cref="SystemColor"/>.
        /// </summary>
        /// <returns>The <see cref="SystemColor"/> array.</returns>
        /// <param name="source">Source <see cref="Colour"/> array.</param>
        public static SystemColor[] ToSystemColors(this Colour[] source)
        => source.Select(x => x.ToSystemColor()).ToArray();

        /// <summary>
        /// Convers an array of <see cref="Colour"/> into to an array of <see cref="XnaColor"/>.
        /// </summary>
        /// <returns>The <see cref="XnaColor"/> array.</returns>
        /// <param name="source">Source <see cref="Colour"/> array.</param>
        public static XnaColor[] ToXnaColors(this Colour[] source)
        => source.Select(x => x.ToXnaColor()).ToArray();

        /// <summary>
        /// Convers an array of <see cref="SystemColor"/> into to an array of <see cref="XnaColor"/>.
        /// </summary>
        /// <returns>The <see cref="XnaColor"/> array.</returns>
        /// <param name="source">Source <see cref="SystemColor"/> array.</param>
        public static XnaColor[] ToXnaColors(this SystemColor[] source)
        => source.Select(x => x.ToXnaColor()).ToArray();
    }
}
