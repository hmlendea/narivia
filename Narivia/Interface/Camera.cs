using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Input;
using Narivia.Settings;

namespace Narivia.Interface
{
    public class Camera
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public Vector2 Size { get; set; }

        /// <summary>
        /// Gets or sets the velocity.
        /// </summary>
        /// <value>The velocity.</value>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Narivia.Entities.Camera"/>'s position has changed since it's last update.
        /// </summary>
        /// <value><c>true</c> if moved; otherwise, <c>false</c>.</value>
        public bool JustMoved { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.Entities.Camera"/> class.
        /// </summary>
        public Camera()
        {
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            Speed = 800;
            Size = SettingsManager.Instance.Resolution; // TODO: Give it it's proper size once the game HUD is implemented
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {

        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {

        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            Vector2 newVelocity = Velocity;

            if (InputManager.Instance.IsKeyDown(Keys.Up, Keys.W))
            {
                newVelocity.Y = -Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (InputManager.Instance.IsKeyDown(Keys.Down, Keys.S))
            {
                newVelocity.Y = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                newVelocity.Y = 0;
            }

            if (InputManager.Instance.IsKeyDown(Keys.Left, Keys.A))
            {
                newVelocity.X = -Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (InputManager.Instance.IsKeyDown(Keys.Right, Keys.D))
            {
                newVelocity.X = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                newVelocity.X = 0;
            }

            Velocity = newVelocity;

            Vector2 newPosition = new Vector2(
                (int)(Position.X + Velocity.X),
                (int)(Position.Y + Velocity.Y));

            if (Position != newPosition)
            {
                Position = newPosition;
                JustMoved = true;
            }
            else
            {
                JustMoved = false;
            }
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}

