using System;

namespace Narivia.Graphics.Geometry
{
    public struct Rectangle2D : IEquatable<Rectangle2D>
    {
        public Point2D Location
        {
            get
            {
                return new Point2D(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Size2D Size
        {
            get
            {
                return new Size2D(Width, Height);
            }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool IsEmpty => X == 0 && Y == 0 && Width == 0 && Height == 0;

        public int Bottom => Y + Height;

        public int Left => X;

        public int Right => X + Width;

        public int Top => Y;

        public static Rectangle2D Empty => new Rectangle2D(0, 0, 0, 0);

        public Rectangle2D(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle2D(Point2D point, Size2D size)
        {
            X = point.X;
            Y = point.Y;
            Width = size.Width;
            Height = size.Height;
        }

        public bool Contains(int x, int y)
        {
            if (Left <= x && Right >= x && Top <= y && Bottom >= y)
            {
                return true;
            }

            return false;
        }

        public bool Contains(Point2D point)
        => Contains(point.X, point.Y);

        public bool Contains(Rectangle2D rectangle)
        => Contains(rectangle.Location) &&
           Contains(rectangle.Right, rectangle.Bottom);

        public bool Equals(Rectangle2D other)
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
                   Equals(Y, other.Y) &&
                   Equals(Width, other.Width) &&
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

            return Equals((Rectangle2D)obj);
        }

        public static bool operator ==(Rectangle2D source, Rectangle2D other)
        => source.Equals(other);

        public static bool operator !=(Rectangle2D source, Rectangle2D other)
        => !(source == other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^
                        Y.GetHashCode() ^
                        Width.GetHashCode() ^
                        Height.GetHashCode();
            }
        }
    }
}
