using Narivia.Models;

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
            string hexa = string.Format("#{0:X2}{1:X2}{2:X2}", colour.Red, colour.Green, colour.Blue);

            return hexa;
        }

        public static Colour FromHexadecimal(string hexa)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Converts the colour to a 32 bit integer.
        /// </summary>
        /// <returns>The ARGB integer value.</returns>
        public static int ToArgb(Colour colour)
        {
            int argb = (colour.Alpha << 24) |
                       (colour.Red << 16) |
                       (colour.Green << 8) |
                       colour.Blue;

            return argb;
        }

        public static Colour FromArgb(int argb)
        {
            throw new System.NotImplementedException();
        }
    }
}
