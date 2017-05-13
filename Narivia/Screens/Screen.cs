using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Widgets;

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

        /// <summary>
        /// Gets or sets the notification widgets.
        /// </summary>
        /// <value>The notifications.</value>
        [XmlElement("Notification")]
        public List<Notification> Notifications { get; set; }

        /// <summary>
        /// Gets or sets the button widgets.
        /// </summary>
        /// <value>The buttons.</value>
        [XmlElement("Button")]
        public List<Button> Buttons { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Screens.Screen"/> class.
        /// </summary>
        public Screen()
        {
            Type = GetType();
            XmlPath = @"Screens/" + Type.ToString().Replace("Narivia.Screens.", "") + ".xml";

            Notifications = new List<Notification>();
            Buttons = new List<Button>();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public virtual void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
            ScreenManager.Instance.GraphicsDevice.Clear(BackgroundColour);

            Notifications.ForEach(x => x.LoadContent());
            Buttons.ForEach(x => x.LoadContent());
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            content.Unload();

            Notifications.ForEach(x => x.UnloadContent());
            Buttons.ForEach(x => x.UnloadContent());
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            Notifications.RemoveAll(x => x.Destroyed);
            Buttons.RemoveAll(x => x.Destroyed);

            Notifications.ForEach(x => x.Update(gameTime));
            Buttons.ForEach(x => x.Update(gameTime));
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            ScreenManager.Instance.GraphicsDevice.Clear(BackgroundColour);

            Notifications.ForEach(x => x.Draw(spriteBatch));
            Buttons.ForEach(x => x.Draw(spriteBatch));
        }

        public void ShowNotification(string text, NotificationType type, NotificationStyle style, Vector2 size)
        {
            Notification notification = new Notification
            {
                Text = text,
                Type = type,
                Style = style,
                Size = size
            };

            notification.LoadContent();

            Notifications.Add(notification);
        }
    }
}

