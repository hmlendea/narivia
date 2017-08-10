using System;

namespace Narivia.Graphics.Geometry
{
    public struct Point2D : IEquatable<Point2D>
    {
        public int X { get; set; }

        public int Y { get; set; }

        public bool IsEmpty => X == 0 && Y == 0;

        public static Point2D Empty => new Point2D(0, 0);

        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point2D(Size2D size)
        {
            X = size.Width;
            Y = size.Height;
        }

        public bool Equals(Point2D other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(X, other.X) &&
                   Equals(Y, other.Y);
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

            return Equals((Point2D)obj);
        }

        public static Point2D operator +(Point2D source, Point2D other)
        => new Point2D(source.X + other.X,
                       source.Y + other.Y);

        public static Point2D operator -(Point2D source, Point2D other)
        => new Point2D(source.X - other.X,
                       source.Y - other.Y);

        public static bool operator ==(Point2D source, Point2D other)
        => source.Equals(other);
        
        public static bool operator !=(Point2D source, Point2D other)
        => !(source == other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^
                        Y.GetHashCode();
            }
        }
    }
}
