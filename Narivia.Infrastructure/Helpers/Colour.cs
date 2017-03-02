namespace Narivia.Infrastructure.Helpers
{
    public class Colour
    {
        /// <summary>
        /// Gets the alpha value.
        /// </summary>
        /// <value>The alpha value - Hardcoded to 255.</value>
        public byte Alpha { get { return 255; } }

        /// <summary>
        /// Gets or sets the red value.
        /// </summary>
        /// <value>The red value.</value>
        public byte Red { get; set; }

        /// <summary>
        /// Gets or sets the green value.
        /// </summary>
        /// <value>The green value.</value>
        public byte Green { get; set; }

        /// <summary>
        /// Gets or sets the blue value.
        /// </summary>
        /// <value>The blue value.</value>
        public byte Blue { get; set; }

        /// <summary>
        /// Converts the colour to hexadecimal.
        /// </summary>
        /// <returns>The hexadecimal code.</returns>
        public string ToHexadecimal()
        {
            string hexa = string.Format("#{0:X2}{1:X2}{2:X2}", Red, Green, Blue);

            return hexa;
        }

        /// <summary>
        /// Converts the colour to a 32 bit integer.
        /// </summary>
        /// <returns>The ARGB integer value.</returns>
        public int ToArgb()
        {
            int argb = (Alpha << 24) | (Red << 16) | (Green << 8) | Blue;

            return argb;
        }
    }
}
