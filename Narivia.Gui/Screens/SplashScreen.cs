using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Gui.Screens;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;

namespace Narivia.Gui.Screens
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
        public GuiImage BackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets the overlay.
        /// </summary>
        /// <value>The overlay.</value>
        public GuiImage OverlayImage { get; set; }

        /// <summary>
        /// Gets or sets the logo.
        /// </summary>
        /// <value>The logo.</value>
        public GuiImage LogoImage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplashScreen"/> class.
        /// </summary>
        public SplashScreen()
        {
            BackgroundColour = Colour.DodgerBlue;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            Delay = 3;
            BackgroundColour = new Colour(94, 96, 192);

            BackgroundImage = new GuiImage
            {
                ContentFile = "SplashScreen/Background",
                RotationEffect = new RotationEffect
                {
                    Speed = 0.1f,
                    MaximumRotation = 0.2f
                },
                ZoomEffect = new ZoomEffect
                {
                    Speed = 0.1f,
                    MinimumZoom = 1.25f,
                    MaximumZoom = 2.00f
                }
            };
            OverlayImage = new GuiImage
            {
                ContentFile = "SplashScreen/Overlay"
            };
            LogoImage = new GuiImage
            {
                ContentFile = "SplashScreen/Logo"
            };

            BackgroundImage.LoadContent();
            OverlayImage.LoadContent();
            LogoImage.LoadContent();

            base.LoadContent();
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

            // TODO: Align and size the elements
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

        protected override void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            base.OnKeyPressed(sender, e);

            ScreenManager.Instance.ChangeScreens(typeof(TitleScreen));
        }

        protected override void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseButtonPressed(sender, e);

            ScreenManager.Instance.ChangeScreens(typeof(TitleScreen));
        }
    }
}
