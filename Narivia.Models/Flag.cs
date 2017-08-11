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
        /// Gets or sets the first layer.
        /// </summary>
        /// <value>The first layer.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Layer1 { get; set; }

        /// <summary>
        /// Gets or sets the second layer.
        /// </summary>
        /// <value>The second layer.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Layer2 { get; set; }

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
        /// Gets or sets the background colour.
        /// </summary>
        /// <value>The background colour.</value>
        public Color BackgroundColour { get; set; }

        /// <summary>
        /// Gets or sets the first layer's colour.
        /// </summary>
        /// <value>The first layer's colour.</value>
        public Color Layer1Colour { get; set; }

        /// <summary>
        /// Gets or sets the second layer's colour.
        /// </summary>
        /// <value>The second layer's colour.</value>
        public Color Layer2Colour { get; set; }

        /// <summary>
        /// Gets or sets the emblem colour.
        /// </summary>
        /// <value>The emblem colour.</value>
        public Color EmblemColour { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="Narivia.Models.Flag"/> is equal to the current <see cref="T:Narivia.Models.Flag"/>.
        /// </summary>
        /// <param name="other">The <see cref="Narivia.Models.Flag"/> to compare with the current <see cref="T:Narivia.Models.Flag"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Narivia.Models.Flag"/> is equal to the current
        /// <see cref="T:Narivia.Models.Flag"/>; otherwise, <c>false</c>.</returns>
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
                   string.Equals(Layer1, other.Layer1) &&
                   string.Equals(Emblem, other.Emblem) &&
                   string.Equals(Skin, other.Skin) &&
                   Equals(Layer1Colour, other.Layer1Colour) &&
                   Equals(Layer2Colour, other.Layer2Colour) &&
                   Equals(EmblemColour, other.EmblemColour);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:Narivia.Models.Flag"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:Narivia.Models.Flag"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="T:Narivia.Models.Flag"/>; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Serves as a hash function for a <see cref="T:Narivia.Models.Flag"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Id != null ? Id.GetHashCode() : 0) * 397) ^
                       (Layer1 != null ? Layer1.GetHashCode() : 0) ^
                       (Emblem != null ? Emblem.GetHashCode() : 0) ^
                       (Skin != null ? Skin.GetHashCode() : 0) ^
                       Layer1Colour.GetHashCode() ^
                       Layer2Colour.GetHashCode() ^
                       EmblemColour.GetHashCode();
            }
        }
    }
}
