using SystemRectangle = System.Drawing.Rectangle;
using XnaRectangle = Microsoft.Xna.Framework.Rectangle;

namespace Narivia.Graphics.Geometry.Mapping
{
    public static class RectangleMappingExtensions
    {
        // >>> TO NARIVIA

        public static Rectangle2D ToRectangle2D(this SystemRectangle source)
        => new Rectangle2D(source.X, source.Y, source.Width, source.Height);
        
        public static Rectangle2D ToRectangle2D(this XnaRectangle source)
        => new Rectangle2D(source.X, source.Y, source.Width, source.Height);

        // >>> TO SYSTEM
        public static SystemRectangle ToSystemRectangle(this Rectangle2D source)
        => new SystemRectangle(source.X, source.Y, source.Width, source.Height);

        public static SystemRectangle ToSystemRectangle(this XnaRectangle source)
        => new SystemRectangle(source.X, source.Y, source.Width, source.Height);

        // >>> TO XNA

        public static XnaRectangle ToXnaRectangle(this Rectangle2D source)
        => new XnaRectangle(source.X, source.Y, source.Width, source.Height);

        public static XnaRectangle ToXnaRectangle(this SystemRectangle source)
        => new XnaRectangle(source.X, source.Y, source.Width, source.Height);
    }
}
