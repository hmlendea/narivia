using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    public sealed class Building : ModelBase
    {
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string BuildingId { get; set; }
    }
}
