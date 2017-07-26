using Microsoft.Xna.Framework;

namespace Narivia.Graphics.SpriteEffects
{
    /// <summary>
    /// Rotation sprite effect.
    /// </summary>
    public class RotationEffect : CustomSpriteEffect
    {
        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the maximum rotation.
        /// </summary>
        /// <value>The maximum rotation.</value>
        public float MaximumRotation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RotationEffect"/> is increasing.
        /// </summary>
        /// <value><c>true</c> if increasing; otherwise, <c>false</c>.</value>
        public bool Increasing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RotationEffect"/> class.
        /// </summary>
        public RotationEffect()
        {
            Speed = 0.5f;
            MaximumRotation = 1.0f;
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
                    Sprite.Rotation -= Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                else
                {
                    Sprite.Rotation += Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                }

                if (Sprite.Rotation < -MaximumRotation)
                {
                    Increasing = true;
                    Sprite.Rotation = -MaximumRotation;
                }
                else if (Sprite.Rotation > MaximumRotation)
                {
                    Increasing = false;
                    Sprite.Rotation = MaximumRotation;
                }
            }
            else
            {
                Sprite.Rotation = MaximumRotation;
            }
        }
    }
}
