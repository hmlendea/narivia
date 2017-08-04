using System;
using System.ComponentModel.DataAnnotations;

using Narivia.Models.Enumerations;

namespace Narivia.Models
{
    /// <summary>
    /// Culture domain model.
    /// </summary>
    public class Culture : IEquatable<Culture>
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
        /// Gets or sets the texture set.
        /// </summary>
        /// <value>The texture set.</value>
        [StringLength(20, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string TextureSet { get; set; }

        /// <summary>
        /// Gets or sets the place name generator.
        /// </summary>
        /// <value>The place name generator.</value>
        public NameGenerator PlaceNameGenerator { get; set; }

        /// <summary>
        /// Gets or sets the place name generator schema.
        /// </summary>
        /// <value>The place name generator schema.</value>
        public string PlaceNameSchema { get; set; }

        public bool Equals(Culture other)
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
                   string.Equals(TextureSet, other.TextureSet) &&
                   Equals(PlaceNameGenerator, other.PlaceNameGenerator) &&
                   string.Equals(PlaceNameSchema, other.PlaceNameSchema);
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

            return Equals((Culture)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Id != null ? Id.GetHashCode() : 0) * 397) ^
                       (Name != null ? Name.GetHashCode() : 0) ^
                       (Description != null ? Description.GetHashCode() : 0) ^
                       (TextureSet != null ? TextureSet.GetHashCode() : 0) ^
                       PlaceNameGenerator.GetHashCode() ^
                       (PlaceNameSchema != null ? PlaceNameSchema.GetHashCode() : 0);
            }
        }
    }
}
