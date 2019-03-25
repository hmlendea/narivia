using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        protected override void DoLoadContent()
        {
            BackgroundImage = new GuiImage
            {
                Id = nameof(BackgroundImage),
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
                Id = nameof(OverlayImage),
                ContentFile = "SplashScreen/Overlay"
            };
            LogoImage = new GuiImage
            {
                Id = nameof(LogoImage),
                ContentFile = "SplashScreen/Logo"
            };

            GuiManager.Instance.GuiElements.Add(BackgroundImage);
            GuiManager.Instance.GuiElements.Add(OverlayImage);
            GuiManager.Instance.GuiElements.Add(LogoImage);

            SetChildrenProperties();
            RegisterEvents();
        }
        
        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            UnregisterEvents();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            SetChildrenProperties();

            if (Delay <= 0 && !ScreenManager.Instance.Transitioning)
            {
                ChangeScreens();
            }

            Delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        
        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            KeyPressed += OnKeyPressed;
            MouseButtonPressed += OnMouseButtonPressed;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            KeyPressed -= OnKeyPressed;
            MouseButtonPressed -= OnMouseButtonPressed;
        }

        void SetChildrenProperties()
        {
            OverlayImage.Size = ScreenManager.Instance.Size;

            BackgroundImage.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - BackgroundImage.ClientRectangle.Width) / 2,
                (ScreenManager.Instance.Size.Height - BackgroundImage.ClientRectangle.Height) / 2);

            LogoImage.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - LogoImage.Size.Width) / 2,
                (ScreenManager.Instance.Size.Height - LogoImage.Size.Height) / 2);
        }

        void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            ChangeScreens();
        }

        void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            ChangeScreens();
        }

        void ChangeScreens()
        {
            ScreenManager.Instance.ChangeScreens(typeof(TitleScreen));
        }
    }
}
