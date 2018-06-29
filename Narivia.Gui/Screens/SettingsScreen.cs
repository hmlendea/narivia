using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Gui.GuiElements;
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
        public override void LoadContent()
        {
            fullScreenToggle = new GuiMenuToggle
            {
                Id = "fullscreenToggle",
                Text = "Toggle fullscreen mode",
                Property = "Fullscreen"
            };
            debugModeToggle = new GuiMenuToggle
            {
                Id = "debugToggle",
                Text = "Toggle debug mode",
                Property = "DebugMode"
            };
            backLink = new GuiMenuLink
            {
                Id = "back",
                Text = "Back",
                TargetScreen = typeof(TitleScreen)
            };
            
            Items.Add(fullScreenToggle);
            Items.Add(debugModeToggle);
            Items.Add(backLink);
            
            fullScreenToggle.ToggleState = SettingsManager.Instance.GraphicsSettings.Fullscreen;
            debugModeToggle.ToggleState = SettingsManager.Instance.DebugMode;

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            SettingsManager.Instance.SaveContent();
            base.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            SettingsManager.Instance.GraphicsSettings.Fullscreen = fullScreenToggle.ToggleState;
            SettingsManager.Instance.DebugMode = debugModeToggle.ToggleState;
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
