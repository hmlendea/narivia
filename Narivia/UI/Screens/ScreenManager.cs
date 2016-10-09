using System;
using System.IO;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Narivia.UI.Graphics;

namespace Narivia.UI.Screens
{
    /// <summary>
    /// Screen manager.
    /// </summary>
    public class ScreenManager
    {
        static ScreenManager instance;

        Screen currentScreen, newScreen;
        XmlScreenManager<Screen> xmlScreenManager;

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
                    XmlScreenManager<ScreenManager> xml = new XmlScreenManager<ScreenManager>();
                    instance = xml.Load("UI/Screens/ScreenManager.xml");
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>The content.</value>
        [XmlIgnore]
        public ContentManager Content { get; private set; }

        /// <summary>
        /// Gets the dimensions.
        /// </summary>
        /// <value>The dimensions.</value>
        [XmlIgnore]
        public Vector2 Dimensions { get; private set; }

        /// <summary>
        /// Gets or sets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        [XmlIgnore]
        public GraphicsDevice GraphicsDevice { get; set; }

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
        public Image Image { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.UI.Screens.ScreenManager"/> class.
        /// </summary>
        public ScreenManager()
        {
            Dimensions = new Vector2(800, 480);
            currentScreen = new SplashScreen();

            xmlScreenManager = new XmlScreenManager<Screen>();
            xmlScreenManager.Type = currentScreen.Type;

            currentScreen = xmlScreenManager.Load("UI/Screens/SplashScreen.xml");
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="content">Content.</param>
        public void LoadContent(ContentManager content)
        {
            Content = new ContentManager(content.ServiceProvider, "Content");

            currentScreen.LoadContent();
            Image.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            currentScreen.UnloadContent();
            Image.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
            Transition(gameTime);
        }

        /// <summary>
        /// Draw the content.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);

            if (Transitioning)
                Image.Draw(spriteBatch);
        }

        /// <summary>
        /// Changes the screen.
        /// </summary>
        /// <param name="screenName">Screen name.</param>
        public void ChangeScreens(string screenName)
        {
            newScreen = (Screen)Activator.CreateInstance(Type.GetType("Narivia.UI.Screens." + screenName));

            Image.Active = true;
            Image.FadeEffect.Increasing = true;
            Image.Opacity = 0.0f;
            Transitioning = true;
        }

        /// <summary>
        /// Transitions to the new screen.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        void Transition(GameTime gameTime)
        {
            if (Transitioning)
            {
                Image.Update(gameTime);

                if (Image.Opacity == 1.0f)
                {
                    currentScreen.UnloadContent();
                    currentScreen = newScreen;
                    xmlScreenManager.Type = currentScreen.Type;

                    if (File.Exists(currentScreen.XmlPath))
                        currentScreen = xmlScreenManager.Load(currentScreen.XmlPath);

                    currentScreen.LoadContent();
                }
                else if (Image.Opacity == 0.0f)
                {
                    Image.Active = false;
                    Transitioning = false;
                }
            }
        }
    }
}

