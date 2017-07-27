using System.ComponentModel.DataAnnotations;

namespace Narivia.Logging.Enumerations
{
    public enum Operation
    {
        [Display(Name = "CONTENT_FILE_LOAD")]
        ContentFileLoad,

        [Display(Name = "GAME_START")]
        GameStart,

        [Display(Name = "GAME_STOP")]
        GameStop,

        [Display(Name = "REPOSITORY_LOADING")]
        RepositoryLoading,

        [Display(Name = "REPOSITORY_SAVING")]
        RepositorySaving,

        [Display(Name = "SETTINGS_LOADING")]
        SettingsLoading,

        [Display(Name = "WORLD_INITIALISING")]
        WorldInitialisation,

        [Display(Name = "WORLD_LOADING")]
        WorldLoading,

        [Display(Name = "WORLD_LOADING_MAP")]
        WorldLoadingMap,

        [Display(Name = "WORLD_LOADING_ENTITIES")]
        WorldLoadingEntities
    }
}
