using System;

using XnaColor = Microsoft.Xna.Framework.Color;
using SystemColor = System.Drawing.Color;

namespace Narivia.Graphics.Extensions
{
    /// <summary>
    /// Colour extensions.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Checks wether the current colour is similar to another.
        /// </summary>
        /// <returns><c>true</c>, if it is similar, <c>false</c> otherwise.</returns>
        /// <param name="other">Colour to compare with.</param>
        /// <param name="tolerance">Tolerance.</param>
        public static bool IsSimilarTo(this XnaColor source, XnaColor other, int tolerance)
        {
            return Math.Abs(source.R - other.R) <= tolerance &&
                   Math.Abs(source.G - other.G) <= tolerance &&
                   Math.Abs(source.B - other.B) <= tolerance;
        }

        /// <summary>
        /// Multiplies a specified colour by a factor.
        /// </summary>
        /// <returns>The multiply.</returns>
        /// <param name="factor">Factor.</param>
        public static SystemColor Multiply(this SystemColor source, float factor)
        {
            byte newA = (byte)(source.A * factor);
            byte newR = (byte)(source.R * factor);
            byte newG = (byte)(source.G * factor);
            byte newB = (byte)(source.B * factor);

            return SystemColor.FromArgb(newA, newR, newG, newB);
        }
    }
}
