using System.IO;
using System.Reflection;

namespace Narivia.Infrastructure.Helpers
{
    public static class ApplicationPaths
    {
        static readonly string rootDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        #region Directories

        /// <summary>
        /// Gets the worlds directory.
        /// </summary>
        /// <value>The worlds directory.</value>
        public static string WorldsDirectory
        {
            get
            {
                return Path.Combine(rootDirectory, "Worlds");
            }
        }

        #endregion

        #region Files

        /// <summary>
        /// Gets the options file.
        /// </summary>
        /// <value>The options file.</value>
        public static string SettingsFile
        {
            get
            {
                return Path.Combine(rootDirectory, "Settings.xml");
            }
        }

        #endregion
    }
}
