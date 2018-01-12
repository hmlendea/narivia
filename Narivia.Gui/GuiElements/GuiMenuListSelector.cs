using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NuciXNA.Primitives.Mapping;

using Narivia.Input.Enumerations;
using Narivia.Input.Events;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Menu item GUI element that cycles through the values of a list of strings
    /// </summary>
    public class GuiMenuListSelector : GuiMenuItem
    {
        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        public List<string> Values { get; set; }

        /// <summary>
        /// Gets the selected index.
        /// </summary>
        /// <value>The selected index.</value>
        [XmlIgnore]
        public int SelectedIndex { get; set; }

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
        /// Occurs when the selected index was changed.
        /// </summary>
        public event EventHandler SelectedIndexChanged;

        /// <summary>
        /// Occurs when the selected value was changed.
        /// </summary>
        public event EventHandler SelectedValueChanged;
        
        int lastSelectedIndex;
        string lastSelectedValue;
        string originalText;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiMenuListSelector"/> class.
        /// </summary>
        public GuiMenuListSelector()
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

            lastSelectedIndex = SelectedIndex;
            lastSelectedValue = SelectedValue;

            originalText = Text;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            Text = originalText;

            if (Values.Count == 0)
            {
                return;
            }

            if (SelectedIndex >= Values.Count)
            {
                SelectedIndex = 0;
            }
            else if (SelectedIndex < 0)
            {
                SelectedIndex = Values.Count - 1;
            }

            Text += $" : {SelectedValue}";

            if (SelectedIndex != lastSelectedIndex)
            {
                OnSelectedIndexChanged(this, null);
            }

            if (SelectedValue != lastSelectedValue)
            {
                OnSelectedValueChanged(this, null);
            }

            lastSelectedIndex = SelectedIndex;
            lastSelectedValue = SelectedValue;

            base.Update(gameTime);
        }

        /// <summary>
        /// Fires when a keyboard key was pressed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Right || e.Key == Keys.D)
            {
                SelectedIndex += 1;
            }
            else if (e.Key == Keys.Left || e.Key == Keys.A)
            {
                SelectedIndex -= 1;
            }
        }

        protected override void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseButtonPressed(sender, e);

            if (!ClientRectangle.Contains(e.Location.ToPoint2D()))
            {
                return;
            }

            if (e.Button == MouseButton.LeftButton)
            {
                SelectedIndex += 1;
            }
            else if (e.Button == MouseButton.RightButton)
            {
                SelectedIndex -= 1;
            }
        }

        /// <summary>
        /// Fired by the SelectedIndexChanged event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndexChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Fired by the SelectedValueChanged event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnSelectedValueChanged(object sender, EventArgs e)
        {
            SelectedValueChanged?.Invoke(sender, e);
        }
    }
}
