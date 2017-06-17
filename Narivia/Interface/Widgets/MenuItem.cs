using System;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Audio;
using Narivia.Graphics;
using Narivia.Graphics.ImageEffects;
using Narivia.Input;
using Narivia.Input.Enumerations;
using Narivia.Input.Events;

namespace Narivia.Interface.Widgets
{
    /// <summary>
    /// Menu item widget.
    /// </summary>
    public class MenuItem : Widget
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
        public Color TextColour { get; set; }

        /// <summary>
        /// Gets or sets the selected text colour.
        /// </summary>
        /// <value>The selected text colour.</value>
        public Color SelectedTextColour { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public new Vector2 Position
        {
            get { return TextImage.Position; }
            set { TextImage.Position = value; }
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public new Vector2 Size { get; set; }

        /// <summary>
        /// Gets the screen area.
        /// </summary>
        /// <value>The screen area.</value>
        public new Rectangle ScreenArea => TextImage.ScreenArea;

        // TODO: Maybe implement my own handler and args
        /// <summary>
        /// Occurs when activated.
        /// </summary>
        public event EventHandler Activated;

        /// <summary>
        /// The text image.
        /// </summary>
        protected Image TextImage;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Narivia.Widgets.MenuItem"/> is selected.
        /// </summary>
        /// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool Selected { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.Widgets.MenuItem"/> class.
        /// </summary>
        public MenuItem()
        {
            TextColour = Color.White;
            SelectedTextColour = Color.Gold;

            Size = new Vector2(512, 48);
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            TextImage = new Image
            {
                Text = Text,
                SpriteSize = Size,
                TextVerticalAlignment = VerticalAlignment.Center,
                TextHorizontalAlignment = HorizontalAlignment.Center
            };
            TextImage.FadeEffect = new FadeEffect
            {
                Speed = 2,
                MinimumOpacity = 0.25f
            };

            base.LoadContent();
            TextImage.LoadContent();

            TextImage.ActivateEffect("FadeEffect");

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
            TextImage.UnloadContent();

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
            if (!Enabled)
            {
                return;
            }

            TextImage.SpriteSize = Size;

            if (Selected)
            {
                TextImage.Active = true;
                TextImage.Tint = SelectedTextColour;
            }
            else
            {
                TextImage.Active = false;
                TextImage.Tint = TextColour;
            }

            base.Update(gameTime);
            TextImage.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            TextImage.Draw(spriteBatch);
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
