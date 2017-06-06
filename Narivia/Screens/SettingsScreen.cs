using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Interface.Widgets;
using Narivia.Settings;

namespace Narivia.Screens
{
    public class SettingsScreen : MenuScreen
    {
        MenuToggle fullScreenToggle;
        MenuToggle debugModeToggle;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            fullScreenToggle = Toggles.FirstOrDefault(t => t.Property == "Fullscreen");
            debugModeToggle = Toggles.FirstOrDefault(t => t.Property == "DebugMode");

            fullScreenToggle.ToggleState = SettingsManager.Instance.Fullscreen;
            debugModeToggle.ToggleState = SettingsManager.Instance.DebugMode;

            LinkEventsToToggles();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            SettingsManager.Instance.SaveContent();
            base.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        protected void fullScreenToggle_OnActivated(object sender, EventArgs e)
        {
            SettingsManager.Instance.Fullscreen = fullScreenToggle.ToggleState;
        }

        protected void debugModeToggle_OnActivated(object sender, EventArgs e)
        {
            SettingsManager.Instance.DebugMode = debugModeToggle.ToggleState;
        }

        void LinkEventsToToggles()
        {
            if (fullScreenToggle != null)
            {
                fullScreenToggle.Activated += fullScreenToggle_OnActivated;
            }

            if (debugModeToggle != null)
            {
                debugModeToggle.Activated += debugModeToggle_OnActivated;
            }
        }
    }
}
