using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Narivia.Interface.Widgets
{
    public class MenuAction : MenuItem
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The type of the action.</value>
        public string ActionId { get; set; }

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
        /// <returns>The draw.</returns>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Enabled)
            {
                return;
            }

            base.Draw(spriteBatch);
        }

        protected override void OnActivated(object sender, EventArgs e)
        {
            base.OnActivated(sender, e);

            switch (ActionId)
            {
                case "Exit":
                    Program.Game.Exit();
                    break;
            }
        }
    }
}
