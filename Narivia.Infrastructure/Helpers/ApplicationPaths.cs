using System;
using System.IO;
using System.Reflection;

namespace Narivia.Infrastructure.Helpers
{
    public static class ApplicationPaths
    {
        static readonly string rootDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string UserDataDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Narivia");

        /// <summary>
        /// Gets the worlds directory.
        /// </summary>
        /// <value>The worlds directory.</value>
        public static string WorldsDirectory => Path.Combine(rootDirectory, "Worlds");

        /// <summary>
        /// Gets the options file.
        /// </summary>
        /// <value>The options file.</value>
        public static string SettingsFile => Path.Combine(UserDataDirectory, "Settings.xml");
    }
}
