using SystemSize = System.Drawing.Size;
using XnaPoint = Microsoft.Xna.Framework.Point;

namespace Narivia.Graphics.Geometry.Mapping
{
    public static class SizeMappingExtensions
    {
        // >>> TO NARIVIA

        public static Size2D ToPoint2D(this SystemSize source)
        => new Size2D(source.Width, source.Height);

        public static Size2D ToPoint2D(this XnaPoint source)
        => new Size2D(source.X, source.Y);

        // >>> TO SYSTEM

        public static SystemSize ToSystemPoint(this Size2D source)
        => new SystemSize(source.Width, source.Height);

        public static SystemSize ToSystemPoint(this XnaPoint source)
        => new SystemSize(source.X, source.Y);

        // >>> TO XNA

        public static XnaPoint ToXnaPoint(this Size2D source)
        => new XnaPoint(source.Width, source.Height);

        public static XnaPoint ToXnaPoint(this SystemSize source)
        => new XnaPoint(source.Width, source.Height);
    }
}
