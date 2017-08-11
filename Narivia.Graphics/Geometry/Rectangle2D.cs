using System;

namespace Narivia.Graphics.Geometry
{
    /// <summary>
    /// 2D rectangle structure
    /// </summary>
    public struct Rectangle2D : IEquatable<Rectangle2D>
    {
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
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

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
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

        /// <summary>
        /// Gets or sets the X-axis coordinate.
        /// </summary>
        /// <value>The X-axis coordinate.</value>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y-axis coordinate.
        /// </summary>
        /// <value>The Y-axis coordinate.</value>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }

        /// <summary>
        /// Gets a value indicating whether the location and size of this <see cref="Rectangle2D"/>
        /// are equal to zero.
        /// </summary>
        /// <value><c>true</c> if is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty => X == 0 && Y == 0 && Width == 0 && Height == 0;

        /// <summary>
        /// Gets the lowest Y-axis coordinate of the rectangle.
        /// </summary>
        /// <value>The bottom Y-axis coordinate.</value>
        public int Bottom => Y + Height;

        /// <summary>
        /// Gets the lowest X-axis coordinate of the rectangle.
        /// </summary>
        /// <value>The left X-axis coordinate.</value>
        public int Left => X;

        /// <summary>
        /// Gets the greatest X-axis coordinate of the rectangle.
        /// </summary>
        /// <value>The right X-axis coordinate.</value>
        public int Right => X + Width;

        /// <summary>
        /// Gets the greatest Y-axis coordinate of the rectangle.
        /// </summary>
        /// <value>The top Y-axis coordinate.</value>
        public int Top => Y;

        /// <summary>
        /// Gets an empty rectangle, whose location and size are equal to zero.
        /// </summary>
        /// <value>The empty rectangle.</value>
        public static Rectangle2D Empty => new Rectangle2D(0, 0, 0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle2D"/> structure.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public Rectangle2D(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle2D"/> structure.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <param name="size">Size.</param>
        public Rectangle2D(Point2D point, Size2D size)
        {
            X = point.X;
            Y = point.Y;
            Width = size.Width;
            Height = size.Height;
        }

        /// <summary>
        /// Checks whether the specified <see cref="Rectangle2D"/> contains a set of coordinates.
        /// </summary>
        /// <param name="x">The X-axis coordinate.</param>
        /// <param name="x">The Y-axis coordinate.</param>
        /// <returns><c>true</c> if the specified set of coordinates are inside the rectangle area;
        /// otherwise, <c>false</c>.</returns>
        public bool Contains(int x, int y)
        {
            if (Left <= x && Right >= x && Top <= y && Bottom >= y)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether the specified <see cref="Rectangle2D"/> contains a <see cref="Point2D"/>.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns><c>true</c> if the specified point is inside the rectangle area;
        /// otherwise, <c>false</c>.</returns>
        public bool Contains(Point2D point)
        => Contains(point.X, point.Y);


        /// <summary>
        /// Checks whether the specified <see cref="Rectangle2D"/> fully contains another <see cref="Rectangle2D"/>.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns><c>true</c> if the specified rectangle is inside the rectangle area;
        /// otherwise, <c>false</c>.</returns>
        public bool Contains(Rectangle2D rectangle)
        => Contains(rectangle.Location) &&
           Contains(rectangle.Right, rectangle.Bottom);

        /// <summary>
        /// Determines whether the specified <see cref="Rectangle2D"/> is equal to the current <see cref="Rectangle2D"/>.
        /// </summary>
        /// <param name="other">The <see cref="Rectangle2D"/> to compare with the current <see cref="Rectangle2D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Rectangle2D"/> is equal to the current
        /// <see cref="Rectangle2D"/>; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Rectangle2D"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Rectangle2D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Rectangle2D"/>; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Determines whether a specified instance of <see cref="Rectangle2D"/> is equal to
        /// another specified <see cref="Rectangle2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Rectangle2D"/> to compare.</param>
        /// <param name="other">The second <see cref="Rectangle2D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Rectangle2D source, Rectangle2D other)
        => source.Equals(other);

        /// <summary>
        /// Determines whether a specified instance of <see cref="Rectangle2D"/> is not equal
        /// to another specified <see cref="Rectangle2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Rectangle2D"/> to compare.</param>
        /// <param name="other">The second <see cref="Rectangle2D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Rectangle2D source, Rectangle2D other)
        => !(source == other);

        /// <summary>
        /// Serves as a hash function for a <see cref="Rectangle2D"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
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
