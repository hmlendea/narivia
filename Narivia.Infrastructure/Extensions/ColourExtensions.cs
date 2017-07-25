using System.Linq;

using Microsoft.Xna.Framework;

using Narivia.Infrastructure.Helpers;

namespace Narivia.Infrastructure.Extensions
{
    public static class ColourExtensions
    {
        public static Color ToXnaColor(this Colour source)
        {
            if(source == null)
            {
                return Color.Transparent;
            }

            return new Color(source.R, source.G, source.B, source.A);
        }

        public static Colour ToNariviaColour(this Color source)
        {
            if (source == null)
            {
                return null;
            }

            return new Colour(source.R, source.G, source.B, source.A);
        }

        public static Color[] ToXnaColors(this Colour[] source)
        {
            return source.Select(x => x.ToXnaColor()).ToArray();
        }

        public static Colour[] ToNariviaColours(this Color[] source)
        {
            return source.Select(x => x.ToNariviaColour()).ToArray();
        }
    }
}
