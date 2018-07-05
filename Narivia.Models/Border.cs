using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    /// <summary>
    /// Border domain model.
    /// </summary>
    public sealed class Border : ModelBase
    {
        /// <summary>
        /// Gets the source province identifier.
        /// </summary>
        /// <value>The source province identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string SourceProvinceId { get; set; }

        /// <summary>
        /// Gets the target province identifier.
        /// </summary>
        /// <value>The target province identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string TargetProvinceId { get; set; }
    }
}