using System;
using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    /// <summary>
    /// Border domain model.
    /// </summary>
    public class Border : IEquatable<Border>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Id { get; set; }

        /// <summary>
        /// Gets the source province identifier.
        /// </summary>
        /// <value>The source province identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string SourceProvinceId { get; set; }

        /// <summary>
        /// Gets the target province identifier.
        /// </summary>
        /// <value>The target province identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string TargetProvinceId { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="Border"/> is equal to the current <see cref="Border"/>.
        /// </summary>
        /// <param name="other">The <see cref="Border"/> to compare with the current <see cref="Border"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Border"/> is equal to the current
        /// <see cref="Border"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Border other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(SourceProvinceId, other.SourceProvinceId) &&
                   string.Equals(TargetProvinceId, other.TargetProvinceId);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Border"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Border"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Border"/>; otherwise, <c>false</c>.</returns>
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

            return Equals((Border)obj);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Border"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((SourceProvinceId != null ? SourceProvinceId.GetHashCode() : 0) * 397) ^
                       (TargetProvinceId != null ? TargetProvinceId.GetHashCode() : 0);
            }
        }
    }
}