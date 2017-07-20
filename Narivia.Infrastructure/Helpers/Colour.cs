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
            : this (r, g, b)
        {
            A = a;
        }
        
        public static Colour Transparent => new Colour(0, 0, 0, 0);

        public static Colour Black => new Colour(0, 0, 0, 255);
        public static Colour Blue => new Colour(0, 0, 255, 255);
        public static Colour DarkRed => new Colour(139, 0, 0, 255);
        public static Colour DodgerBlue => new Colour(30, 144, 255, 255);
        public static Colour Gold => new Colour(255, 215, 0, 255);
        public static Colour White => new Colour(255, 255, 255, 255);

        public static Colour FromArgb(byte r, byte g, byte b) => ColourTranslator.FromArgb(r, g, b);

        public static Colour FromArgb(byte a, byte r, byte g, byte b) => ColourTranslator.FromArgb(a, r, g, b);

        /// <summary>
        /// Converts the colour to a 32 bit integer.
        /// </summary>
        /// <returns>The ARGB integer value.</returns>
        public int ToArgb() => ColourTranslator.ToArgb(this);

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
