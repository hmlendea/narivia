using System.ComponentModel.DataAnnotations;

namespace Narivia.Infrastructure.Logging.Enumerations
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
