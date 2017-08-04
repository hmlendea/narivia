using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Narivia.Models
{
    /// <summary>
    /// Faction flag.
    /// </summary>
    public class Flag : IEquatable<Flag>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the background.
        /// </summary>
        /// <value>The background.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Background { get; set; }

        /// <summary>
        /// Gets or sets the emblem.
        /// </summary>
        /// <value>The emblem.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Emblem { get; set; }

        /// <summary>
        /// Gets or sets the skin.
        /// </summary>
        /// <value>The skin.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Skin { get; set; }

        /// <summary>
        /// Gets or sets the background primary colour.
        /// </summary>
        /// <value>The background primary colour.</value>
        public Color BackgroundPrimaryColour { get; set; }

        /// <summary>
        /// Gets or sets the background secondary colour.
        /// </summary>
        /// <value>The background secondary colour.</value>
        public Color BackgroundSecondaryColour { get; set; }

        /// <summary>
        /// Gets or sets the emblem colour.
        /// </summary>
        /// <value>The emblem colour.</value>
        public Color EmblemColour { get; set; }

        public bool Equals(Flag other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Id, other.Id) &&
                   string.Equals(Background, other.Background) &&
                   string.Equals(Emblem, other.Emblem) &&
                   string.Equals(Skin, other.Skin) &&
                   Equals(BackgroundPrimaryColour, other.BackgroundPrimaryColour) &&
                   Equals(BackgroundSecondaryColour, other.BackgroundSecondaryColour) &&
                   Equals(EmblemColour, other.EmblemColour);
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

            return Equals((Flag)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Id != null ? Id.GetHashCode() : 0) * 397) ^
                       (Background != null ? Background.GetHashCode() : 0) ^
                       (Emblem != null ? Emblem.GetHashCode() : 0) ^
                       (Skin != null ? Skin.GetHashCode() : 0) ^
                       (BackgroundPrimaryColour != null ? BackgroundPrimaryColour.GetHashCode() : 0) ^
                       (BackgroundSecondaryColour != null ? BackgroundSecondaryColour.GetHashCode() : 0) ^
                       (EmblemColour != null ? EmblemColour.GetHashCode() : 0);
            }
        }
    }
}
