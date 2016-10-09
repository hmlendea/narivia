using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Narivia.UI.Screens
{
    /// <summary>
    /// Screen manager.
    /// </summary>
    public class ScreenManager
    {
        static ScreenManager instance;

        readonly Screen currentScreen;
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
                    instance = new ScreenManager();

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
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            currentScreen.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
        }

        /// <summary>
        /// Draw the content.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
        }
    }
}

