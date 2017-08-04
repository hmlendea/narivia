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
        /// Gets the target region identifier.
        /// </summary>
        /// <value>The target region identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string TargetFactionId { get; set; }

        /// <summary>
        /// Gets or sets the relation value.
        /// </summary>
        /// <value>The relation value.</value>
        [Range(-100, 100)]
        public int Value { get; set; }

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