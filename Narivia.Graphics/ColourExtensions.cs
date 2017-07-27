using System.Linq;

using Microsoft.Xna.Framework;

namespace Narivia.Graphics
{
    /// <summary>
    /// Colour extensions.
    /// </summary>
    public static class ColourExtensions
    {
        /// <summary>
        /// Convers a Narivia Colour into to an XNA Color.
        /// </summary>
        /// <returns>The XNA Color.</returns>
        /// <param name="source">Source Narivia Colour.</param>
        public static Color ToXnaColor(this Colour source)
        {
            if (source == null)
            {
                return Color.Transparent;
            }

            return new Color(source.R, source.G, source.B, source.A);
        }

        /// <summary>
        /// Convers an XNA Color into to a Narivia Colour.
        /// </summary>
        /// <returns>The Narivia Colour.</returns>
        /// <param name="source">Source XNA Color.</param>
        public static Colour ToNariviaColour(this Color source) => new Colour(source.R, source.G, source.B, source.A);

        /// <summary>
        /// Convers a Narivia Colour array into to an XNA Color array.
        /// </summary>
        /// <returns>The XNA Color array.</returns>
        /// <param name="source">Source Narivia Colour array.</param>
        public static Color[] ToXnaColors(this Colour[] source)
        {
            return source.Select(x => x.ToXnaColor()).ToArray();
        }

        /// <summary>
        /// Convers an XNA Color array into to a Narivia Colour array.
        /// </summary>
        /// <returns>The Narivia Colour array.</returns>
        /// <param name="source">Source XNA Color array.</param>
        public static Colour[] ToNariviaColours(this Color[] source)
        {
            return source.Select(x => x.ToNariviaColour()).ToArray();
        }
    }
}
