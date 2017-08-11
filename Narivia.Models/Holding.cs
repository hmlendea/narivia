using System;
using System.ComponentModel.DataAnnotations;

using Narivia.Models.Enumerations;

namespace Narivia.Models
{
    /// <summary>
    /// Holding domain model.
    /// </summary>
    public class Holding : IEquatable<Holding>
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
        /// Gets or sets the identifier of the province to which this holding belongs to.
        /// </summary>
        /// <value>The province identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string ProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public HoldingType Type { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="Holding"/> is equal to the current <see cref="Holding"/>.
        /// </summary>
        /// <param name="other">The <see cref="Holding"/> to compare with the current <see cref="Holding"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Holding"/> is equal to the current
        /// <see cref="Holding"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Holding other)
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
                   string.Equals(ProvinceId, other.ProvinceId) &&
                   Equals(Type, other.Type);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Holding"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Holding"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Holding"/>; otherwise, <c>false</c>.</returns>
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

            return Equals((Holding)obj);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Holding"/> object.
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
                       (ProvinceId != null ? ProvinceId.GetHashCode() : 0) ^
                       Type.GetHashCode();
            }
        }
    }
}
