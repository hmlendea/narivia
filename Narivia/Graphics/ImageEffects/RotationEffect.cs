using Microsoft.Xna.Framework;

namespace Narivia.Graphics.ImageEffects
{
    /// <summary>
    /// Rotation effect.
    /// </summary>
    public class RotationEffect : ImageEffect
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
        /// Gets or sets a value indicating whether this <see cref="Narivia.Graphics.ImageEffects.RotationEffect"/> is increasing.
        /// </summary>
        /// <value><c>true</c> if increasing; otherwise, <c>false</c>.</value>
        public bool Increasing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Graphics.ImageEffects.RotationEffect"/> class.
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
                    Image.Rotation -= Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                else
                {
                    Image.Rotation += Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                }

                if (Image.Rotation < -MaximumRotation)
                {
                    Increasing = true;
                    Image.Rotation = -MaximumRotation;
                }
                else if (Image.Rotation > MaximumRotation)
                {
                    Increasing = false;
                    Image.Rotation = MaximumRotation;
                }
            }
            else
            {
                Image.Rotation = MaximumRotation;
            }
        }
    }
}
