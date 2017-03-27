using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Menus;

namespace Narivia.Screens
{
    /// <summary>
    /// Title screen.
    /// </summary>
    public class TitleScreen : Screen
    {
        /// <summary>
        /// The menu manager.
        /// </summary>
        readonly MenuManager menuManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.Screens.TitleScreen"/> class.
        /// </summary>
        public TitleScreen()
        {
            menuManager = new MenuManager();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            menuManager.LoadContent("Menus/TitleMenu.xml");
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
            menuManager.UnloadContent();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            menuManager.Update(gameTime);
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            menuManager.Draw(spriteBatch);
        }
    }
}
