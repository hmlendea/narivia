using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Gui.GuiElements;
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

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            Toggles.Add(new GuiMenuToggle { Text = "Toggle fullscreen mode", Property = "Fullscreen" });
            Toggles.Add(new GuiMenuToggle { Text = "Toggle debug mode", Property = "DebugMode" });
            Links.Add(new GuiMenuLink { Text = "Back", LinkId = "TitleScreen" });

            fullScreenToggle = Toggles.FirstOrDefault(t => t.Property == "Fullscreen");
            debugModeToggle = Toggles.FirstOrDefault(t => t.Property == "DebugMode");

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
