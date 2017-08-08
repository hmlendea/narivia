using System;
using System.IO;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Gui.GuiElements;
using Narivia.Gui.GuiElements.Enumerations;
using Narivia.Input;
using Narivia.Input.Events;

namespace Narivia.Gui.Screens
{
    /// <summary>
    /// Screen.
    /// </summary>
    public class Screen
    {
        public string Id { get; set; }

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
        /// Gets or sets the background colour.
        /// </summary>
        /// <value>The background colour.</value>
        public Color ForegroundColour { get; set; }

        /// <summary>
        /// Gets or sets the screen arguments.
        /// </summary>
        /// <value>The screen arguments.</value>
        public string[] ScreenArgs { get; set; }

        /// <summary>
        /// Occurs when the a kew is pressed while this <see cref="Screen"/> has input focus.
        /// </summary>
        public event KeyboardKeyEventHandler KeyPressed;

        /// <summary>
        /// Occurs when a mouse button is pressed.
        /// </summary>
        public event MouseButtonEventHandler MouseButtonPressed;

        /// <summary>
        /// Occurs when the mouse moved.
        /// </summary>
        public event MouseEventHandler MouseMoved;

        /// <summary>
        /// Initializes a new instance of the <see cref="Screen"/> class.
        /// </summary>
        public Screen()
        {
            Type = GetType();
            Id = Guid.NewGuid().ToString();

            XmlPath = Path.Combine("Screens", $"{Type.Name}.xml");
            
            BackgroundColour = Color.Black;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public virtual void LoadContent()
        {
            GraphicsManager.Instance.Graphics.GraphicsDevice.Clear(BackgroundColour);
            
            GuiManager.Instance.LoadContent();

            RegisterEvents();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            GuiManager.Instance.UnloadContent();

            UnregisterEvents();
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
        public void ShowNotification(string title, string text, NotificationType type, NotificationStyle style, Point size)
        {
            GuiNotificationDialog notification = new GuiNotificationDialog
            {
                Title = title,
                Text = text,
                Type = type,
                Style = style,
                Position = new Point((ScreenManager.Instance.Size.X - size.X) / 2,
                                     (ScreenManager.Instance.Size.Y - size.Y) / 2),
                Size = size
            };

            notification.LoadContent();

            GuiManager.Instance.GuiElements.Add(notification);
        }

        protected virtual void RegisterEvents()
        {
            InputManager.Instance.KeyboardKeyPressed += OnKeyPressed;

            InputManager.Instance.MouseButtonPressed += OnMouseButtonPressed;
            InputManager.Instance.MouseMoved += OnMouseMoved;
        }

        protected virtual void UnregisterEvents()
        {
            InputManager.Instance.KeyboardKeyPressed -= OnKeyPressed;

            InputManager.Instance.MouseButtonPressed -= OnMouseButtonPressed;
            InputManager.Instance.MouseMoved -= OnMouseMoved;
        }

        protected virtual void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            KeyPressed?.Invoke(sender, e);
        }

        protected virtual void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            MouseButtonPressed?.Invoke(sender, e);
        }

        protected virtual void OnMouseMoved(object sender, MouseEventArgs e)
        {
            MouseMoved?.Invoke(sender, e);
        }
    }
}

