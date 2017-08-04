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
        public string Id => $"{SourceRegionId}:{TargetRegionId}";

        /// <summary>
        /// Gets the source region identifier.
        /// </summary>
        /// <value>The source region identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string SourceRegionId { get; set; }

        /// <summary>
        /// Gets the target region identifier.
        /// </summary>
        /// <value>The target region identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string TargetRegionId { get; set; }

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

            return string.Equals(SourceRegionId, other.SourceRegionId) &&
                   string.Equals(TargetRegionId, other.TargetRegionId);
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

            return Equals((Border)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((SourceRegionId != null ? SourceRegionId.GetHashCode() : 0) * 397) ^
                       (TargetRegionId != null ? TargetRegionId.GetHashCode() : 0);
            }
        }
    }
}