using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Gui.GuiElements;

namespace Narivia.Gui.Screens
{
    /// <summary>
    /// Title screen.
    /// </summary>
    public class TitleScreen : MenuScreen
    {
        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            Axis = "Y";

            Links.Add(new GuiMenuLink { Text = "New Game", LinkId = "NewGameScreen" });
            Links.Add(new GuiMenuLink { Text = "Settings", LinkId = "SettingsScreen" });
            Links.Add(new GuiMenuLink { Text = "Exit", LinkId = "Exit" });

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
