using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui;
using NuciXNA.Gui.Controls;
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

        GuiImage backgroundImage;
        GuiImage overlayImage;
        GuiImage logoImage;

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
            backgroundImage = new GuiImage
            {
                Id = nameof(backgroundImage),
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
            overlayImage = new GuiImage
            {
                Id = nameof(overlayImage),
                ContentFile = "SplashScreen/Overlay"
            };
            logoImage = new GuiImage
            {
                Id = nameof(logoImage),
                ContentFile = "SplashScreen/Logo"
            };

            GuiManager.Instance.RegisterControls(backgroundImage, overlayImage, logoImage);
            
            RegisterEvents();
            SetChildrenProperties();
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
            
            if (!backgroundImage.RotationEffect.IsActive)
            {
                backgroundImage.RotationEffect.Activate();
                backgroundImage.ScaleEffect.Activate();
            }

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
            overlayImage.Size = ScreenManager.Instance.Size;

            backgroundImage.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - backgroundImage.ClientRectangle.Width) / 2,
                (ScreenManager.Instance.Size.Height - backgroundImage.ClientRectangle.Height) / 2);

            logoImage.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - logoImage.Size.Width) / 2,
                (ScreenManager.Instance.Size.Height - logoImage.Size.Height) / 2);
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
