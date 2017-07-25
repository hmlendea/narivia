using System;

namespace Narivia.Infrastructure.Helpers
{
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

        public Colour()
        {
            A = 255;
            R = 0;
            G = 0;
            B = 0;
        }

        public Colour(byte r, byte g, byte b)
            : this()
        {
            R = r;
            G = g;
            B = b;
        }

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

        public bool IsSimilarTo(Colour colour, int tolerance)
        {
            return Math.Abs(R - colour.R) <= tolerance &&
                   Math.Abs(G - colour.G) <= tolerance &&
                   Math.Abs(B - colour.B) <= tolerance;
        }

        public static Colour Multiply(Colour value, float scale)
        {
            byte newA = (byte)(value.A * scale);
            byte newR = (byte)(value.R * scale);
            byte newG = (byte)(value.G * scale);
            byte newB = (byte)(value.B * scale);

            return new Colour(newR, newG, newB, newA);
        }

        public static Colour operator *(Colour value, float scale)
        {
            return Multiply(value, scale);
        }
    }
}
