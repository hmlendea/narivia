using System.ComponentModel.DataAnnotations;

namespace Narivia.Infrastructure.Logging.Enumerations
{
    public enum Operation
    {
        [Display(Name = "CONTENT_FILE_LOAD")]
        ContentFileLoad,

        [Display(Name = "GAME_START")]
        GameStart,

        [Display(Name = "GAME_STOP")]
        GameStop,

        [Display(Name = "WORLD_LOADING")]
        WorldLoading
    }
}
