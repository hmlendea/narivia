using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    /// <summary>
    /// Relation domain model.
    /// </summary>
    public sealed class Relation : ModelBase
    {
        /// <summary>
        /// Gets the source faction identifier.
        /// </summary>
        /// <value>The source faction identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string SourceFactionId { get; set; }

        /// <summary>
        /// Gets the target province identifier.
        /// </summary>
        /// <value>The target province identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string TargetFactionId { get; set; }

        /// <summary>
        /// Gets or sets the relation value.
        /// </summary>
        /// <value>The relation value.</value>
        [Range(-100, 100)]
        public int Value { get; set; }
    }
}