using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Gui.Screens;

namespace Narivia.Gui.Screens
{
    /// <summary>
    /// Title screen.
    /// </summary>
    public class TitleScreen : MenuScreen
    {
        GuiMenuLink newGameLink;
        GuiMenuLink settingsLink;
        GuiMenuAction extiAction;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            newGameLink = new GuiMenuLink
            {
                Id = "newGame",
                Text = "New Game",
                TargetScreen = typeof(NewGameScreen)
            };
            settingsLink = new GuiMenuLink
            {
                Id = "settings",
                Text = "Settings",
                TargetScreen = typeof(SettingsScreen)
            };
            extiAction = new GuiMenuAction
            {
                Id = "exit",
                Text = "Exit",
                ActionId = "Exit"
            };

            Items.Add(newGameLink);
            Items.Add(settingsLink);
            Items.Add(extiAction);

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
