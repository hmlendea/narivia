using System;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Audio;
using Narivia.Graphics.ImageEffects;
using Narivia.Infrastructure.Helpers;
using Narivia.Input;
using Narivia.Input.Events;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Menu item GUI element.
    /// </summary>
    public class GuiMenuItem : GuiElement
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the text colour.
        /// </summary>
        /// <value>The text colour.</value>
        public Colour TextColour { get; set; }

        /// <summary>
        /// Gets or sets the selected text colour.
        /// </summary>
        /// <value>The selected text colour.</value>
        public Colour SelectedTextColour { get; set; }

        // TODO: Maybe implement my own handler and args
        /// <summary>
        /// Occurs when activated.
        /// </summary>
        public event EventHandler Activated;

        /// <summary>
        /// The text GUI element.
        /// </summary>
        protected GuiText text;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiMenuItem"/> is selected.
        /// </summary>
        /// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool Selected { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.GUI elements.MenuItem"/> class.
        /// </summary>
        public GuiMenuItem()
        {
            TextColour = Colour.White;
            SelectedTextColour = Colour.ChromeYellow;

            Size = new Vector2(512, 48);
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            text = new GuiText { FontName = "MenuFont" };
            text.FadeEffect = new FadeEffect
            {
                Speed = 2,
                MinimumOpacity = 0.25f
            };

            SetChildrenProperties();

            Children.Add(text);

            base.LoadContent();

            text.ActivateEffect("FadeEffect");

            InputManager.Instance.KeyboardKeyPressed += InputManager_OnKeyboardKeyPressed;
            InputManager.Instance.MouseButtonPressed += InputManager_OnMouseButtonPressed;
            InputManager.Instance.MouseMoved += InputManager_OnMouseMoved;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();

            InputManager.Instance.KeyboardKeyPressed -= InputManager_OnKeyboardKeyPressed;
            InputManager.Instance.MouseButtonPressed -= InputManager_OnMouseButtonPressed;
            InputManager.Instance.MouseMoved -= InputManager_OnMouseMoved;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            SetChildrenProperties();

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

        void SetChildrenProperties()
        {
            text.Text = Text;
            text.Position = Position;
            text.Size = Size;

            if (Selected)
            {
                text.EffectsActive = true;
                text.TextColour = SelectedTextColour;
            }
            else
            {
                text.EffectsActive = false;
                text.TextColour = TextColour;
            }
        }

        /// <summary>
        /// Fired by the Activated event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnActivated(object sender, EventArgs e)
        {
            if (Activated != null)
            {
                Activated(this, null);
            }

            AudioManager.Instance.PlaySound("Interface/click");
        }

        void InputManager_OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (ScreenArea.Contains(e.MousePosition))
            {
                OnActivated(this, null);
            }
        }

        void InputManager_OnMouseMoved(object sender, MouseEventArgs e)
        {
            if (ScreenArea.Contains(e.CurrentMousePosition) && !ScreenArea.Contains(e.PreviousMousePosition))
            {
                AudioManager.Instance.PlaySound("Interface/select");
                Selected = true;
            }
        }

        void InputManager_OnKeyboardKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            if (Selected && e.Key == Keys.Enter || e.Key == Keys.E)
            {
                OnActivated(this, null);
            }
        }
    }
}
