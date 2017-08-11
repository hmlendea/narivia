using System;
using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    /// <summary>
    /// Relation domain model.
    /// </summary>
    public class Relation : IEquatable<Relation>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public string Id => $"{SourceFactionId}:{TargetFactionId}";

        /// <summary>
        /// Gets the source faction identifier.
        /// </summary>
        /// <value>The source faction identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string SourceFactionId { get; set; }

        /// <summary>
        /// Gets the target province identifier.
        /// </summary>
        /// <value>The target province identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string TargetFactionId { get; set; }

        /// <summary>
        /// Gets or sets the relation value.
        /// </summary>
        /// <value>The relation value.</value>
        [Range(-100, 100)]
        public int Value { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="Relation"/> is equal to the current <see cref="Relation"/>.
        /// </summary>
        /// <param name="other">The <see cref="Relation"/> to compare with the current <see cref="Relation"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Relation"/> is equal to the current
        /// <see cref="Relation"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Relation other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(SourceFactionId, other.SourceFactionId) &&
                   string.Equals(TargetFactionId, other.TargetFactionId) &&
                   Equals(Value, other.Value);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Relation"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Relation"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Relation"/>; otherwise, <c>false</c>.</returns>
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

            return Equals((Relation)obj);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Relation"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((SourceFactionId != null ? SourceFactionId.GetHashCode() : 0) * 397) ^
                       (TargetFactionId != null ? TargetFactionId.GetHashCode() : 0) ^
                       Value.GetHashCode();
            }
        }
    }
}