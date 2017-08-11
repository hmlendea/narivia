using System;

namespace Narivia.Graphics.Geometry
{
    /// <summary>
    /// 2D size structure.
    /// </summary>
    public struct Size2D : IEquatable<Size2D>
    {
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
        /// Gets a value indicating whether this <see cref="Size2D"/> is zero.
        /// </summary>
        /// <value><c>true</c> if is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty => Width == 0 && Height == 0;

        /// <summary>
        /// Gets the size of zero.
        /// </summary>
        /// <value>The size of zero.</value>
        public static Size2D Empty => new Size2D(0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="Size2D"/> structure.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public Size2D(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size2D"/> structure.
        /// </summary>
        /// <param name="point">Point.</param>
        public Size2D(Point2D point)
        {
            Width = point.X;
            Height = point.Y;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Size2D"/> is equal to the current <see cref="Size2D"/>.
        /// </summary>
        /// <param name="other">The <see cref="Size2D"/> to compare with the current <see cref="Size2D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Size2D"/> is equal to the current <see cref="Size2D"/>;
        /// otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Size2D"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Size2D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current <see cref="Size2D"/>;
        /// otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Adds the values of a <see cref="Size2D"/> to those of another <see cref="Size2D"/>,
        /// yielding a new <see cref="Size2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Size2D"/> to add.</param>
        /// <param name="other">The second <see cref="Size2D"/> to add.</param>
        /// <returns>The <see cref="Size2D"/> that is the sum of the values of <c>source</c> and <c>other</c>.</returns>
        public static Size2D operator +(Size2D source, Size2D other)
        => new Size2D(source.Width + other.Width,
                      source.Height + other.Height);

        /// <summary>
        /// Subtracts the values of a <see cref="Size2D"/> from those of another <see cref="Size2D"/>,
        /// yielding a new <see cref="Size2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Size2D"/> to subtract.</param>
        /// <param name="other">The second <see cref="Size2D"/> to subtract.</param>
        /// <returns>The <see cref="Size2D"/> that is the subtraction of the values of <c>other</c> from <c>source</c>.</returns>
        public static Size2D operator -(Size2D source, Size2D other)
        => new Size2D(source.Width - other.Width,
                      source.Height - other.Height);

        /// <summary>
        /// Determines whether a specified instance of <see cref="Size2D"/> is equal to
        /// another specified <see cref="Size2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Size2D"/> to compare.</param>
        /// <param name="other">The second <see cref="Size2D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Size2D source, Size2D other)
        => source.Equals(other);

        /// <summary>
        /// Determines whether a specified instance of <see cref="Size2D"/> is not equal to
        /// another specified <see cref="Size2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Size2D"/> to compare.</param>
        /// <param name="other">The second <see cref="Size2D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Size2D source, Size2D other)
        => !(source == other);

        /// <summary>
        /// Serves as a hash function for a <see cref="Size2D"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
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
