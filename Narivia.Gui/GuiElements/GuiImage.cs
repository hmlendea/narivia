using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Graphics.CustomSpriteEffects;
using Narivia.Graphics.Enumerations;

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
        public Color TintColour { get; set; }

        /// <summary>
        /// Gets or sets the source rectangle.
        /// </summary>
        /// <value>The source rectangle.</value>
        public Rectangle SourceRectangle { get; set; }

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
            TintColour = Color.White;
            Size = Point.Zero;
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

            if (SourceRectangle == Rectangle.Empty)
            {
                SourceRectangle = new Rectangle(0, 0, sprite.SpriteSize.X, sprite.SpriteSize.Y);
            }

            if (Size == Point.Zero)
            {
                Size = sprite.SourceRectangle.Size;
            }

            sprite.AlphaMaskFile = MaskFile;
            sprite.ContentFile = ContentFile;
            sprite.SourceRectangle = SourceRectangle;
            sprite.Tint = TintColour;
            sprite.Position = Position;
            sprite.TextureLayout = TextureLayout;
            sprite.FadeEffect = FadeEffect;
            sprite.Active = EffectsActive;

            if (sprite.SourceRectangle != Rectangle.Empty)
            {
                sprite.Scale = new Vector2((float)Size.X / sprite.SourceRectangle.Width,
                                           (float)Size.Y / sprite.SourceRectangle.Height);
            }
        }
    }
}
