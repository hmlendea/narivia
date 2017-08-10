using Narivia.Graphics.Geometry;

using SystemPoint = System.Drawing.Point;
using SystemRectangle = System.Drawing.Rectangle;
using SystemSize = System.Drawing.Size;

using XnaPoint = Microsoft.Xna.Framework.Point;
using XnaRectangle = Microsoft.Xna.Framework.Rectangle;

namespace Narivia.Graphics.Geometry.Mapping
{
    public static class GeometryMappingExtensions
    {
        // >>> TO NARIVIA

        public static Point2D ToPoint2D(this SystemPoint source)
        => new Point2D(source.X, source.Y);

        public static Point2D ToPoint2D(this XnaPoint source)
        => new Point2D(source.X, source.Y);

        public static Rectangle2D ToRectangle2D(this SystemRectangle source)
        => new Rectangle2D(source.X, source.Y, source.Width, source.Height);
        
        public static Rectangle2D ToRectangle2D(this XnaRectangle source)
        => new Rectangle2D(source.X, source.Y, source.Width, source.Height);

        public static Size2D ToSize2D(this SystemSize source)
        => new Size2D(source.Width, source.Height);

        public static Size2D ToSize2D(this XnaPoint source)
        => new Size2D(source.X, source.Y);

        // >>> TO SYSTEM

        public static SystemPoint ToSystemPoint(this Point2D source)
        => new SystemPoint(source.X, source.Y);

        public static SystemPoint ToSystemPoint(this XnaPoint source)
        => new SystemPoint(source.X, source.Y);

        public static SystemRectangle ToSystemRectangle(this Rectangle2D source)
        => new SystemRectangle(source.X, source.Y, source.Width, source.Height);

        public static SystemRectangle ToSystemRectangle(this XnaRectangle source)
        => new SystemRectangle(source.X, source.Y, source.Width, source.Height);

        public static SystemSize ToSystemSize(this Size2D source)
        => new SystemSize(source.Width, source.Height);

        public static SystemSize ToSystemSize(this XnaPoint source)
        => new SystemSize(source.X, source.Y);

        // >>> TO XNA

        public static XnaPoint ToXnaPoint(this Point2D source)
        => new XnaPoint(source.X, source.Y);

        public static XnaPoint ToXnaPoint(this SystemPoint source)
        => new XnaPoint(source.X, source.Y);

        public static XnaRectangle ToXnaRectangle(this Rectangle2D source)
        => new XnaRectangle(source.X, source.Y, source.Width, source.Height);

        public static XnaRectangle ToXnaRectangle(this SystemRectangle source)
        => new XnaRectangle(source.X, source.Y, source.Width, source.Height);

        public static XnaPoint ToXnaPoint(this Size2D source)
        => new XnaPoint(source.Width, source.Height);

        public static XnaPoint ToXnaPoint(this SystemSize source)
        => new XnaPoint(source.Width, source.Height);
    }
}
