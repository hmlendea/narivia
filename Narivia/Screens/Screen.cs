using System;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Input;

namespace Narivia.Screens
{
    /// <summary>
    /// Screen.
    /// </summary>
    public class Screen
    {
        protected ContentManager content;

        /// <summary>
        /// Gets or sets the xml path.
        /// </summary>
        /// <value>The xml path.</value>
        public string XmlPath { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [XmlIgnore]
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the background colour.
        /// </summary>
        /// <value>The background colour.</value>
        public Color BackgroundColour { get; set; }

        Texture2D background;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Screens.Screen"/> class.
        /// </summary>
        public Screen()
        {
            Type = GetType();
            XmlPath = @"Screens/" + Type.ToString().Replace("Narivia.Screens.", "") + ".xml";
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public virtual void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");


            background = new Texture2D(ScreenManager.Instance.GraphicsDevice,
                                             (int)ScreenManager.Instance.Size.X,
                                             (int)ScreenManager.Instance.Size.Y);

            Color[] data = new Color[background.Width * background.Height];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = BackgroundColour;
            }

            background.SetData(data);
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            content.Unload();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
        }
    }
}

