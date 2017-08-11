using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Xml.Serialization;

using Narivia.Models.Enumerations;

namespace Narivia.Models
{
    /// <summary>
    /// Region domain model.
    /// </summary>
    public class Region : IEquatable<Region>
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
        /// Gets or sets the resource identifier.
        /// </summary>
        /// <value>The resource identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string ResourceId { get; set; }

        /// <summary>
        /// Gets the faction identifier.
        /// </summary>
        /// <value>The faction identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string FactionId { get; set; }

        /// <summary>
        /// Gets the sovereign faction identifier.
        /// </summary>
        /// <value>The sovereign faction identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string SovereignFactionId { get; set; }

        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        /// <value>The colour.</value>
        [XmlIgnore]
        public Color Colour { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Region"/> is locked.
        /// A region is normally locked as soon as it is conquered, and is unlocked when the turn has passed.
        /// This is done in order to prevent the same region from being conquered multiple times in one turn.
        /// </summary>
        /// <value><c>true</c> if locked; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool Locked { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [XmlIgnore]
        public RegionType Type { get; set; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        [XmlIgnore]
        public RegionState State
        {
            get
            {
                if (FactionId == SovereignFactionId)
                {
                    return RegionState.Sovereign;
                }

                return RegionState.Occupied;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="Region"/> is equal to the current <see cref="Region"/>.
        /// </summary>
        /// <param name="other">The <see cref="Region"/> to compare with the current <see cref="Region"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Region"/> is equal to the current
        /// <see cref="Region"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Region other)
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
                   string.Equals(ResourceId, other.ResourceId) &&
                   string.Equals(FactionId, other.FactionId) &&
                   string.Equals(SovereignFactionId, other.SovereignFactionId) &&
                   Equals(Colour, other.Colour) &&
                   Equals(Locked, other.Locked) &&
                   Equals(Type, other.Type);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Region"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Region"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Region"/>; otherwise, <c>false</c>.</returns>
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

            return Equals((Region)obj);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Region"/> object.
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
                       (ResourceId != null ? ResourceId.GetHashCode() : 0) ^
                       (FactionId != null ? FactionId.GetHashCode() : 0) ^
                       (SovereignFactionId != null ? SovereignFactionId.GetHashCode() : 0) ^
                       (Colour != null ? Colour.GetHashCode() : 0) ^
                       Locked.GetHashCode() ^
                       Type.GetHashCode();
            }
        }
    }
}
