using Microsoft.Xna.Framework;

using Narivia.Helpers;

namespace Narivia.Settings
{
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
                            XmlManager<SettingsManager> xmlManager = new XmlManager<SettingsManager>();
                            instance = xmlManager.Load("Settings.xml");
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
        public Vector2 Resolution { get; set; }

        /// <summary>
        /// Gets or sets the fullscreen mode.
        /// </summary>
        /// <value>The fullscreen mode.</value>
        public bool Fullscreen { get; set; }

        /// <summary>
        /// Updates the settings.
        /// </summary>
        public void Update(ref GraphicsDeviceManager graphics)
        {
            if (graphics.IsFullScreen != Fullscreen)
            {
                graphics.ToggleFullScreen();
            }
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveContent()
        {
            XmlManager<SettingsManager> xmlManager = new XmlManager<SettingsManager>();
            xmlManager.Save("Settings.xml", this);
        }
    }
}
