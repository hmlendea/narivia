namespace Narivia.Infrastructure.Helpers
{
    public class Colour
    {
        // Hardcoded to fully opaque colours
        public byte Alpha { get { return 255; } }

        public byte Red { get; set; }

        public byte Green { get; set; }

        public byte Blue { get; set; }

        public string ToHexadecimal()
        {
            string hexa = string.Format("#{0:X2}{1:X2}{2:X2}", Red, Green, Blue);

            return hexa;
        }

        public int ToArgb()
        {
            int argb = (Alpha << 24) | (Red << 16) | (Green << 8) | Blue;

            return argb;
        }
    }
}
