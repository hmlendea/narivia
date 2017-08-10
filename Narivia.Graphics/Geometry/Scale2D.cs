using System;

namespace Narivia.Graphics.Geometry
{
    public struct Scale2D : IEquatable<Scale2D>
    {
        public float Horizontal { get; set; }

        public float Vertical { get; set; }

        public bool IsEmpty => Horizontal == 0 && Vertical == 0;

        public static Scale2D Empty => new Scale2D(0, 0);

        public static Scale2D One => new Scale2D(1, 1);

        public Scale2D(float horizontal, float vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }

        public Scale2D(Point2D point)
        {
            Horizontal = point.X;
            Vertical = point.Y;
        }

        public Scale2D(Size2D size)
        {
            Horizontal = size.Width;
            Vertical = size.Height;
        }

        public bool Equals(Scale2D other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Horizontal, other.Horizontal) &&
                   Equals(Vertical, other.Vertical);
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

            return Equals((Scale2D)obj);
        }

        public static Scale2D operator +(Scale2D source, Scale2D other)
        => new Scale2D(source.Horizontal + other.Horizontal,
                       source.Vertical + other.Vertical);

        public static Scale2D operator -(Scale2D source, Scale2D other)
        => new Scale2D(source.Horizontal - other.Horizontal,
                       source.Vertical - other.Vertical);

        public static bool operator ==(Scale2D source, Scale2D other)
        => source.Equals(other);

        public static bool operator !=(Scale2D source, Scale2D other)
        => !(source == other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (Horizontal.GetHashCode() * 397) ^
                        Vertical.GetHashCode();
            }
        }
    }
}
