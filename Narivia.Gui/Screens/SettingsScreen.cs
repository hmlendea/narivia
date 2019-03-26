
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;
using NuciXNA.Gui.Screens;

using Narivia.Settings;

namespace Narivia.Gui.Screens
{
    /// <summary>
    /// Settings screen.
    /// </summary>
    public class SettingsScreen : MenuScreen
    {
        GuiMenuToggle fullScreenToggle;
        GuiMenuToggle debugModeToggle;
        GuiMenuLink backLink;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            fullScreenToggle = new GuiMenuToggle
            {
                Id = nameof(fullScreenToggle),
                Text = "Toggle fullscreen mode"
            };
            debugModeToggle = new GuiMenuToggle
            {
                Id = nameof(debugModeToggle),
                Text = "Toggle debug mode"
            };
            backLink = new GuiMenuLink
            {
                Id = nameof(backLink),
                Text = "Back",
                TargetScreen = typeof(TitleScreen)
            };
            
            Items.Add(fullScreenToggle);
            Items.Add(debugModeToggle);
            Items.Add(backLink);

            RegisterEvents();
            
            fullScreenToggle.SetState(SettingsManager.Instance.GraphicsSettings.Fullscreen);
            debugModeToggle.SetState(SettingsManager.Instance.DebugMode);

            base.DoLoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            SettingsManager.Instance.SaveContent();

            UnregisterEvents();

            base.DoUnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            SettingsManager.Instance.GraphicsSettings.Fullscreen = fullScreenToggle.IsOn;
            SettingsManager.Instance.DebugMode = debugModeToggle.IsOn;

            base.DoUnloadContent();
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            fullScreenToggle.StateChanged += OnFullscreenToggleStateChanged;
            debugModeToggle.StateChanged += OnDebugModeToggleStateChanged;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            fullScreenToggle.StateChanged -= OnFullscreenToggleStateChanged;
            debugModeToggle.StateChanged -= OnDebugModeToggleStateChanged;
        }

        void OnFullscreenToggleStateChanged(object sender, EventArgs e)
        {
            SettingsManager.Instance.GraphicsSettings.Fullscreen = fullScreenToggle.IsOn;
        }

        void OnDebugModeToggleStateChanged(object sender, EventArgs e)
        {
            SettingsManager.Instance.DebugMode = debugModeToggle.IsOn;
        }
    }
}
