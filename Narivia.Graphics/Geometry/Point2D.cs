using System;

namespace Narivia.Graphics.Geometry
{
    /// <summary>
    /// 2D orthogonal coordinates structure.
    /// </summary>
    public struct Point2D : IEquatable<Point2D>
    {
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
        /// Gets a value indicating whether the coordinates of this <see cref="Point2D"/> are zero.
        /// </summary>
        /// <value><c>true</c> if the coorinates are zero; otherwise, <c>false</c>.</value>
        public bool IsEmpty => X == 0 && Y == 0;

        /// <summary>
        /// Gets a <see cref="Point2D"/> with the coordinates of zero.
        /// </summary>
        /// <value>The orthogonal centre point.</value>
        public static Point2D Empty => new Point2D(0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="Point2D"/> structure.
        /// </summary>
        /// <param name="x">The X-axis coordinate.</param>
        /// <param name="y">The Y-axis coordinate.</param>
        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point2D"/> structure.
        /// </summary>
        /// <param name="size">Size.</param>
        public Point2D(Size2D size)
        {
            X = size.Width;
            Y = size.Height;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Point2D"/> is equal to the current <see cref="Point2D"/>.
        /// </summary>
        /// <param name="other">The <see cref="Point2D"/> to compare with the current <see cref="Point2D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Point2D"/> is equal to the current <see cref="Point2D"/>;
        /// otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Point2D"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Point2D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Point2D"/>; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Adds the coordinates of a <see cref="Point2D"/> to those of another <see cref="Point2D"/>,
        /// yielding a new <see cref="Point2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Point2D"/> to add.</param>
        /// <param name="other">The second <see cref="Point2D"/> to add.</param>
        /// <returns>The <see cref="Point2D"/> whose coordinates are the sum of the coordinates of <c>source</c> and <c>other</c>.</returns>
        public static Point2D operator +(Point2D source, Point2D other)
        => new Point2D(source.X + other.X,
                       source.Y + other.Y);

        /// <summary>
        /// Subtracts the coordinates of a <see cref="Point2D"/> from those of another <see cref="Point2D"/>,
        /// yielding a new <see cref="Point2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Point2D"/> to subtract.</param>
        /// <param name="other">The second <see cref="Point2D"/> to subtract.</param>
        /// <returns>The <see cref="Point2D"/> whose coordinates are the sum of the coordinates of <c>source</c> and <c>other</c>.</returns>
        public static Point2D operator -(Point2D source, Point2D other)
        => new Point2D(source.X - other.X,
                       source.Y - other.Y);

        /// <summary>
        /// Determines whether a specified instance of <see cref="Point2D"/> is equal to another specified <see cref="Point2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Point2D"/> to compare.</param>
        /// <param name="other">The second <see cref="Point2D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Point2D source, Point2D other)
        => source.Equals(other);

        /// <summary>
        /// Determines whether a specified instance of <see cref="Point2D"/> is not equal to
        /// another specified <see cref="Point2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Point2D"/> to compare.</param>
        /// <param name="other">The second <see cref="Point2D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Point2D source, Point2D other)
        => !(source == other);

        /// <summary>
        /// Serves as a hash function for a <see cref="Point2D"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
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
