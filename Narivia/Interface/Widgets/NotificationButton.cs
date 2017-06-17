using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Audio;
using Narivia.Graphics;
using Narivia.Input;
using Narivia.Input.Enumerations;
using Narivia.Input.Events;
using Narivia.Interface.Widgets.Enumerations;

namespace Narivia.Interface.Widgets
{
    /// <summary>
    /// Notification button widget.
    /// </summary>
    public class NotificationButton : Widget
    {
        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public override Vector2 Size { get { return new Vector2(32, 32); } }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public NotificationIcon Icon { get; set; }

        Image background;
        Image icon;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            background = new Image
            {
                ImagePath = "Interface/notification_small",
                SourceRectangle = new Rectangle(96, 96, 32, 32)
            };

            icon = new Image
            {
                ImagePath = "Interface/notification_icons",
                SourceRectangle = CalculateIconSourceRectangle(Icon)
            };

            background.LoadContent();
            icon.LoadContent();

            base.LoadContent();

            InputManager.Instance.MouseButtonPressed += InputManager_OnMouseButtonPressed;
            InputManager.Instance.MouseMoved += InputManager_OnMouseMoved;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            background.UnloadContent();
            icon.UnloadContent();

            base.UnloadContent();

            InputManager.Instance.MouseButtonPressed -= InputManager_OnMouseButtonPressed;
            InputManager.Instance.MouseMoved -= InputManager_OnMouseMoved;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }

            background.Position = Position;
            icon.Position = new Vector2(Position.X + 6, Position.Y + 6);

            background.Update(gameTime);
            icon.Update(gameTime);

            CheckForInput();

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }

            background.Draw(spriteBatch);
            icon.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        Rectangle CalculateIconSourceRectangle(NotificationIcon notificationIcon)
        {
            // TODO: Actually do something useful
            return new Rectangle((int)notificationIcon % 8 * 20,
                                 (int)notificationIcon / 8 * 20,
                                 20,
                                 20);
        }

        void CheckForInput()
        {
        }

        /// <summary>
        /// Fire by the Clicked event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void OnClicked(object sender, MouseButtonEventArgs e)
        {
            base.OnClicked(sender, e);

            Destroy();
        }

        void InputManager_OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (!ScreenArea.Contains(e.MousePosition) || e.Button != MouseButton.LeftButton)
            {
                return;
            }

            AudioManager.Instance.PlaySound("Interface/click");
        }

        void InputManager_OnMouseMoved(object sender, MouseEventArgs e)
        {
            if (ScreenArea.Contains(e.CurrentMousePosition) && !ScreenArea.Contains(e.PreviousMousePosition))
            {
                AudioManager.Instance.PlaySound("Interface/select");
            }
        }
    }
}
