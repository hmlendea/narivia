using System;

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
        public float Delay { get; set; }

        /// <summary>
        /// Gets or sets the background.
        /// </summary>
        /// <value>The background.</value>
        public Image BackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets the overlay.
        /// </summary>
        /// <value>The overlay.</value>
        public Image OverlayImage { get; set; }

        /// <summary>
        /// Gets or sets the logo.
        /// </summary>
        /// <value>The logo.</value>
        public Image LogoImage { get; set; }

        public SplashScreen()
        {
            BackgroundColour = Color.DodgerBlue;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            BackgroundImage.LoadContent();
            OverlayImage.LoadContent();
            LogoImage.LoadContent();

            AlignItems();
            BackgroundImage.ActivateEffect("RotationEffect");
            BackgroundImage.ActivateEffect("ZoomEffect");
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
            BackgroundImage.UnloadContent();
            OverlayImage.UnloadContent();
            LogoImage.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            BackgroundImage.Update(gameTime);
            OverlayImage.Update(gameTime);
            LogoImage.Update(gameTime);

            Delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            AlignItems();

            if (InputManager.Instance.IsMouseButtonDown(MouseButton.LeftButton) ||
                InputManager.Instance.IsAnyKeyDown() || Delay <= 0)
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
            base.Draw(spriteBatch);

            BackgroundImage.Draw(spriteBatch);
            OverlayImage.Draw(spriteBatch);
            LogoImage.Draw(spriteBatch);
        }

        void AlignItems()
        {
            Vector2 backgroundScale = Vector2.One * 
                Math.Max(ScreenManager.Instance.Size.X, ScreenManager.Instance.Size.Y) /
                Math.Max(BackgroundImage.Size.X, BackgroundImage.Size.Y);

            Vector2 overlayScale = new Vector2(ScreenManager.Instance.Size.X / OverlayImage.Size.X,
                                               ScreenManager.Instance.Size.Y / OverlayImage.Size.Y);
            
            BackgroundImage.Position = (ScreenManager.Instance.Size - BackgroundImage.Size) / 2;
            BackgroundImage.Scale = backgroundScale;

            OverlayImage.Position = (ScreenManager.Instance.Size - OverlayImage.Size) / 2;
            OverlayImage.Scale = overlayScale;

            LogoImage.Position = (ScreenManager.Instance.Size - LogoImage.Size) / 2;
        }
    }
}

