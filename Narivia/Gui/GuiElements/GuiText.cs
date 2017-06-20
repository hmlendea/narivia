using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Graphics.ImageEffects;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Text GUI element.
    /// </summary>
    public class GuiText : GuiElement
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
        /// Gets or sets the background colour.
        /// </summary>
        /// <value>The background colour.</value>
        public Color BackgroundColour { get; set; }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        public string FontName { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment of the text.
        /// </summary>
        /// <value>The vertical alignment.</value>
        public VerticalAlignment VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment of the text.
        /// </summary>
        /// <value>The horizontal alignment.</value>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the margins.
        /// </summary>
        /// <value>The margins.</value>
        public int Margins { get; set; }

        /// <summary>
        /// Gets or sets the fade effect.
        /// </summary>
        /// <value>The fade effect.</value>
        public FadeEffect FadeEffect { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the effects are active.
        /// </summary>
        /// <value><c>true</c> if the effects are active; otherwise, <c>false</c>.</value>
        public bool EffectsActive { get; set; }

        Image backgroundImage;
        Image textImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiText"/> class.
        /// </summary>
        public GuiText()
        {
            FontName = "ButtonFont";
            TextColour = Color.Gold;
            BackgroundColour = Color.Transparent;

            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Center;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            backgroundImage = new Image
            {
                ImagePath = "Interface/backgrounds",
                SourceRectangle = new Rectangle(0, 0, 1, 1)
            };
            textImage = new Image();

            SetChildrenProperties();

            backgroundImage.LoadContent();
            textImage.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            backgroundImage.UnloadContent();
            textImage.UnloadContent();

            base.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            SetChildrenProperties();

            backgroundImage.Update(gameTime);
            textImage.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (BackgroundColour != Color.Transparent)
            {
                backgroundImage.Draw(spriteBatch);
            }

            textImage.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        /// <summary>
        /// Activates the effect.
        /// </summary>
        /// <param name="effect">Effect.</param>
        public void ActivateEffect(string effect)
        => textImage.ActivateEffect(effect);

        void SetChildrenProperties()
        {
            backgroundImage.Tint = BackgroundColour;
            backgroundImage.Position = Position;
            backgroundImage.Scale = new Vector2(Size.X + Margins * 2, Size.Y + Margins * 2);

            textImage.Text = Text;
            textImage.FontName = FontName;
            textImage.Tint = TextColour;
            textImage.TextVerticalAlignment = VerticalAlignment;
            textImage.TextHorizontalAlignment = HorizontalAlignment;
            textImage.Position = new Vector2(Position.X + Margins, Position.Y + Margins);
            textImage.SpriteSize = new Vector2(Size.X - Margins * 2, Size.Y - Margins * 2);
            textImage.FadeEffect = FadeEffect;
            textImage.Active = EffectsActive;
        }
    }
}
