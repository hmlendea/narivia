using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Input;
using Narivia.Input.Enumerations;

namespace Narivia.Interface.Widgets
{
    /// <summary>
    /// Menu item widget that cycles through the values of a list of strings
    /// </summary>
    public class MenuListSelector : MenuItem
    {
        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        public List<string> Values { get; set; }

        /// <summary>
        /// Gets the index of the selected value.
        /// </summary>
        /// <value>The index of the selected value.</value>
        [XmlIgnore]
        public int SelectedIndex { get; private set; }

        /// <summary>
        /// Gets the selected value.
        /// </summary>
        /// <value>The selected value.</value>
        [XmlIgnore]
        public string SelectedValue
        {
            get
            {
                if (Values.Count > 0 && Values.Count > SelectedIndex)
                {
                    return Values[SelectedIndex];
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the display text.
        /// </summary>
        /// <value>The display text.</value>
        public string DisplayText
        {
            get
            {
                string displayText = Text;

                if (!string.IsNullOrEmpty(SelectedValue))
                {
                    displayText += $" : {SelectedValue}";
                }

                return displayText;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuListSelector"/> class.
        /// </summary>
        public MenuListSelector()
        {
            Values = new List<string>();
            SelectedIndex = -1;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            if (Values.Count > 0)
            {
                SelectedIndex = 0;
            }
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

            if (Values.Count == 0)
            {
                return;
            }

            if (SelectedIndex == -1)
            {
                SelectedIndex = 0;
            }

            if ((InputManager.Instance.IsCursorInArea(ScreenArea) && InputManager.Instance.IsMouseButtonPressed(MouseButton.LeftButton)) ||
                InputManager.Instance.IsKeyPressed(Keys.Right))
            {
                SelectedIndex += 1;
            }
            if ((InputManager.Instance.IsCursorInArea(ScreenArea) && InputManager.Instance.IsMouseButtonPressed(MouseButton.RightButton)) ||
                InputManager.Instance.IsKeyPressed(Keys.Left))
            {
                SelectedIndex += 1;
            }

            if (SelectedIndex > Values.Count - 1)
            {
                SelectedIndex = 0;
            }
            else if (SelectedIndex < 0)
            {
                SelectedIndex = Values.Count - 1;
            }

            TextImage.Text = DisplayText;
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
