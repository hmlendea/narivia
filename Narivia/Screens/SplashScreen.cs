using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Input;

namespace Narivia.Screens
{
    /// <summary>
    /// Splash screen.
    /// </summary>
    public class SplashScreen : Screen
    {
        float delay = 3;

        /// <summary>
        /// Gets or sets the background.
        /// </summary>
        /// <value>The background.</value>
        public Image Background { get; set; }

        /// <summary>
        /// Gets or sets the logo.
        /// </summary>
        /// <value>The logo.</value>
        public Image Logo { get; set; }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            Background.LoadContent();
            Logo.LoadContent();

            AlignItems();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
            Background.UnloadContent();
            Logo.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Background.Update(gameTime);
            Logo.Update(gameTime);

            delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            AlignItems();

            if (InputManager.Instance.IsMouseButtonDown(MouseButton.LeftButton) ||
                InputManager.Instance.IsAnyKeyDown() || delay <= 0)
            {
                ScreenManager.Instance.ChangeScreens("TitleScreen");
            }
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);
            Logo.Draw(spriteBatch);
        }

        void AlignItems()
        {
            Background.Position = (ScreenManager.Instance.Size - Background.Size) / 2;
            Background.Scale = ScreenManager.Instance.Size / Background.Size;
            Logo.Position = (ScreenManager.Instance.Size - Logo.Size) / 2;
        }
    }
}

