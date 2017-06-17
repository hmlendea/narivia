using System;

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
    /// Notification widget.
    /// </summary>
    public class Notification : Widget
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public NotificationType Type { get; set; }

        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        /// <value>The style.</value>
        public NotificationStyle Style { get; set; }

        /// <summary>
        /// Gets or sets the size of the notification.
        /// </summary>
        /// <value>The size of the notification.</value>
        public Vector2 NotificationSize
        {
            get
            {
                return new Vector2((int)Math.Round(Size.X / tileSize),
                                   (int)Math.Round(Size.Y / tileSize));
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the text colour.
        /// </summary>
        /// <value>The text colour.</value>
        public Color TextColour { get; set; }

        Image[,] images;
        Image titleImage;
        Image textImage;
        Image yesButtonImage;
        Image noButtonImage;

        const int tileSize = 32;

        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        public Notification()
        {
            Type = NotificationType.Informational;
            Style = NotificationStyle.Big;
            TextColour = Color.Black;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            string imagePath, fontName;

            images = new Image[(int)NotificationSize.X, (int)NotificationSize.Y];
            titleImage = new Image();
            textImage = new Image();

            switch (Style)
            {
                default:
                case NotificationStyle.Big:
                    imagePath = "Interface/notification_big";
                    fontName = "NotificationFontBig";
                    break;

                case NotificationStyle.Small:
                    imagePath = "Interface/notification_small";
                    fontName = "NotificationFontSmall";
                    break;
            }

            for (int y = 0; y < NotificationSize.Y; y++)
            {
                for (int x = 0; x < NotificationSize.X; x++)
                {
                    images[x, y] = new Image
                    {
                        ImagePath = imagePath,
                        FontName = fontName,
                        Position = new Vector2(Position.X + x * tileSize, Position.Y + y * tileSize),
                        SourceRectangle = CalculateSourceRectangle(x, y)
                    };

                    images[x, y].LoadContent();
                }
            }

            titleImage.Text = Title;
            titleImage.TextVerticalAlignment = VerticalAlignment.Center;
            titleImage.Tint = TextColour;
            titleImage.FontName = "NotificationTitleFontBig";
            titleImage.Position = new Vector2(Position.X, Position.Y + tileSize);
            titleImage.SpriteSize = new Vector2(NotificationSize.X * tileSize, tileSize);

            textImage.Text = Text;
            textImage.TextVerticalAlignment = VerticalAlignment.Center;
            textImage.Tint = TextColour;
            textImage.FontName = fontName;
            textImage.Position = new Vector2(Position.X + tileSize / 2, Position.Y + tileSize * 2.5f);
            textImage.SpriteSize = new Vector2(Size.X - tileSize, Size.Y - titleImage.SpriteSize.Y - tileSize);

            yesButtonImage = new Image
            {
                ImagePath = "Interface/notification_controls",
                SourceRectangle = new Rectangle(0, 0, tileSize, tileSize),
                Position = new Vector2(Position.X + (NotificationSize.X - 1) * tileSize, Position.Y)
            };

            if (Type == NotificationType.Interogative)
            {
                noButtonImage = new Image
                {
                    ImagePath = "Interface/notification_controls",
                    SourceRectangle = new Rectangle(tileSize, 0, tileSize, tileSize),
                    Position = new Vector2(Position.X, Position.Y)
                };

                noButtonImage.LoadContent();
            }

            titleImage.LoadContent();
            textImage.LoadContent();
            yesButtonImage.LoadContent();

            base.LoadContent();

            InputManager.Instance.MouseButtonPressed += InputManager_OnMouseButtonPressed;
            InputManager.Instance.MouseMoved += InputManager_OnMouseMoved;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            foreach (Image image in images)
            {
                image.UnloadContent();
            }

            titleImage.UnloadContent();
            textImage.UnloadContent();
            yesButtonImage.UnloadContent();

            if (Type == NotificationType.Interogative)
            {
                noButtonImage.UnloadContent();
            }

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
            foreach (Image image in images)
            {
                image.Update(gameTime);
            }

            titleImage.Update(gameTime);
            textImage.Update(gameTime);
            yesButtonImage.Update(gameTime);

            if (Type == NotificationType.Interogative)
            {
                noButtonImage.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Image image in images)
            {
                image.Draw(spriteBatch);
            }

            titleImage.Draw(spriteBatch);
            textImage.Draw(spriteBatch);
            yesButtonImage.Draw(spriteBatch);

            if (Type == NotificationType.Interogative)
            {
                noButtonImage.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }

        Rectangle CalculateSourceRectangle(int x, int y)
        {
            int sx = 1;
            int sy = 1;

            if ((int)NotificationSize.X == 1)
            {
                sx = 3;
            }
            else if (x == 0)
            {
                sx = 0;
            }
            else if (x == (int)NotificationSize.X - 1)
            {
                sx = 2;
            }

            if ((int)NotificationSize.Y == 1)
            {
                sy = 3;
            }
            else if (y == 0)
            {
                sy = 0;
            }
            else if (y == (int)NotificationSize.Y - 1)
            {
                sy = 2;
            }

            return new Rectangle(sx * tileSize, sy * tileSize, tileSize, tileSize);
        }

        void InputManager_OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (yesButtonImage.ScreenArea.Contains(e.MousePosition) && e.Button == MouseButton.LeftButton)
            {
                AudioManager.Instance.PlaySound("Interface/click");

                Destroy();
            }

            if (Type == NotificationType.Interogative &&
                noButtonImage.ScreenArea.Contains(e.MousePosition) &&
                e.Button == MouseButton.LeftButton)
            {
                AudioManager.Instance.PlaySound("Interface/click");

                Destroy();
            }
        }

        void InputManager_OnMouseMoved(object sender, MouseEventArgs e)
        {
            if (yesButtonImage.ScreenArea.Contains(e.CurrentMousePosition) && !yesButtonImage.ScreenArea.Contains(e.PreviousMousePosition))
            {
                AudioManager.Instance.PlaySound("Interface/select");
            }

            if (Type == NotificationType.Interogative &&
                noButtonImage.ScreenArea.Contains(e.CurrentMousePosition) && !noButtonImage.ScreenArea.Contains(e.PreviousMousePosition))
            {
                AudioManager.Instance.PlaySound("Interface/select");
            }
        }
    }
}
