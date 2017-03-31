using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Screens;

namespace Narivia.Widgets
{
    /// <summary>
    /// Menu link widget
    /// </summary>
    public class MenuLink : MenuItem
    {
        /// <summary>
        /// Gets or sets the link identifier.
        /// </summary>
        /// <value>The link identifier.</value>
        public string LinkId { get; set; }
        
        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
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
            if (!Enabled)
            {
                return;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }

            base.Draw(spriteBatch);
        }

        /// <summary>
        /// Activates this link.
        /// </summary>
        public override void Activate()
        {
            base.Activate();

            ScreenManager.Instance.ChangeScreens(LinkId);
        }
    }
}
