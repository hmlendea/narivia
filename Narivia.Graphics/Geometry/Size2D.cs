using System;

namespace Narivia.Graphics.Geometry
{
    public struct Size2D : IEquatable<Size2D>
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public bool IsEmpty => Width == 0 && Height == 0;

        public static Size2D Empty => new Size2D(0, 0);

        public Size2D(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Size2D(Point2D point)
        {
            Width = point.X;
            Height = point.Y;
        }

        public bool Equals(Size2D other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Width, other.Width) &&
                   Equals(Height, other.Height);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Size2D)obj);
        }

        public static Size2D operator +(Size2D source, Size2D other)
        => new Size2D(source.Width + other.Width,
                      source.Height + other.Height);

        public static Size2D operator -(Size2D source, Size2D other)
        => new Size2D(source.Width - other.Width,
                      source.Height - other.Height);

        public static bool operator ==(Size2D source, Size2D other)
        => source.Equals(other);

        public static bool operator !=(Size2D source, Size2D other)
        => !(source == other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (Width.GetHashCode() * 397) ^
                        Height.GetHashCode();
            }
        }
    }
}
