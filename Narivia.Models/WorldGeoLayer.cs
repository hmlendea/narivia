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
