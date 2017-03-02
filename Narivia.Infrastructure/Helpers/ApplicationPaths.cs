using System.IO;
using System.Reflection;

namespace Narivia.Infrastructure.Helpers
{
    public static class ApplicationPaths
    {
        static readonly string rootDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static readonly string DataDirectory = Path.Combine(rootDirectory, "data");

        #region Directories


        /// <summary>
        /// Gets the resource packs directory.
        /// </summary>
        /// <value>The resource packs directory.</value>
        public static string ResourcePacksDirectory
        {
            get
            {
                return Path.Combine(DataDirectory, "resourcepacks");
            }
        }

        /// <summary>
        /// Gets the worlds directory.
        /// </summary>
        /// <value>The worlds directory.</value>
        public static string WorldsDirectory
        {
            get
            {
                return Path.Combine(DataDirectory, "worlds");
            }
        }

        /// <summary>
        /// Gets the sounds directory.
        /// </summary>
        /// <value>The sounds directory.</value>
        public static string SoundsDirectory
        {
            get
            {
                return Path.Combine(DataDirectory, "sound");
            }
        }

        #endregion

        #region Files

        /// <summary>
        /// Gets the options file.
        /// </summary>
        /// <value>The options file.</value>
        public static string OptionsFile
        {
            get
            {
                return Path.Combine(rootDirectory, "options.xml");
            }
        }

        #endregion
    }
}
