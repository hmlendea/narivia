using Microsoft.Xna.Framework;

using Narivia.Infrastructure.Helpers;

namespace Narivia.Infrastructure.Extensions
{
    public static class ColourExtensions
    {
        public static Color ToXnaColor(this Colour source)
        {
            return new Color(source.R, source.G, source.B, source.A);
        }

        public static Colour ToNariviaColour(this Color source)
        {
            return new Colour(source.R, source.G, source.B, source.A);
        }
    }
}
