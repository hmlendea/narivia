using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Gui.GuiElements;
using Narivia.Gui.GuiElements.Enumerations;
using Narivia.Infrastructure.Extensions;

namespace Narivia.Gui.Screens
{
    /// <summary>
    /// Screen.
    /// </summary>
    public class Screen
    {
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
        /// Gets or sets the notification GUI elements.
        /// </summary>
        /// <value>The notifications.</value>
        [XmlElement("Notification")]
        public List<GuiNotificationDialog> Notifications { get; set; }

        /// <summary>
        /// Gets or sets the button GUI elements.
        /// </summary>
        /// <value>The buttons.</value>
        [XmlElement("Button")]
        public List<GuiButton> Buttons { get; set; }

        /// <summary>
        /// Gets or sets the screen arguments.
        /// </summary>
        /// <value>The screen arguments.</value>
        public string[] ScreenArgs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Screen"/> class.
        /// </summary>
        public Screen()
        {
            Type = GetType();

            XmlPath = Path.Combine("Screens", $"{Type.GetTypeNameWithoutNamespace()}.xml");

            Notifications = new List<GuiNotificationDialog>();
            Buttons = new List<GuiButton>();

            BackgroundColour = Color.Black;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public virtual void LoadContent()
        {
            GraphicsManager.Instance.Graphics.GraphicsDevice.Clear(BackgroundColour);

            GuiManager.Instance.GuiElements.AddRange(Notifications);
            GuiManager.Instance.GuiElements.AddRange(Buttons);

            GuiManager.Instance.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            GuiManager.Instance.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            GuiManager.Instance.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            GraphicsManager.Instance.Graphics.GraphicsDevice.Clear(BackgroundColour);
            GuiManager.Instance.Draw(spriteBatch);
        }

        /// <summary>
        /// Shows a notification.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="type">Type.</param>
        /// <param name="style">Style.</param>
        /// <param name="size">Size.</param>
        public void ShowNotification(string title, string text, NotificationType type, NotificationStyle style, Vector2 size)
        {
            GuiNotificationDialog notification = new GuiNotificationDialog
            {
                Title = title,
                Text = text,
                Type = type,
                Style = style,
                Position = (ScreenManager.Instance.Size - size) / 2,
                Size = size
            };

            notification.LoadContent();

            GuiManager.Instance.GuiElements.Add(notification);
        }
    }
}

