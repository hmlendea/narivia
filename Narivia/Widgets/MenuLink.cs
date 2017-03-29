using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Audio;
using Narivia.Graphics;
using Narivia.Graphics.ImageEffects;
using Narivia.Input;

namespace Narivia.Widgets
{
    public class MenuLink : Widget
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the type of the link.
        /// </summary>
        /// <value>The type of the link.</value>
        public string LinkType { get; set; }

        /// <summary>
        /// Gets or sets the link identifier.
        /// </summary>
        /// <value>The link identifier.</value>
        public string LinkId { get; set; }

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
        /// Gets or sets a value indicating whether this <see cref="T:Narivia.Widgets.MenuLink"/> is selected.
        /// </summary>
        /// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool Selected { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.Widgets.MenuLink"/> class.
        /// </summary>
        public MenuLink()
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

            if (Selected)
            {
                Image.Active = true;
                Image.Tint = SelectedTextColour;
            }
            else
            {
                Image.Active = false;
                Image.Tint = TextColour;
            }

            if (InputManager.Instance.IsCursorEnteringArea(Image.ScreenArea))
            {
                AudioManager.Instance.PlaySound("Interface/select");
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <returns>The draw.</returns>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
