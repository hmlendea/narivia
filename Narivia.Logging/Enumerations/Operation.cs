using System.ComponentModel.DataAnnotations;

namespace Narivia.Logging.Enumerations
{
    /// <summary>
    /// Operation enumeration.
    /// </summary>
    public enum Operation
    {
        /// <summary>
        /// The content file loading operation.
        /// </summary>
        [Display(Name = "CONTENT_FILE_LOAD")]
        ContentFileLoad,

        /// <summary>
        /// The game start operation.
        /// </summary>
        [Display(Name = "GAME_START")]
        GameStart,

        /// <summary>
        /// The game stop operation.
        /// </summary>
        [Display(Name = "GAME_STOP")]
        GameStop,

        /// <summary>
        /// The repository loading operation.
        /// </summary>
        [Display(Name = "REPOSITORY_LOADING")]
        RepositoryLoading,

        /// <summary>
        /// The repository saving operation.
        /// </summary>
        [Display(Name = "REPOSITORY_SAVING")]
        RepositorySaving,

        /// <summary>
        /// The settings loading operation.
        /// </summary>
        [Display(Name = "SETTINGS_LOADING")]
        SettingsLoading,

        /// <summary>
        /// The world initialisation operation.
        /// </summary>
        [Display(Name = "WORLD_INITIALISING")]
        WorldInitialisation,

        /// <summary>
        /// The world loading operation.
        /// </summary>
        [Display(Name = "WORLD_LOADING")]
        WorldLoading,

        /// <summary>
        /// The map loading step of the world loading operation.
        /// </summary>
        [Display(Name = "WORLD_LOADING_MAP")]
        WorldLoadingMap,

        /// <summary>
        /// The entities loading step of the world loading operation.
        /// </summary>
        [Display(Name = "WORLD_LOADING_ENTITIES")]
        WorldLoadingEntities
    }
}
