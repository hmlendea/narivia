using Microsoft.Xna.Framework;

namespace Narivia.Graphics.SpriteEffects
{
    /// <summary>
    /// Zoom effect.
    /// </summary>
    public class ZoomEffect : CustomSpriteEffect
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
        /// Gets or sets a value indicating whether this <see cref="ZoomEffect"/> is increasing.
        /// </summary>
        /// <value><c>true</c> if increasing; otherwise, <c>false</c>.</value>
        public bool Increasing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomEffect"/> class.
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
        /// <param name="sprite">Sprite.</param>
        public override void LoadContent(ref Sprite sprite)
        {
            base.LoadContent(ref sprite);
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

            if (Sprite.Active)
            {
                if (Increasing == false)
                {
                    Sprite.Zoom -= Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                else
                {
                    Sprite.Zoom += Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                }

                if (Sprite.Zoom < MinimumZoom)
                {
                    Increasing = true;
                    Sprite.Zoom = MinimumZoom;
                }
                else if (Sprite.Zoom > MaximumZoom)
                {
                    Increasing = false;
                    Sprite.Zoom = MaximumZoom;
                }
            }
            else
            {
                Sprite.Zoom = MaximumZoom;
            }
        }
    }
}
