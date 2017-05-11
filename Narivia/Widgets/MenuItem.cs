using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Audio;
using Narivia.Graphics;
using Narivia.Graphics.ImageEffects;
using Narivia.Input;

namespace Narivia.Widgets
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
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            Image.Text = Text;
            Image.FadeEffect = new FadeEffect
            {
                Speed = 2,
                MinimumOpacity = 0.25f
            };

            base.LoadContent();

            Image.ActivateEffect("FadeEffect");
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

            if (InputManager.Instance.IsCursorEnteringArea(Image.ScreenArea))
            {
                AudioManager.Instance.PlaySound("Interface/select");
                Selected = true;
            }

            if (Selected)
            {
                Image.Active = true;
                Image.Tint = SelectedTextColour;

                if (InputManager.Instance.IsKeyPressed(Keys.Enter, Keys.E) ||
                    (Image.ScreenArea.Contains(InputManager.Instance.MousePosition) &&
                    InputManager.Instance.IsMouseButtonPressed(MouseButton.LeftButton)))
                {
                    Activate();
                }
            }
            else
            {
                Image.Active = false;
                Image.Tint = TextColour;
            }

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

        /// <summary>
        /// Activates this item.
        /// </summary>
        public virtual void Activate()
        {
            AudioManager.Instance.PlaySound("Interface/click");
        }
    }
}
