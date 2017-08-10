using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Graphics.CustomSpriteEffects;
using Narivia.Graphics.Enumerations;
using Narivia.Graphics.Geometry;
using Narivia.Graphics.Geometry.Mapping;
using Narivia.Graphics.Mapping;

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

        public bool TextOutline { get; set; }

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

        GuiImage backgroundImage;
        Sprite textSprite;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiText"/> class.
        /// </summary>
        public GuiText()
        {
            FontName = "ButtonFont";
            ForegroundColour = Colour.Gold;
            BackgroundColour = Colour.Transparent;

            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Center;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            backgroundImage = new GuiImage
            {
                ContentFile = "ScreenManager/FillImage",
                TextureLayout = TextureLayout.Tile
            };

            textSprite = new Sprite();

            Children.Add(backgroundImage);

            SetChildrenProperties();

            textSprite.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();

            textSprite.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            textSprite.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            textSprite.Draw(spriteBatch);
        }

        /// <summary>
        /// Activates the effect.
        /// </summary>
        /// <param name="effect">Effect.</param>
        public void ActivateEffect(string effect)
        => textSprite.ActivateEffect(effect);

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            backgroundImage.TintColour = BackgroundColour;
            backgroundImage.Location = Location;
            backgroundImage.Size = new Size2D(Size.Width + Margins * 2,
                                              Size.Height + Margins * 2);

            textSprite.Text = Text;
            textSprite.FontName = FontName;
            textSprite.Tint = ForegroundColour.ToXnaColor();
            textSprite.TextVerticalAlignment = VerticalAlignment;
            textSprite.TextHorizontalAlignment = HorizontalAlignment;
            textSprite.TextOutline = TextOutline;
            textSprite.Location = new Point2D(Location.X + Margins,
                                              Location.Y + Margins).ToXnaPoint();
            textSprite.SpriteSize = new Point2D(Size.Width - Margins * 2,
                                                Size.Height - Margins * 2).ToXnaPoint();
            textSprite.FadeEffect = FadeEffect;
            textSprite.Active = EffectsActive;
        }
    }
}
