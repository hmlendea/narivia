using System;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Audio;
using Narivia.Graphics;
using Narivia.Graphics.ImageEffects;
using Narivia.Input;

namespace Narivia.Interface.Widgets
{
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

        public new Vector2 Position
        {
            get { return image.Position; }
            set { image.Position = value; }
        }

        public new Vector2 Size
        {
            get { return image.SpriteSize; }
            set { image.SpriteSize = value; }
        }

        public new Rectangle ScreenArea => image.ScreenArea;

        // TODO: Maybe implement my own handler and args
        public event EventHandler Activated;

        Image image;

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

            image = new Image();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            image.Text = Text;
            image.FadeEffect = new FadeEffect
            {
                Speed = 2,
                MinimumOpacity = 0.25f
            };

            base.LoadContent();
            image.LoadContent();

            image.ActivateEffect("FadeEffect");
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
            image.UnloadContent();
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

            if (InputManager.Instance.IsCursorEnteringArea(ScreenArea))
            {
                AudioManager.Instance.PlaySound("Interface/select");
                Selected = true;
            }

            if (Selected)
            {
                image.Active = true;
                image.Tint = SelectedTextColour;

                if (InputManager.Instance.IsKeyPressed(Keys.Enter, Keys.E) ||
                    (ScreenArea.Contains(InputManager.Instance.MousePosition) &&
                    InputManager.Instance.IsMouseButtonPressed(MouseButton.LeftButton)))
                {
                    OnActivated(this, null);
                }
            }
            else
            {
                image.Active = false;
                image.Tint = TextColour;
            }

            base.Update(gameTime);
            image.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            image.Draw(spriteBatch);
        }

        protected virtual void OnActivated(object sender, EventArgs e)
        {
            if (Activated != null)
            {
                Activated(this, null);
            }

            AudioManager.Instance.PlaySound("Interface/click");
        }
    }
}
