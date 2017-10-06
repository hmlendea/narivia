using System.IO;
using System.Collections.Generic;

using Narivia.DataAccess.IO;
using Narivia.Graphics;
using Narivia.Graphics.Geometry;

using NuciLog;
using NuciLog.Enumerations;

namespace Narivia.Settings
{
    /// <summary>
    /// Settings manager.
    /// </summary>
    public class SettingsManager
    {
        static volatile SettingsManager instance;
        static object syncRoot = new object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static SettingsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SettingsManager();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets or sets the resolution.
        /// </summary>
        /// <value>The resolution.</value>
        public Size2D Resolution { get; set; }

        /// <summary>
        /// Gets or sets the fullscreen mode.
        /// </summary>
        /// <value>The fullscreen mode.</value>
        public bool Fullscreen { get; set; }

        /// <summary>
        /// Gets or sets the debug mode.
        /// </summary>
        /// <value>The debug mode.</value>
        public bool DebugMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sound is enabled.
        /// </summary>
        /// <value>The sound toggle.</value>
        public bool SoundEnabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsManager"/> class.
        /// </summary>
        public SettingsManager()
        {
            Resolution = new Size2D(1280, 720);
            Fullscreen = false;
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadContent()
        {
            if (!File.Exists(ApplicationPaths.SettingsFile))
            {
                string logMessage = "Settings file is missing. Using default settings.";
                Dictionary<LogInfoKey, string> logDetails = new Dictionary<LogInfoKey, string>
                {
                    { LogInfoKey.FileName, ApplicationPaths.SettingsFile }
                };

                LogManager.Instance.Warn(Operation.SettingsLoading, OperationStatus.Failure, logMessage, logDetails);

                SaveContent();
                return;
            }

            XmlManager<SettingsManager> xmlManager = new XmlManager<SettingsManager>();
            SettingsManager storedSettings = xmlManager.Read(ApplicationPaths.SettingsFile);

            instance = storedSettings;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveContent()
        {
            XmlManager<SettingsManager> xmlManager = new XmlManager<SettingsManager>();
            xmlManager.Write(ApplicationPaths.SettingsFile, this);
        }

        /// <summary>
        /// Updates the settings.
        /// </summary>
        public void Update()
        {
            bool graphicsChanged = false;

            if (GraphicsManager.Instance.Graphics.IsFullScreen != Fullscreen)
            {
                GraphicsManager.Instance.Graphics.IsFullScreen = Fullscreen;

                graphicsChanged = true;
            }

            if (GraphicsManager.Instance.Graphics.PreferredBackBufferWidth != Resolution.Width ||
                GraphicsManager.Instance.Graphics.PreferredBackBufferHeight != Resolution.Height)
            {
                GraphicsManager.Instance.Graphics.PreferredBackBufferWidth = Resolution.Width;
                GraphicsManager.Instance.Graphics.PreferredBackBufferHeight = Resolution.Height;

                graphicsChanged = true;
            }

            if (graphicsChanged)
            {
                GraphicsManager.Instance.Graphics.ApplyChanges();
            }
        }
    }
}
