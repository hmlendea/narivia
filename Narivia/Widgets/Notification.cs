using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;

namespace Narivia.Widgets
{
    public class Notification : Widget
    {
        public NotificationType NotificationType { get; set; }

        public Vector2 NotificationSize
        {
            get { return size; }
            set
            {
                size = new Vector2(Math.Max(1, (float)Math.Round(value.X)),
                                   Math.Max(1, (float)Math.Round(value.Y)));
            }
        }

        Vector2 size;
        Image[,] images;
        
        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            string imagePath;
            images = new Image[(int)NotificationSize.X, (int)NotificationSize.Y];

            switch (NotificationType)
            {
                default:
                case NotificationType.Big:
                    imagePath = "Interface/notification_big";
                    break;

                case NotificationType.Small:
                    imagePath = "Interface/notification_small";
                    break;
            }

            for (int y = 0; y < NotificationSize.Y; y++)
            {
                for (int x = 0; x < NotificationSize.X; x++)
                {
                    images[x, y] = new Image
                    {
                        ImagePath = imagePath,
                        Position = new Vector2(Position.X + x * 32, Position.Y + y * 32),
                        SourceRectangle = CalculateSourceRectangle(x, y)
                    };

                    images[x, y].LoadContent();
                }
            }
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

            return new Rectangle(sx * 32, sy * 32, 32, 32);
        }
    }
}
