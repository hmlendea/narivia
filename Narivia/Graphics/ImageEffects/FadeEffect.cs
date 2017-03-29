using Microsoft.Xna.Framework;

namespace Narivia.Graphics.ImageEffects
{
    /// <summary>
    /// Fade effect.
    /// </summary>
    public class FadeEffect : ImageEffect
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
        /// Gets or sets a value indicating whether this <see cref="Narivia.Graphics.ImageEffects.FadeEffect"/> is increasing.
        /// </summary>
        /// <value><c>true</c> if increasing; otherwise, <c>false</c>.</value>
        public bool Increasing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Graphics.ImageEffects.FadeEffect"/> class.
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
                    Image.Opacity -= Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                else
                {
                    Image.Opacity += Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                }

                if (Image.Opacity < MinimumOpacity)
                {
                    Increasing = true;
                    Image.Opacity = MinimumOpacity;
                }
                else if (Image.Opacity > MaximumOpacity)
                {
                    Increasing = false;
                    Image.Opacity = MaximumOpacity;
                }
            }
            else
            {
                Image.Opacity = MaximumOpacity;
            }
        }
    }
}
