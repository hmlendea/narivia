using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Graphics;
using Narivia.Input;

namespace Narivia.Widgets
{
    public class Notification : Widget
    {
        public NotificationType NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the size of the notification.
        /// </summary>
        /// <value>The size of the notification.</value>
        public Vector2 NotificationSize
        {
            get { return size; }
            set
            {
                size = new Vector2(Math.Max(1, (float)Math.Round(value.X)),
                                   Math.Max(1, (float)Math.Round(value.Y)));
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

        Vector2 size;
        Image[,] images;
        Image textImage;

        const int tileSize = 32;

        public Notification()
        {
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

            switch (NotificationType)
            {
                default:
                case NotificationType.Big:
                    imagePath = "Interface/notification_big";
                    fontName = "NotificationFontBig";
                    break;

                case NotificationType.Small:
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

            textImage.LoadContent();
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
