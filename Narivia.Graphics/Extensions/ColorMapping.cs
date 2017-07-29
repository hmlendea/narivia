using System.Linq;

using XnaColor = Microsoft.Xna.Framework.Color;
using SystemColor = System.Drawing.Color;

namespace Narivia.Graphics.Extensions
{
    /// <summary>
    /// Color mapping.
    /// </summary>
    public static class ColorMapping
    {
        /// <summary>
        /// Convers an XNA Color into to a System Color.
        /// </summary>
        /// <returns>The System Color.</returns>
        /// <param name="source">Source XNA Color.</param>
        public static SystemColor ToSystemColor(this XnaColor source)
        => SystemColor.FromArgb(source.A, source.R, source.G, source.B);

        /// <summary>
        /// Convers a System Color into to an XNA Color.
        /// </summary>
        /// <returns>The System Color.</returns>
        /// <param name="source">Source XNA Color.</param>
        public static XnaColor ToXnaColor(this SystemColor source)
        => new XnaColor(source.R, source.G, source.B, source.A);

        /// <summary>
        /// Convers an XNA Color array into to a System Color array.
        /// </summary>
        /// <returns>The System Color array.</returns>
        /// <param name="source">Source XNA Color array.</param>
        public static SystemColor[] ToSystemColors(this XnaColor[] source)
        => source.Select(x => x.ToSystemColor()).ToArray();

        /// <summary>
        /// Convers a System Color array into to an XNA Color array.
        /// </summary>
        /// <returns>The XNA Color array.</returns>
        /// <param name="source">Source System Color array.</param>
        public static XnaColor[] ToXnaColors(this SystemColor[] source)
        => source.Select(x => x.ToXnaColor()).ToArray();
    }
}
