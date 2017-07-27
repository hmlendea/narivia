using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Helpers;
using Narivia.Resources;
using Narivia.Settings;

namespace Narivia.Gui
{
    /// <summary>
    /// FPS indicator.
    /// </summary>
    public class FpsIndicator
    {
        GameTime gameTime;
        SpriteFont fpsFont;
        Vector2 fpsCounterSize;
        string fpsString;

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FpsIndicator"/> class.
        /// </summary>
        public FpsIndicator()
        {
            Position = Vector2.Zero;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            fpsFont = ResourceManager.Instance.LoadSpriteFont("Fonts/FrameCounterFont");
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
            this.gameTime = gameTime;

            fpsString = $"FPS: {Math.Round(FramerateCounter.Instance.AverageFramesPerSecond)}";
            fpsCounterSize = fpsFont.MeasureString(fpsString);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            FramerateCounter.Instance.Update(deltaTime);

            if (SettingsManager.Instance.DebugMode)
            {
                spriteBatch.DrawString(fpsFont, fpsString, new Vector2(1, 1), Color.Lime);
            }
        }
    }
}
