using XnaVector2 = Microsoft.Xna.Framework.Vector2;

namespace Narivia.Graphics.Geometry.Mapping
{
    public static class ScaleMappingExtensions
    {
        // >>> TO NARIVIA

        public static Scale2D ToScale2D(this XnaVector2 source)
        => new Scale2D(source.X, source.Y);
        
        // >>> TO XNA

        public static XnaVector2 ToXnaVector2(this Scale2D source)
        => new XnaVector2(source.Horizontal, source.Vertical);
    }
}
