using System;
using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    /// <summary>
    /// World georaphic layer.
    /// </summary>
    public class WorldGeoLayer : IEquatable<WorldGeoLayer>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>The region identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Tileset { get; set; }

        /// <summary>
        /// Gets or sets the layer tiles.
        /// </summary>
        /// <value>The layer tiles.</value>
        public int[,] Tiles { get; set; }

        /// <summary>
        /// Gets or sets the layer opacity.
        /// </summary>
        /// <value>The layer opacity.</value>
        public float Opacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WorldGeoLayer"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="WorldGeoLayer"/> is equal to the current <see cref="WorldGeoLayer"/>.
        /// </summary>
        /// <param name="other">The <see cref="WorldGeoLayer"/> to compare with the current <see cref="WorldGeoLayer"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="WorldGeoLayer"/> is equal to the current
        /// <see cref="WorldGeoLayer"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(WorldGeoLayer other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Name, other.Name) &&
                   string.Equals(Tileset, other.Tileset) &&
                   Equals(Tiles, other.Tiles) &&
                   Equals(Opacity, other.Opacity) &&
                   Equals(Visible, other.Visible);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="WorldGeoLayer"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="WorldGeoLayer"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="WorldGeoLayer"/>; otherwise, <c>false</c>.</returns>
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

            return Equals((WorldGeoLayer)obj);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="WorldGeoLayer"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^
                       (Tileset != null ? Tileset.GetHashCode() : 0) ^
                       (Tiles != null ? Tiles.GetHashCode() : 0) ^
                       Opacity.GetHashCode() ^
                       Visible.GetHashCode();
            }
        }
    }
}
