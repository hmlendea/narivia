using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Audio;
using Narivia.Graphics;
using Narivia.Input;

namespace Narivia.Interface.Widgets
{
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

        // TODO: Maybe implement my own handler and args
        public event EventHandler Clicked;

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
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            background.UnloadContent();
            icon.UnloadContent();

            base.UnloadContent();
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
            if (InputManager.Instance.IsCursorEnteringArea(ScreenArea))
            {
                AudioManager.Instance.PlaySound("Interface/select");
            }

            if (InputManager.Instance.IsCursorInArea(ScreenArea) &&
                InputManager.Instance.IsMouseButtonPressed(MouseButton.LeftButton))
            {
                AudioManager.Instance.PlaySound("Interface/click");
                OnClicked(this, null);
            }
        }

        protected void OnClicked(object sender, EventArgs e)
        {
            if (Clicked != null)
            {
                Clicked(this, null);
            }

            Destroy();
        }
    }
}
