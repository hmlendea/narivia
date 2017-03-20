using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Graphics;
using Narivia.Input;

namespace Narivia.Screens
{
    /// <summary>
    /// Splash screen.
    /// </summary>
    public class SplashScreen : Screen
    {
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public Image Image { get; set; }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            Image.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
            Image.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Image.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.Enter))
            {
                ScreenManager.Instance.ChangeScreens("TitleScreen");
            }
        }

        /// <summary>
        /// Draws the content.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}

