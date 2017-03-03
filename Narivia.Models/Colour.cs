namespace Narivia.Models
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
    }
}
