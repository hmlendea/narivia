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
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public string Id => $"{FactionId}:{UnitId}";

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
