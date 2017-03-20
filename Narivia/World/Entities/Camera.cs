using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Input;

namespace Narivia.WorldMap.Entities
{
    public class Camera
    {
        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        public float MovementSpeed { get; set; }

        public Camera()
        {
            Velocity = Vector2.Zero;
            MovementSpeed = 100;
        }

        public void LoadContent()
        {

        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            //if ((int)Velocity.X == 0)
            {
                if (InputManager.Instance.KeyDown(Keys.Up, Keys.W))
                {
                    Velocity = new Vector2(Velocity.X, -MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                else if (InputManager.Instance.KeyDown(Keys.Down, Keys.S))
                {
                    Velocity = new Vector2(Velocity.X, MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                else
                {
                    Velocity = new Vector2(Velocity.X, 0);
                }
            }

            //if ((int)Velocity.Y == 0)
            {
                if (InputManager.Instance.KeyDown(Keys.Left, Keys.A))
                {
                    Velocity = new Vector2(-MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, Velocity.Y);
                }
                else if (InputManager.Instance.KeyDown(Keys.Right, Keys.D))
                {
                    Velocity = new Vector2(MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, Velocity.Y);
                }
                else
                {
                    Velocity = new Vector2(0, Velocity.Y);
                }
            }

            Position += Velocity;

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}

