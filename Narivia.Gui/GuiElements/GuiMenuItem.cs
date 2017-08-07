using System;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Narivia.Audio;
using Narivia.Graphics.CustomSpriteEffects;
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
        /// Gets or sets the selected text colour.
        /// </summary>
        /// <value>The selected text colour.</value>
        public Color SelectedTextColour { get; set; }

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
        /// Initializes a new instance of the <see cref="GuiMenuItem"/> class.
        /// </summary>
        public GuiMenuItem()
        {
            ForegroundColour = Color.White;
            SelectedTextColour = Color.Gold;

            Size = new Point(512, 48);
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

            Children.Add(text);

            base.LoadContent();

            text.ActivateEffect("FadeEffect");
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            InputManager.Instance.KeyboardKeyPressed += OnKeyboardKeyPressed;
        }

        protected override void UnregisterEvents()
        {
            base.UnregisterEvents();

            InputManager.Instance.KeyboardKeyPressed -= OnKeyboardKeyPressed;
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            text.Text = Text;
            text.Position = Position;
            text.Size = Size;

            if (Selected)
            {
                text.EffectsActive = true;
                text.ForegroundColour = SelectedTextColour;
            }
            else
            {
                text.EffectsActive = false;
                text.ForegroundColour = ForegroundColour;
            }
        }

        protected override void OnClicked(object sender, MouseButtonEventArgs e)
        {
            base.OnClicked(sender, e);

            OnActivated(this, null);
        }

        protected override void OnMouseEntered(object sender, MouseEventArgs e)
        {
            base.OnMouseEntered(sender, e);

            AudioManager.Instance.PlaySound("Interface/select");
            Selected = true;
        }

        /// <summary>
        /// Fired by the Activated event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnActivated(object sender, EventArgs e)
        {
            Activated?.Invoke(this, null);

            AudioManager.Instance.PlaySound("Interface/click");
        }

        protected virtual void OnKeyboardKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            if (Selected && e.Key == Keys.Enter || e.Key == Keys.E)
            {
                OnActivated(this, null);
            }
        }
    }
}
