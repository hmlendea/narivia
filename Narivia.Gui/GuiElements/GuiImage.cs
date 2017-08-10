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
    /// Image GUI element.
    /// </summary>
    public class GuiImage : GuiElement
    {
        /// <summary>
        /// Gets or sets the content file.
        /// </summary>
        /// <value>The content file.</value>
        public string ContentFile { get; set; }

        /// <summary>
        /// Gets or sets the mask file.
        /// </summary>
        /// <value>The mask file.</value>
        public string MaskFile { get; set; }

        /// <summary>
        /// Gets or sets the tint colour.
        /// </summary>
        /// <value>The tint colour.</value>
        public Colour TintColour { get; set; }

        /// <summary>
        /// Gets or sets the source rectangle.
        /// </summary>
        /// <value>The source rectangle.</value>
        public Rectangle2D SourceRectangle { get; set; }

        /// <summary>
        /// Gets or sets the texture fill mode.
        /// </summary>
        /// <value>The fill mode.</value>
        public TextureLayout TextureLayout { get; set; }

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

        Sprite sprite;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiImage"/> class.
        /// </summary>
        public GuiImage()
        {
            TintColour = Colour.White;
            Size = Size2D.Empty;
            TextureLayout = TextureLayout.Stretch;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            sprite = new Sprite();

            SetChildrenProperties();

            sprite.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            sprite.UnloadContent();

            base.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        /// <summary>
        /// Activates the effect.
        /// </summary>
        /// <param name="effect">Effect.</param>
        public void ActivateEffect(string effect)
        => sprite.ActivateEffect(effect);

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            if (SourceRectangle == Rectangle2D.Empty)
            {
                SourceRectangle = new Rectangle2D(0, 0, sprite.SpriteSize.X, sprite.SpriteSize.Y);
            }

            if (Size == Size2D.Empty)
            {
                Size = sprite.SourceRectangle.Size.ToSize2D();
            }

            sprite.Active = EffectsActive;
            sprite.AlphaMaskFile = MaskFile;
            sprite.ContentFile = ContentFile;
            sprite.FadeEffect = FadeEffect;
            sprite.Location = Location.ToXnaPoint();
            sprite.SourceRectangle = SourceRectangle.ToXnaRectangle();
            sprite.TextureLayout = TextureLayout;
            sprite.Tint = TintColour.ToXnaColor();

            if (!sprite.SourceRectangle.IsEmpty)
            {
                sprite.Scale = new Vector2((float)Size.Width / sprite.SourceRectangle.Width,
                                           (float)Size.Height / sprite.SourceRectangle.Height);
            }
        }
    }
}
