using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    /// <summary>
    /// Entity base.
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public virtual string Id { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Narivia.Models.EntityBase"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Narivia.Models.EntityBase"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Narivia.Models.EntityBase"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            EntityBase other = obj as EntityBase;
            return Id == other?.Id;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Narivia.Models.EntityBase"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Narivia.Models.EntityBase"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Narivia.Models.EntityBase"/>.</returns>
        public override string ToString()
        {
            return string.Format("{0} #{1}", base.ToString(), Id);
        }
    }
}
