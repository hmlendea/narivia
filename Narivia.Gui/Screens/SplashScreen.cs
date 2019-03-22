using Microsoft.Xna.Framework;
using NuciXNA.Gui;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Gui.Screens;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Input;
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
            Delay = 3;
            BackgroundColour = Colour.DodgerBlue;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            BackgroundImage = new GuiImage
            {
                Id = "backgroundImage",
                ContentFile = "SplashScreen/Background",
                RotationEffect = new OscilationEffect
                {
                    Speed = 0.25f,
                    MinimumMultiplier = 0.5f,
                    MaximumMultiplier = 1.5f
                },
                ScaleEffect = new ZoomEffect
                {
                    MinimumMultiplier = 0.5f,
                    MaximumMultiplier = 1.5f
                },
                AreEffectsActive = true
            };
            OverlayImage = new GuiImage
            {
                Id = "overlayImage",
                ContentFile = "SplashScreen/Overlay"
            };
            LogoImage = new GuiImage
            {
                Id = "logoImage",
                ContentFile = "SplashScreen/Logo"
            };

            GuiManager.Instance.GuiElements.Add(BackgroundImage);
            GuiManager.Instance.GuiElements.Add(OverlayImage);
            GuiManager.Instance.GuiElements.Add(LogoImage);

            base.LoadContent();

            BackgroundImage.RotationEffect.Activate();
            BackgroundImage.ScaleEffect.Activate();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Delay <= 0 && !ScreenManager.Instance.Transitioning)
            {
                ChangeScreens();
            }

            Delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void SetChildrenProperties()
        {
            OverlayImage.Size = ScreenManager.Instance.Size;

            BackgroundImage.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - BackgroundImage.ClientRectangle.Width) / 2,
                (ScreenManager.Instance.Size.Height - BackgroundImage.ClientRectangle.Height) / 2);

            LogoImage.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - LogoImage.Size.Width) / 2,
                (ScreenManager.Instance.Size.Height - LogoImage.Size.Height) / 2);
        }

        protected override void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            base.OnKeyPressed(sender, e);

            ChangeScreens();
        }

        protected override void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseButtonPressed(sender, e);

            ChangeScreens();
        }

        void ChangeScreens()
        {
            ScreenManager.Instance.ChangeScreens(typeof(TitleScreen));
        }
    }
}
