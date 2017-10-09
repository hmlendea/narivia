using System;
using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    /// <summary>
    /// Army domain model.
    /// </summary>
    public class Army : IEquatable<Army>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the faction identifier.
        /// </summary>
        /// <value>The faction identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string FactionId { get; set; }

        /// <summary>
        /// Gets or sets the unit identifier.
        /// </summary>
        /// <value>The unit identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string UnitId { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        [Range(0, int.MaxValue)]
        public int Size { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="Army"/> is equal to the current <see cref="Army"/>.
        /// </summary>
        /// <param name="other">The <see cref="Army"/> to compare with the current <see cref="Army"/>.</param>
        /// <returns><c>true</c> if the specified <see cref=Army"/> is equal to the current
        /// <see cref="Army"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Army other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(FactionId, other.FactionId) &&
                   string.Equals(UnitId, other.UnitId) &&
                   Equals(Size, other.Size);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Army"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Army"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Army"/>; otherwise, <c>false</c>.</returns>
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

            return Equals((Army)obj);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Army"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((FactionId != null ? FactionId.GetHashCode() : 0) * 397) ^
                       (UnitId != null ? UnitId.GetHashCode() : 0) ^
                       Size.GetHashCode();
            }
        }
    }
}
