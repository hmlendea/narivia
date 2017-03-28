using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Input;
using Narivia.Settings;

namespace Narivia.Menus
{
    public class SettingsMenu : Menu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.Menus.SettingsMenu"/> class.
        /// </summary>
        public SettingsMenu() : base()
        {

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
            MenuItem selectedItem = Items[ItemNumber];

            if (InputManager.Instance.KeyPressed(Keys.Enter) &&
                selectedItem.LinkType == "Setting")
            {
                System.Console.WriteLine("IS SETTING");
                switch (selectedItem.LinkId)
                {
                    case "Fullscreen":
                        SettingsManager.Instance.Fullscreen = !SettingsManager.Instance.Fullscreen;

                        System.Console.WriteLine("TOGGLE FS");
                        break;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Items.ForEach(item => item.Image.Draw(spriteBatch));
        }
    }
}

