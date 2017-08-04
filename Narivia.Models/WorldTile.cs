using System;
using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    public class WorldTile : IEquatable<WorldTile>
    {
        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>The region identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string RegionId { get; set; }

        /// <summary>
        /// Gets the biome identifier.
        /// </summary>
        /// <value>The biome identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string BiomeId { get; set; }

        public bool Equals(WorldTile other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(RegionId, other.RegionId) &&
                   string.Equals(BiomeId, other.BiomeId);
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

            return Equals((WorldTile)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((RegionId != null ? RegionId.GetHashCode() : 0) * 397) ^
                       (BiomeId != null ? BiomeId.GetHashCode() : 0);
            }
        }
    }
}
