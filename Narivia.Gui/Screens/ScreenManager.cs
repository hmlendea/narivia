using System;
using System.IO;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Graphics.CustomSpriteEffects;
using Narivia.Common.Helpers;
using Narivia.Settings;

namespace Narivia.Gui.Screens
{
    /// <summary>
    /// Screen manager.
    /// </summary>
    public class ScreenManager
    {
        static volatile ScreenManager instance;
        static object syncRoot = new object();

        Screen currentScreen, newScreen;

        readonly XmlManager<Screen> xmlScreenManager;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            XmlManager<ScreenManager> xmlManager = new XmlManager<ScreenManager>();
                            instance = xmlManager.Load(Path.Combine("Screens", $"{nameof(ScreenManager)}.xml"));
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        [XmlIgnore]
        public Vector2 Size { get; private set; }

        /// <summary>
        /// Gets or sets the sprite batch.
        /// </summary>
        /// <value>The sprite batch.</value>
        [XmlIgnore]
        public SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current screen is transitioning.
        /// </summary>
        /// <value><c>true</c> if transitioning; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool Transitioning { get; private set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public Sprite TransitionImage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenManager"/> class.
        /// </summary>
        public ScreenManager()
        {
            Size = SettingsManager.Instance.Resolution;
            currentScreen = new SplashScreen();

            xmlScreenManager = new XmlManager<Screen>();
            xmlScreenManager.Type = currentScreen.Type;

            currentScreen = xmlScreenManager.Load(Path.Combine("Screens", $"{nameof(SplashScreen)}.xml"));
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            currentScreen.LoadContent();
            TransitionImage.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            currentScreen.UnloadContent();
            TransitionImage.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);

            if (Transitioning)
            {
                Transition(gameTime);
                return;
            }

            Size = SettingsManager.Instance.Resolution;
            TransitionImage.Scale = Size;
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);

            if (Transitioning)
            {
                TransitionImage.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Changes the screen.
        /// </summary>
        /// <param name="screenName">Screen name.</param>
        public void ChangeScreens(string screenName)
        {
            ChangeScreens(screenName, null);
        }

        /// <summary>
        /// Changes the screen.
        /// </summary>
        /// <param name="screenName">Screen name.</param>
        /// <param name="screenArgs">Screen arguments.</param>
        public void ChangeScreens(string screenName, string[] screenArgs)
        {
            newScreen = (Screen)Activator.CreateInstance(Type.GetType($"{typeof(Screen).Namespace}.{screenName}"));

            xmlScreenManager.Type = newScreen.Type;

            if (File.Exists(currentScreen.XmlPath))
            {
                newScreen = xmlScreenManager.Load(newScreen.XmlPath);
            }

            newScreen.ScreenArgs = screenArgs;

            TransitionImage.ActivateEffect(nameof(FadeEffect));
            TransitionImage.Active = true;
            TransitionImage.FadeEffect.Increasing = true;
            TransitionImage.Opacity = 0.0f;

            Transitioning = true;
        }

        /// <summary>
        /// Transitions to the new screen.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        void Transition(GameTime gameTime)
        {
            TransitionImage.Update(gameTime);

            if (TransitionImage.Opacity >= 1.0f)
            {
                currentScreen.UnloadContent();
                currentScreen = newScreen;
                currentScreen.LoadContent();
            }
            else if (TransitionImage.Opacity <= 0.0f)
            {
                TransitionImage.Active = false;
                Transitioning = false;
            }
        }
    }
}

