using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;

namespace Narivia.Widgets
{
    public class Notification : Widget
    {
        public NotificationType Type { get; set; }

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
        Image textImage;
        Image yesButtonImage;
        Image noButtonImage;

        const int tileSize = 32;

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

            textImage.Text = Text;
            textImage.Tint = TextColour;
            textImage.FontName = fontName;
            textImage.Position = new Vector2(Position.X + tileSize * 0.5f,
                                             Position.Y + tileSize * 1.0f);
            textImage.Size = Size - textImage.Position * 2;

            yesButtonImage = new Image
            {
                ImagePath = "Interface/notification_icons",
                SourceRectangle = new Rectangle(0, 0, tileSize, tileSize),
                Position = new Vector2(this.Position.X + (NotificationSize.X - 1) * tileSize, Position.Y)
            };

            if (Type == NotificationType.Interogative)
            {
                noButtonImage = new Image
                {
                    ImagePath = "Interface/notification_icons",
                    SourceRectangle = new Rectangle(tileSize, 0, tileSize, tileSize),
                    Position = new Vector2(this.Position.X, Position.Y)
                };

                noButtonImage.LoadContent();
            }

            textImage.LoadContent();
            yesButtonImage.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            foreach(Image image in images)
            {
                image.UnloadContent();
            }

            textImage.UnloadContent();
            yesButtonImage.UnloadContent();

            if (Type == NotificationType.Interogative)
            {
                noButtonImage.UnloadContent();
            }
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

            textImage.Update(gameTime);
            yesButtonImage.Update(gameTime);

            if (Type == NotificationType.Interogative)
            {
                noButtonImage.Update(gameTime);
            }
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

            textImage.Draw(spriteBatch);
            yesButtonImage.Draw(spriteBatch);

            if (Type == NotificationType.Interogative)
            {
                noButtonImage.Draw(spriteBatch);
            }
        }

        Rectangle CalculateSourceRectangle(int x, int y)
        {
            int sx = 1;
            int sy = 1;

            if (NotificationSize.X == 1)
            {
                sx = 3;
            }
            else if (x == 0)
            {
                sx = 0;
            }
            else if (x == NotificationSize.X - 1)
            {
                sx = 2;
            }

            if (NotificationSize.Y == 1)
            {
                sy = 3;
            }
            else if (y == 0)
            {
                sy = 0;
            }
            else if (y == NotificationSize.Y - 1)
            {
                sy = 2;
            }

            return new Rectangle(sx * tileSize, sy * tileSize, tileSize, tileSize);
        }
    }
}
