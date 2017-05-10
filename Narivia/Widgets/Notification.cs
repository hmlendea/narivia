using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;

namespace Narivia.Widgets
{
    public class Notification : Widget
    {
        public NotificationType NotificationType { get; set; }

        public Vector2 Size { get; set; }

        Image[,] images;

        public Notification()
        {
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            string imagePath;
            images = new Image[(int)Size.X, (int)Size.Y];

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

            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    Image image = new Image();
                    image.ImagePath = imagePath;
                    image.Position = new Vector2(Position.X + x * 32, Position.Y + y * 32);

                    int sx = 1;
                    int sy = 1;

                    if (Size.X == 1)
                    {
                        sx = 3;
                    }
                    else if (x == 0)
                    {
                        sx = 0;
                    }
                    else if (x == Size.X - 1)
                    {
                        sx = 2;
                    }

                    if (Size.Y == 1)
                    {
                        sy = 3;
                    }
                    else if (y == 0)
                    {
                        sy = 0;
                    }
                    else if (y == Size.Y - 1)
                    {
                        sy = 2;
                    }


                    image.SourceRectangle = new Rectangle(sx * 32, sy * 32, 32, 32); // TODO: Select the right one

                    images[x, y] = image;
                    image.LoadContent();
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
    }
}
