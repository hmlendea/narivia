using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Infrastructure.Helpers;
using Narivia.Input;
using Narivia.Input.Events;

namespace Narivia.Screens
{
    /// <summary>
    /// Splash screen.
    /// </summary>
    public class SplashScreen : Screen
    {
        /// <summary>
        /// Gets or sets the delay.
        /// </summary>
        /// <value>The delay.</value>
        public float Delay { get; set; }

        /// <summary>
        /// Gets or sets the background.
        /// </summary>
        /// <value>The background.</value>
        public Sprite BackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets the overlay.
        /// </summary>
        /// <value>The overlay.</value>
        public Sprite OverlayImage { get; set; }

        /// <summary>
        /// Gets or sets the logo.
        /// </summary>
        /// <value>The logo.</value>
        public Sprite LogoImage { get; set; }

        public SplashScreen()
        {
            BackgroundColour = Colour.DodgerBlue;
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

            InputManager.Instance.KeyboardKeyPressed += InputManager_OnKeyboardKeyPressed;
            InputManager.Instance.MouseButtonPressed += InputManager_OnMouseButtonPressed;
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

            InputManager.Instance.KeyboardKeyPressed -= InputManager_OnKeyboardKeyPressed;
            InputManager.Instance.MouseButtonPressed -= InputManager_OnMouseButtonPressed;
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
                Math.Max(BackgroundImage.SpriteSize.X, BackgroundImage.SpriteSize.Y);

            Vector2 overlayScale = new Vector2(ScreenManager.Instance.Size.X / OverlayImage.SpriteSize.X,
                                               ScreenManager.Instance.Size.Y / OverlayImage.SpriteSize.Y);

            BackgroundImage.Position = (ScreenManager.Instance.Size - BackgroundImage.SpriteSize) / 2;
            BackgroundImage.Scale = backgroundScale;

            OverlayImage.Position = (ScreenManager.Instance.Size - OverlayImage.SpriteSize) / 2;
            OverlayImage.Scale = overlayScale;

            LogoImage.Position = (ScreenManager.Instance.Size - LogoImage.SpriteSize) / 2;
        }

        void InputManager_OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            ScreenManager.Instance.ChangeScreens("TitleScreen");
        }

        void InputManager_OnKeyboardKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            ScreenManager.Instance.ChangeScreens("TitleScreen");
        }
    }
}
