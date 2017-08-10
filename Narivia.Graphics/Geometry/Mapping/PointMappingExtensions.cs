using SystemPoint = System.Drawing.Point;
using XnaPoint = Microsoft.Xna.Framework.Point;

namespace Narivia.Graphics.Geometry.Mapping
{
    public static class PointMappingExtensions
    {
        // >>> TO NARIVIA

        public static Point2D ToPoint2D(this SystemPoint source)
        => new Point2D(source.X, source.Y);

        public static Point2D ToPoint2D(this XnaPoint source)
        => new Point2D(source.X, source.Y);
       
        // >>> TO SYSTEM

        public static SystemPoint ToSystemPoint(this Point2D source)
        => new SystemPoint(source.X, source.Y);

        public static SystemPoint ToSystemPoint(this XnaPoint source)
        => new SystemPoint(source.X, source.Y);
        
        // >>> TO XNA

        public static XnaPoint ToXnaPoint(this Point2D source)
        => new XnaPoint(source.X, source.Y);

        public static XnaPoint ToXnaPoint(this SystemPoint source)
        => new XnaPoint(source.X, source.Y);
    }
}
