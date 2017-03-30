using Microsoft.Xna.Framework;

namespace Narivia.Graphics.ImageEffects
{
    /// <summary>
    /// Zoom effect.
    /// </summary>
    public class ZoomEffect : ImageEffect
    {
        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the minimum zoom.
        /// </summary>
        /// <value>The minimum zoom.</value>
        public float MinimumZoom { get; set; }

        /// <summary>
        /// Gets or sets the maximum zoom.
        /// </summary>
        /// <value>The maximum zoom.</value>
        public float MaximumZoom { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Narivia.Graphics.ImageEffects.ZoomEffect"/> is increasing.
        /// </summary>
        /// <value><c>true</c> if increasing; otherwise, <c>false</c>.</value>
        public bool Increasing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Graphics.ImageEffects.ZoomEffect"/> class.
        /// </summary>
        public ZoomEffect()
        {
            Speed = 0.5f;
            MinimumZoom = 0.75f;
            MaximumZoom = 1.25f;
            Increasing = false;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="image">Image.</param>
        public override void LoadContent(ref Image image)
        {
            base.LoadContent(ref image);
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Image.Active)
            {
                if (Increasing == false)
                {
                    Image.Zoom -= Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                else
                {
                    Image.Zoom += Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                }

                if (Image.Zoom < MinimumZoom)
                {
                    Increasing = true;
                    Image.Zoom = MinimumZoom;
                }
                else if (Image.Zoom > MaximumZoom)
                {
                    Increasing = false;
                    Image.Zoom = MaximumZoom;
                }
            }
            else
            {
                Image.Zoom = MaximumZoom;
            }
        }
    }
}
