namespace Narivia.Interface.Widgets.Enumerations
{
    /// <summary>
    /// Notification icon.
    /// </summary>
    public enum NotificationIcon
    {
        /// <summary>
        /// 0 - Debug/Test notification icon.
        /// </summary>
        DebugTest = 0,

        /// <summary>
        /// 1 - Icon for battles that ended in defeat.
        /// </summary>
        BattleDefeat = 1,

        /// <summary>
        /// 2 - Icon for battles that ended in victory.
        /// </summary>
        BattleVictory = 2,

        /// <summary>
        /// 3 - Icon for destroyed faction notitifications.
        /// </summary>
        FactionDestroyed = 3,

        /// <summary>
        /// 3 - Icon for revived faction notitifications.
        /// </summary>
        FactionRevived = 4,

        /// <summary>
        /// 5 - Icon for battles where the player lost a region.
        /// </summary>
        RegionLost = 5,

        /// <summary>
        /// 6 - Icon for battles where the player defended a region.
        /// </summary>
        RegionDefended = 6,

        /// <summary>
        /// 7 - Icon for when a player region was liberated.
        /// </summary>
        RegionLiberated = 7,

        /// <summary>
        /// 9 - Icon for turn reports.
        /// </summary>
        TurnReport = 8,

        /// <summary>
        /// 4 - Icon fo recruitment reports.
        /// </summary>
        RecruitmentReport = 9,

        /// <summary>
        /// 8 - Icon for relations reports.
        /// </summary>
        RelationsReport = 10,

        /// <summary>
        /// 10 - Icon for generic information.
        /// </summary>
        Information = 11
    }
}
