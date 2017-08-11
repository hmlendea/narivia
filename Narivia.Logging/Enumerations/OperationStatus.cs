using System.ComponentModel.DataAnnotations;

namespace Narivia.Logging.Enumerations
{
    /// <summary>
    /// Operation status.
    /// </summary>
    public enum OperationStatus
    {
        /// <summary>
        /// The started operation status.
        /// </summary>
        [Display(Name = "STARTED")]
        Started,

        /// <summary>
        /// The success operation status.
        /// </summary>
        [Display(Name = "SUCCESS")]
        Success,

        /// <summary>
        /// The failure operation status.
        /// </summary>
        [Display(Name = "FAILURE")]
        Failure
    }
}
