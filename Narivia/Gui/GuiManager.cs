using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Gui.GuiElements;

namespace Narivia.Gui
{
    public class GuiManager
    {
        static volatile GuiManager instance;
        static object syncRoot = new object();

        /// <summary>
        /// Gets or sets the GUI elements.
        /// </summary>
        /// <value>The GUI elements.</value>
        public List<GuiElement> GuiElements { get; set; }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static GuiManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new GuiManager();
                        }
                    }
                }

                return instance;
            }
        }

        public GuiManager()
        {
            GuiElements = new List<GuiElement>();
        }


        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            GuiElements.ToList().ForEach(w => w.LoadContent());
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            GuiElements.ForEach(w => w.UnloadContent());
            GuiElements.Clear();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            GuiElements.RemoveAll(w => w.Destroyed);

            foreach (GuiElement guiElement in GuiElements.Where(w => w.Enabled))
            {
                guiElement.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (GuiElement guiElement in GuiElements.Where(w => w.Visible))
            {
                guiElement.Draw(spriteBatch);
            }
        }
    }
}
