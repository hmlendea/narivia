using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Graphics;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Graphics.Enumerations;
using NuciXNA.Primitives;

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
        /// Gets or sets the animation effect.
        /// </summary>
        /// <value>The animation effect.</value>
        public AnimationEffect AnimationEffect { get; set; }

        /// <summary>
        /// Gets or sets the fade effect.
        /// </summary>
        /// <value>The fade effect.</value>
        public FadeEffect FadeEffect { get; set; }

        /// <summary>
        /// Gets or sets the rotation effect.
        /// </summary>
        /// <value>The rotation effect.</value>
        public RotationEffect RotationEffect { get; set; }

        /// <summary>
        /// Gets or sets the zoom effect.
        /// </summary>
        /// <value>The zoom effect.</value>
        public ZoomEffect ZoomEffect { get; set; }

        public float Rotation { get; set; }

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

            if (SourceRectangle == Rectangle2D.Empty)
            {
                SourceRectangle = new Rectangle2D(Point2D.Empty, sprite.SpriteSize);
            }
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

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            if (Size == Size2D.Empty)
            {
                Size = sprite.SourceRectangle.Size;
            }

            sprite.Active = EffectsActive;
            sprite.AlphaMaskFile = MaskFile;
            sprite.ContentFile = ContentFile;
            sprite.Location = Location;
            sprite.SourceRectangle = SourceRectangle;
            sprite.Rotation = Rotation;
            sprite.TextureLayout = TextureLayout;
            sprite.Tint = TintColour;

            sprite.AnimationEffect = AnimationEffect;
            sprite.FadeEffect = FadeEffect;
            sprite.RotationEffect = RotationEffect;
            sprite.ZoomEffect = ZoomEffect;

            if (!sprite.SourceRectangle.IsEmpty)
            {
                sprite.Scale = new Scale2D((float)Size.Width / sprite.SourceRectangle.Width,
                                           (float)Size.Height / sprite.SourceRectangle.Height);
            }
        }
    }
}
