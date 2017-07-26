using Microsoft.Xna.Framework;

namespace Narivia.Graphics.SpriteEffects
{
    /// <summary>
    /// Fade effect.
    /// </summary>
    public class FadeEffect : CustomSpriteEffect
    {
        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the minimum opacity.
        /// </summary>
        /// <value>The minimum opacity.</value>
        public float MinimumOpacity { get; set; }

        /// <summary>
        /// Gets or sets the maximum opacity.
        /// </summary>
        /// <value>The maximum opacity.</value>
        public float MaximumOpacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FadeEffect"/> is increasing.
        /// </summary>
        /// <value><c>true</c> if increasing; otherwise, <c>false</c>.</value>
        public bool Increasing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FadeEffect"/> class.
        /// </summary>
        public FadeEffect()
        {
            Speed = 1;
            MinimumOpacity = 0.0f;
            MaximumOpacity = 1.0f;
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
                    Sprite.Opacity -= Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                else
                {
                    Sprite.Opacity += Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                }

                if (Sprite.Opacity < MinimumOpacity)
                {
                    Increasing = true;
                    Sprite.Opacity = MinimumOpacity;
                }
                else if (Sprite.Opacity > MaximumOpacity)
                {
                    Increasing = false;
                    Sprite.Opacity = MaximumOpacity;
                }
            }
            else
            {
                Sprite.Opacity = MaximumOpacity;
            }
        }
    }
}
