using System;

namespace Narivia.Graphics.Geometry
{
    /// <summary>
    /// 2D scale strucure.
    /// </summary>
    public struct Scale2D : IEquatable<Scale2D>
    {
        /// <summary>
        /// Gets or sets the horizontal scale.
        /// </summary>
        /// <value>The horizontal scale.</value>
        public float Horizontal { get; set; }

        /// <summary>
        /// Gets or sets the vertical scale.
        /// </summary>
        /// <value>The vertical scale.</value>
        public float Vertical { get; set; }

        /// <summary>
        /// Gets a value indicating whether the values of this <see cref="Scale2D"/> are zero.
        /// </summary>
        /// <value><c>true</c> if is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty => Horizontal == 0 && Vertical == 0;

        /// <summary>
        /// Gets a scale of zero.
        /// </summary>
        /// <value>The scale of zero.</value>
        public static Scale2D Empty => new Scale2D(0, 0);

        /// <summary>
        /// Gets a scale of one.
        /// </summary>
        /// <value>The scale of one.</value>
        public static Scale2D One => new Scale2D(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="Scale2D"/> structure.
        /// </summary>
        /// <param name="horizontal">Horizontal scale.</param>
        /// <param name="vertical">Vertical scale.</param>
        public Scale2D(float horizontal, float vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scale2D"/> structure.
        /// </summary>
        /// <param name="point">Point.</param>
        public Scale2D(Point2D point)
        {
            Horizontal = point.X;
            Vertical = point.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scale2D"/> structure.
        /// </summary>
        /// <param name="size">Size.</param>
        public Scale2D(Size2D size)
        {
            Horizontal = size.Width;
            Vertical = size.Height;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Scale2D"/> is equal to the current <see cref="Scale2D"/>.
        /// </summary>
        /// <param name="other">The <see cref="Scale2D"/> to compare with the current <see cref="Scale2D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Scale2D"/> is equal to the current
        /// <see cref="Scale2D"/>; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Scale2D"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Scale2D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Scale2D"/>; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Adds the values of a <see cref="Scale2D"/> to those of another <see cref="Scale2D"/>,
        /// yielding a new <see cref="Scale2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Scale2D"/> to add.</param>
        /// <param name="other">The second <see cref="Scale2D"/> to add.</param>
        /// <returns>The <see cref="Scale2D"/> that is the sum of the values of <c>source</c> and <c>other</c>.</returns>
        public static Scale2D operator +(Scale2D source, Scale2D other)
        => new Scale2D(source.Horizontal + other.Horizontal,
                       source.Vertical + other.Vertical);

        /// <summary>
        /// Subtracts the values of a <see cref="Scale2D"/> from those of another <see cref="Scale2D"/>,
        /// yielding a new <see cref="Scale2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Scale2D"/> to subtract.</param>
        /// <param name="other">The second <see cref="Scale2D"/> to subtract.</param>
        /// <returns>The <see cref="Scale2D"/> that is the subtraction of the values of <c>other</c> from <c>source</c>.</returns>
        public static Scale2D operator -(Scale2D source, Scale2D other)
        => new Scale2D(source.Horizontal - other.Horizontal,
                       source.Vertical - other.Vertical);

        /// <summary>
        /// Determines whether a specified instance of <see cref="Scale2D"/> is equal to
        /// another specified <see cref="Scale2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Scale2D"/> to compare.</param>
        /// <param name="other">The second <see cref="Scale2D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Scale2D source, Scale2D other)
        => source.Equals(other);

        /// <summary>
        /// Determines whether a specified instance of <see cref="Scale2D"/> is not equal to
        /// another specified <see cref="Scale2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Scale2D"/> to compare.</param>
        /// <param name="other">The second <see cref="Scale2D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Scale2D source, Scale2D other)
        => !(source == other);

        /// <summary>
        /// Serves as a hash function for a <see cref="Scale2D"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
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
