using System;

namespace Narivia.Infrastructure.Helpers
{
    public static class ColourTranslator
    {
        /// <summary>
        /// Converts the colour to hexadecimal.
        /// </summary>
        /// <returns>The hexadecimal code.</returns>
        public static string ToHexadecimal(Colour colour)
        {
            string hexa = string.Format("#{0:X2}{1:X2}{2:X2}", colour.R, colour.G, colour.B);

            return hexa;
        }

        /// <summary>
        /// Creates a colour from the hexadecimal code.
        /// </summary>
        /// <returns>The colour.</returns>
        /// <param name="hexa">Hexadecimal code.</param>
        public static Colour FromHexadecimal(string hexa)
        {
            Colour colour = new Colour();

            if (hexa[0] == '#')
            {
                hexa = hexa.Remove(0, 1);
            }

            if (hexa.Length == 3)
            {
                colour.R = Convert.ToByte(hexa[0] + "" + hexa[0], 16);
                colour.G = Convert.ToByte(hexa[1] + "" + hexa[1], 16);
                colour.B = Convert.ToByte(hexa[2] + "" + hexa[2], 16);
            }
            else if (hexa.Length == 6)
            {
                colour.R = Convert.ToByte(hexa[0] + "" + hexa[1], 16);
                colour.G = Convert.ToByte(hexa[2] + "" + hexa[3], 16);
                colour.B = Convert.ToByte(hexa[4] + "" + hexa[5], 16);
            }
            else
            {
                throw new ArgumentException("Hex colour '" + hexa + "' is invalid.");
            }

            return colour;
        }

        /// <summary>
        /// Converts the colour to a 32 bit integer.
        /// </summary>
        /// <returns>The ARGB integer value.</returns>
        public static int ToArgb(Colour colour)
        {
            int argb = (colour.A << 24) |
                       (colour.R << 16) |
                       (colour.G << 8) |
                       colour.B;

            return argb;
        }

        /// <summary>
        /// Creates a colour from the ARGB integer.
        /// </summary>
        /// <returns>The colour.</returns>
        /// <param name="argb">ARGB integer.</param>
        public static Colour FromArgb(int argb)
        {
            // TODO: Implement this (Colour.FromArgb)
            throw new NotImplementedException();
        }

        public static Colour FromArgb(byte r, byte g, byte b)
        {
            return new Colour(r, g, b);
        }

        public static Colour FromArgb(byte a, byte r, byte g, byte b)
        {
            return new Colour(r, g, b, a);
        }
    }
}
