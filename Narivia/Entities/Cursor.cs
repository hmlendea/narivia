using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Graphics;
using Narivia.Graphics.ImageEffects;
using Narivia.Input;

namespace Narivia.Entities
{
    public class Cursor
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public Vector2 Position { get; private set; }

        Image image;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.Entities.Cursor"/> class.
        /// </summary>
        public Cursor()
        {
            image = new Image
            {
                ImagePath = "Cursors/idle",
                Effects = "AnimationEffect",
                Active = true
            };

            image.AnimationEffect = new AnimationEffect
            {
                FrameAmount = new Vector2(8, 1),
                SwitchFrame = 150
            };
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            image.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            image.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            image.Position = InputManager.Instance.MousePosition;
            image.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            image.Draw(spriteBatch);
        }
    }
}
