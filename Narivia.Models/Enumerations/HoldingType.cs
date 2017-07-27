using System.ComponentModel.DataAnnotations;

namespace Narivia.Models.Enumerations
{
    /// <summary>
    /// Holding Type
    /// </summary>
    public enum HoldingType
    {
        /// <summary>
        /// Empty land.
        /// </summary>
        [Display(Name = "N/A")]
        Empty = 0,

        /// <summary>
        /// Castle holding.
        /// </summary>
        [Display(Name = "Castle")]
        Castle = 1,

        /// <summary>
        /// City holding.
        /// </summary>L
        [Display(Name = "City")]
        City = 2,

        /// <summary>
        /// Temple holding.
        /// </summary>
        [Display(Name = "Temple")]
        Temple = 3
    }
}
