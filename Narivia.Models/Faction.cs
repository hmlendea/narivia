using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Xml.Serialization;

namespace Narivia.Models
{
    /// <summary>
    /// Faction domain model.
    /// </summary>
    public class Faction : IEquatable<Faction>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [StringLength(20, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [StringLength(300, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the flag identifier.
        /// </summary>
        /// <value>The flag identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string FlagId { get; set; }

        /// <summary>
        /// Gets or sets the culture identifier.
        /// </summary>
        /// <value>The culture identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string CultureId { get; set; }

        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        /// <value>The colour.</value>
        [XmlIgnore]
        public Color Colour { get; set; }

        /// <summary>
        /// Gets or sets the wealth.
        /// </summary>
        /// <value>The wealth.</value>
        [Range(0, int.MaxValue)]
        public int Wealth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Faction"/> is alive.
        /// </summary>
        /// <value><c>true</c> if alive; otherwise, <c>false</c>.</value>
        public bool Alive { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="Faction"/> is equal to the current <see cref="Faction"/>.
        /// </summary>
        /// <param name="other">The <see cref="Faction"/> to compare with the current <see cref="Faction"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Faction"/> is equal to the current
        /// <see cref="Faction"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Faction other)
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
                   string.Equals(Name, other.Name) &&
                   string.Equals(Description, other.Description) &&
                   string.Equals(FlagId, other.FlagId) &&
                   string.Equals(CultureId, other.CultureId) &&
                   Equals(Colour, other.Colour) &&
                   Equals(Wealth, other.Wealth) &&
                   Equals(Alive, other.Alive);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Faction"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Faction"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Faction"/>; otherwise, <c>false</c>.</returns>
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

            return Equals((Faction)obj);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Faction"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Id != null ? Id.GetHashCode() : 0) * 397) ^
                       (Name != null ? Name.GetHashCode() : 0) ^
                       (Description != null ? Description.GetHashCode() : 0) ^
                       (FlagId != null ? FlagId.GetHashCode() : 0) ^
                       (CultureId != null ? CultureId.GetHashCode() : 0) ^
                       Colour.GetHashCode() ^
                       Wealth.GetHashCode() ^
                       Alive.GetHashCode();
            }
        }
    }
}
