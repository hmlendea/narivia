using System;

namespace Narivia.Graphics
{
    /// <summary>
    /// Colour.
    /// </summary>
    public class Colour
    {
        /// <summary>
        /// Gets the alpha value.
        /// </summary>
        /// <value>The alpha value</value>
        public byte A { get; set; }

        /// <summary>
        /// Gets or sets the red value.
        /// </summary>
        /// <value>The red value.</value>
        public byte R { get; set; }

        /// <summary>
        /// Gets or sets the green value.
        /// </summary>
        /// <value>The green value.</value>
        public byte G { get; set; }

        /// <summary>
        /// Gets or sets the blue value.
        /// </summary>
        /// <value>The blue value.</value>
        public byte B { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Colour"/> class.
        /// </summary>
        public Colour()
        {
            A = 255;
            R = 0;
            G = 0;
            B = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Colour"/> class.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        public Colour(byte r, byte g, byte b)
            : this()
        {
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Colour"/> class.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        public Colour(byte r, byte g, byte b, byte a)
            : this(r, g, b)
        {
            A = a;
        }

        public static Colour Transparent => new Colour(0, 0, 0, 0);

        public static Colour Black => new Colour(0, 0, 0, 255);
        public static Colour Blue => new Colour(0, 0, 255, 255);
        public static Colour ChromeYellow => new Colour(252, 209, 22, 255);
        public static Colour CobaltBlue => new Colour(0, 43, 127, 255);
        public static Colour DarkRed => new Colour(139, 0, 0, 255);
        public static Colour DodgerBlue => new Colour(30, 144, 255, 255);
        public static Colour Gold => new Colour(255, 215, 0, 255);
        public static Colour Green => new Colour(0, 255, 0, 255);
        public static Colour Red => new Colour(255, 0, 0, 255);
        public static Colour Vermilion => new Colour(206, 17, 38, 255);
        public static Colour White => new Colour(255, 255, 255, 255);

        /// <summary>
        /// Creates a colour from an ARGB integer.
        /// </summary>
        /// <returns>The colour.</returns>
        /// <param name="argb">ARGB integer.</param>
        public static Colour FromArgb(int argb) => ColourTranslator.FromArgb(argb);

        /// <summary>
        /// Creates a colour from RGB values.
        /// </summary>
        /// <returns>The colour.</returns>
        /// <param name="r">Red value.</param>
        /// <param name="g">Green value.</param>
        /// <param name="b">Blue value.</param>
        public static Colour FromArgb(byte r, byte g, byte b) => ColourTranslator.FromArgb(r, g, b);

        /// <summary>
        /// Creates a colour from ARGB values.
        /// </summary>
        /// <returns>The colour.</returns>
        /// <param name="a">Alpha value.</param>
        /// <param name="r">Red value.</param>
        /// <param name="g">Green value.</param>
        /// <param name="b">Blue value.</param>
        public static Colour FromArgb(byte a, byte r, byte g, byte b) => ColourTranslator.FromArgb(a, r, g, b);

        /// <summary>
        /// Creates a colour from a hexadecimal code.
        /// </summary>
        /// <returns>The colour.</returns>
        /// <param name="hexa">Hexadecimal code.</param>
        public static Colour FromHexadecimal(string hexa) => ColourTranslator.FromHexadecimal(hexa);

        /// <summary>
        /// Converts the colour to a 32 bit integer.
        /// </summary>
        /// <returns>The ARGB integer value.</returns>
        public int ToArgb() => ColourTranslator.ToArgb(this);

        /// <summary>
        /// Converts the colour to a hexadecimal string.
        /// </summary>
        /// <returns>A string representing the hexadecimal code of the colour.</returns>
        public string ToHexadecimal() => ColourTranslator.ToHexadecimal(this);

        /// <summary>
        /// Checks wether the current colour is similar to another.
        /// </summary>
        /// <returns><c>true</c>, if it is similar, <c>false</c> otherwise.</returns>
        /// <param name="colour">Colour to compare.</param>
        /// <param name="tolerance">Tolerance.</param>
        public bool IsSimilarTo(Colour colour, int tolerance)
        {
            return Math.Abs(R - colour.R) <= tolerance &&
                   Math.Abs(G - colour.G) <= tolerance &&
                   Math.Abs(B - colour.B) <= tolerance;
        }

        /// <summary>
        /// Multiplies a specified colour by a factor.
        /// </summary>
        /// <returns>The multiply.</returns>
        /// <param name="colour">Colour.</param>
        /// <param name="factor">Factor.</param>
        public static Colour Multiply(Colour colour, float factor)
        {
            byte newA = (byte)(colour.A * factor);
            byte newR = (byte)(colour.R * factor);
            byte newG = (byte)(colour.G * factor);
            byte newB = (byte)(colour.B * factor);

            return new Colour(newR, newG, newB, newA);
        }

        /// <summary>
        /// Multiplies a specified colour by a factor.
        /// </summary>
        /// <returns>The multiply.</returns>
        /// <param name="colour">Colour.</param>
        /// <param name="factor">Factor.</param>
        public static Colour operator *(Colour colour, float factor)
        {
            return Multiply(colour, factor);
        }
    }
}
