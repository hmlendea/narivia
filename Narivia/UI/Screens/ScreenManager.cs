using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Narivia.UI.Screens
{
    public class ScreenManager
    {
        static ScreenManager instance;

        ContentManager content;
        Vector2 dimensions;
        GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;

        Screen currentScreen;

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

        [XmlIgnore]
        public ContentManager Content { get { return content; } }

        [XmlIgnore]
        public Vector2 Dimensions { get { return dimensions; } }

        [XmlIgnore]
        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
            set { graphicsDevice = value; }
        }

        [XmlIgnore]
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.UI.Screens.ScreenManager"/> class.
        /// </summary>
        public ScreenManager()
        {
            dimensions = new Vector2(800, 480);
            currentScreen = new SplashScreen();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="content">Content.</param>
        public void LoadContent(ContentManager content)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
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

