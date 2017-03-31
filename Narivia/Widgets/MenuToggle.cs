using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Settings;

namespace Narivia.Widgets
{
    public class MenuToggle : MenuItem
    {
        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        /// <value>The type of the property.</value>
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the toggle state.
        /// </summary>
        /// <value>The type of the toggle state.</value>
        [XmlIgnore]
        public bool ToggleState
        {
            get
            {
                switch (Property)
                {
                    case "Fullscreen":
                        return SettingsManager.Instance.Fullscreen;
                }

                return false;
            }
            set
            {
                switch (Property)
                {
                    case "Fullscreen":
                        SettingsManager.Instance.Fullscreen = value;
                        break;
                }
            }
        }

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

            string displayText = Text + ": " + ToggleState;

            if (Image.Text != displayText)
            {
                // TODO: Update the text
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

        /// <summary>
        /// Activates this item.
        /// </summary>
        public override void Activate()
        {
            base.Activate();

            ToggleState = !ToggleState;
        }
    }
}
