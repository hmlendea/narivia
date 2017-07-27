using System;
using System.IO;
using System.Reflection;

namespace Narivia.Infrastructure.Helpers
{
    /// <summary>
    /// Application paths.
    /// </summary>
    public static class ApplicationPaths
    {
        static readonly string rootDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Gets the user data directory.
        /// </summary>
        /// <value>The user data directory.</value>
        public static string UserDataDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Narivia");

        /// <summary>
        /// Gets the worlds directory.
        /// </summary>
        /// <value>The worlds directory.</value>
        public static string WorldsDirectory => Path.Combine(rootDirectory, "Worlds");

        /// <summary>
        /// Gets the common assets directory.
        /// </summary>
        /// <value>The common assets directory.</value>
        public static string CommonAssetsDirectory => Path.Combine(rootDirectory, "CommonAssets");

        /// <summary>
        /// Gets the logs directory path.
        /// </summary>
        /// <value>The path to the logs directory.</value>
        public static string LogsDirectory => Path.Combine(UserDataDirectory, "Logs");

        /// <summary>
        /// Gets the word lists directory.
        /// </summary>
        /// <value>The word lists directory.</value>
        public static string WordListsDirectory => Path.Combine(CommonAssetsDirectory, "WordLists");

        /// <summary>
        /// Gets the options file.
        /// </summary>
        /// <value>The options file.</value>
        public static string SettingsFile => Path.Combine(UserDataDirectory, "Settings.xml");
    }
}
