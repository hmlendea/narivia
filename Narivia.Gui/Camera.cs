using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NuciXNA.Input;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;

using Narivia.Settings;

namespace Narivia.Gui
{
    public class Camera
    {
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Point2D Location { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public Size2D Size { get; set; }

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
        /// Gets or sets a value indicating whether this <see cref="Camera"/>'s location has changed since it's last update.
        /// </summary>
        /// <value><c>true</c> if moved; otherwise, <c>false</c>.</value>
        public bool JustMoved { get; set; }

        int directionY;
        int directionX;

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
        public Camera()
        {
            Location = Point2D.Empty;
            Velocity = Vector2.Zero;
            Speed = 800;
            Size = SettingsManager.Instance.GraphicsSettings.Resolution; // TODO: Give it it's proper size once the game HUD is implemented
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            InputManager.Instance.KeyboardKeyDown += InputManager_OnKeyboardKeyDown;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            InputManager.Instance.KeyboardKeyDown -= InputManager_OnKeyboardKeyDown;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            Vector2 newVelocity = Velocity;

            if (directionY == -1)
            {
                newVelocity.Y = -Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (directionY == 1)
            {
                newVelocity.Y = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                newVelocity.Y = 0;
            }

            if (directionX == -1)
            {
                newVelocity.X = -Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (directionX == 1)
            {
                newVelocity.X = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                newVelocity.X = 0;
            }

            Velocity = newVelocity;

            Point2D newLocation = new Point2D((int)(Location.X + Velocity.X),
                                              (int)(Location.Y + Velocity.Y));

            if (Location != newLocation)
            {
                Location = newLocation;
                JustMoved = true;
            }
            else
            {
                JustMoved = false;
            }

            directionX = 0;
            directionY = 0;
        }

        /// <summary>
        /// Centres the camera on the specified location.
        /// </summary>
        public void CentreOnLocation(Point2D location)
        {
            Location = new Point2D(location.X - Size.Width / 2, location.Y - Size.Height / 2);
        }

        void InputManager_OnKeyboardKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Up || e.Key == Keys.W)
            {
                directionY = -1;
            }
            else if (e.Key == Keys.Down || e.Key == Keys.S)
            {
                directionY = 1;
            }

            if (e.Key == Keys.Left || e.Key == Keys.A)
            {
                directionX = -1;
            }
            else if (e.Key == Keys.Right || e.Key == Keys.D)
            {
                directionX = 1;
            }
        }
    }
}

