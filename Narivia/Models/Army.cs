using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    /// <summary>
    /// Army domain model.
    /// </summary>
    public class Army : ModelBase
    {
        /// <summary>
        /// Gets or sets the faction identifier.
        /// </summary>
        /// <value>The faction identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string FactionId { get; set; }

        /// <summary>
        /// Gets or sets the unit identifier.
        /// </summary>
        /// <value>The unit identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string UnitId { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        [Range(0, int.MaxValue)]
        public int Size { get; set; }
    }
}
