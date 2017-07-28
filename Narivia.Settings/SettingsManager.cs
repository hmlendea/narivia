using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Narivia.Graphics;
using Narivia.Infrastructure.Helpers;
using Narivia.Logging;
using Narivia.Logging.Enumerations;

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
        /// Initializes a new instance of the <see cref="SettingsManager"/> class.
        /// </summary>
        public SettingsManager()
        {
            Resolution = new Vector2(1280, 720);
            Fullscreen = false;
        }

        /// <summary>
        /// Gets or sets the resolution.
        /// </summary>
        /// <value>The resolution.</value>
        public Vector2 Resolution { get; set; }

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
        /// Loads the settings.
        /// </summary>
        public void LoadContent()
        {
            if (!File.Exists(ApplicationPaths.SettingsFile))
            {
                LogManager.Instance.Warn(LogBuilder.BuildKvpMessage(
                    Operation.SettingsLoading,
                    OperationStatus.Failure,
                    new Dictionary<LogInfoKey, string>
                    {
                        { LogInfoKey.FileName, ApplicationPaths.SettingsFile },
                        { LogInfoKey.Message, "Settings file is missing. Using default settings." }
                    }));

                SaveContent();
                return;
            }

            XmlManager<SettingsManager> xmlManager = new XmlManager<SettingsManager>();
            SettingsManager storedSettings = xmlManager.Load(ApplicationPaths.SettingsFile);

            instance = storedSettings;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveContent()
        {
            XmlManager<SettingsManager> xmlManager = new XmlManager<SettingsManager>();
            xmlManager.Save(ApplicationPaths.SettingsFile, this);
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

            if (GraphicsManager.Instance.Graphics.PreferredBackBufferWidth != (int)Resolution.X ||
                GraphicsManager.Instance.Graphics.PreferredBackBufferHeight != (int)Resolution.Y)
            {
                GraphicsManager.Instance.Graphics.PreferredBackBufferWidth = (int)Resolution.X;
                GraphicsManager.Instance.Graphics.PreferredBackBufferHeight = (int)Resolution.Y;

                graphicsChanged = true;
            }

            if (graphicsChanged)
            {
                GraphicsManager.Instance.Graphics.ApplyChanges();
            }
        }
    }
}
