using System.ComponentModel.DataAnnotations;

namespace Narivia.Logging.Enumerations
{
    public enum OperationStatus
    {
        [Display(Name = "STARTED")]
        Started,

        [Display(Name = "SUCCESS")]
        Success,

        [Display(Name = "FAILURE")]
        Failure
    }
}
