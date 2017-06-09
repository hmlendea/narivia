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
        public new Vector2 Size { get { return new Vector2(tileSize, tileSize); } }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public NotificationIcon Icon { get; set; }

        // TODO: Maybe implement my own handler and args
        public event EventHandler Clicked;

        const int tileSize = 32;

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
                SourceRectangle = new Rectangle(tileSize * 3, tileSize * 3, tileSize, tileSize),
                Position = Position
            };

            icon = new Image
            {
                ImagePath = "Interface/notification_icons",
                SourceRectangle = CalculateIconSourceRectangle(Icon),
                Position = Position
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
            icon.LoadContent();

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

        Rectangle CalculateIconSourceRectangle(NotificationIcon icon)
        {
            // TODO: Actually do something useful
            return new Rectangle(0 * tileSize, 0 * tileSize, tileSize, tileSize);
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
        }
    }
}
