using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.DataAccess.Resources;
using Narivia.Graphics.CustomSpriteEffects;
using Narivia.Graphics.Enumerations;
using Narivia.Graphics.Extensions;
using Narivia.Graphics.Helpers;

namespace Narivia.Graphics
{
    /// <summary>
    /// Sprite.
    /// </summary>
    public class Sprite
    {
        Vector2 origin;
        RenderTarget2D renderTarget;
        SpriteFont font;

        FadeEffect fadeEffect;
        RotationEffect rotationEffect;
        ZoomEffect zoomEffect;
        AnimationEffect animationEffect;

        Texture2D texture;
        Texture2D alphaMask;

        readonly Dictionary<string, CustomSpriteEffect> effectList;

        string loadedContentFile;
        string loadedAlphaMaskFile;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Sprite"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the tint.
        /// </summary>
        /// <value>The tint.</value>
        public Color Tint { get; set; }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        public float Opacity { get; set; }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>The rotation.</value>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the zoom.
        /// </summary>
        /// <value>The zoom.</value>
        public float Zoom { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        // TODO: Make this a number (Outline size)
        public bool TextOutline { get; set; }

        /// <summary>
        /// Gets or sets the text horizontal alignment.
        /// </summary>
        /// <value>The text horizontal alignment.</value>
        public HorizontalAlignment TextHorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the text vertical alignment.
        /// </summary>
        /// <value>The text vertical alignment.</value>
        public VerticalAlignment TextVerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        public string FontName { get; set; }

        /// <summary>
        /// Gets or sets the content file.
        /// </summary>
        /// <value>The content file.</value>
        public string ContentFile { get; set; }

        /// <summary>
        /// Gets or sets the alpha mask path.
        /// </summary>
        /// <value>The alpha mask path.</value>
        public string AlphaMaskFile { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public Point Position { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public Point SpriteSize { get; set; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>The scale.</value>
        public Vector2 Scale { get; set; }
        
        /// <summary>
        /// Gets or sets the source rectangle.
        /// </summary>
        /// <value>The source rectangle.</value>
        [XmlIgnore]
        public Rectangle SourceRectangle { get; set; }

        /// <summary>
        /// Gets the covered screen area.
        /// </summary>
        /// <value>The covered screen area.</value>
        public Rectangle ClientRectangle
        {
            get
            {
                return new Rectangle(
                    Position.X,
                    Position.Y,
                    (int)(SourceRectangle.Width * Scale.X),
                    (int)(SourceRectangle.Height * Scale.Y));
            }
        }

        /// <summary>
        /// Gets the texture size.
        /// </summary>
        /// <value>The texture size.</value>
        [XmlIgnore]
        public Vector2 TextureSize => new Vector2(texture.Width, texture.Height);

        /// <summary>
        /// Gets or sets the fill mode.
        /// </summary>
        /// <value>The fill mode.</value>
        public TextureLayout TextureLayout { get; set; }

        /// <summary>
        /// Gets or sets the effects.
        /// </summary>
        /// <value>The effects.</value>
        public string Effects { get; set; }

        /// <summary>
        /// Gets or sets the fade effect.
        /// </summary>
        /// <value>The fade effect.</value>
        public FadeEffect FadeEffect
        {
            get { return fadeEffect; }
            set { fadeEffect = value; }
        }

        /// <summary>
        /// Gets or sets the rotation effect.
        /// </summary>
        /// <value>The rotation effect.</value>
        public RotationEffect RotationEffect
        {
            get { return rotationEffect; }
            set { rotationEffect = value; }
        }

        /// <summary>
        /// Gets or sets the zoom effect.
        /// </summary>
        /// <value>The zoom effect.</value>
        public ZoomEffect ZoomEffect
        {
            get { return zoomEffect; }
            set { zoomEffect = value; }
        }

        /// <summary>
        /// Gets or sets the animation effect.
        /// </summary>
        /// <value>The animation effect.</value>
        public AnimationEffect AnimationEffect
        {
            get { return animationEffect; }
            set { animationEffect = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        public Sprite()
        {
            Active = true;

            effectList = new Dictionary<string, CustomSpriteEffect>();
            Effects = string.Empty;

            ContentFile = string.Empty;
            Text = string.Empty;
            FontName = "MenuFont";

            Position = Point.Zero;
            SourceRectangle = Rectangle.Empty;

            Opacity = 1.0f;
            Zoom = 1.0f;
            Scale = Vector2.One;
            TextureLayout = TextureLayout.Stretch;

            Tint = Color.White;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                Text = string.Empty;
            }

            LoadContentFile();
            LoadAlphaMaskFile();

            font = ResourceManager.Instance.LoadSpriteFont("Fonts/" + FontName);

            if (SpriteSize == Point.Zero)
            {
                Point size = Point.Zero;

                if (texture != null)
                {
                    size.X = texture.Width;
                    size.Y = texture.Height;
                }
                else
                {
                    size.X = (int)font.MeasureString(Text).X;
                    size.Y = (int)font.MeasureString(Text).Y;
                }

                SpriteSize = size;
            }

            if (SourceRectangle == Rectangle.Empty)
            {
                SourceRectangle = new Rectangle(0, 0, SpriteSize.X, SpriteSize.Y);
            }

            renderTarget = new RenderTarget2D(
                GraphicsManager.Instance.Graphics.GraphicsDevice,
                SpriteSize.X, SpriteSize.Y);

            GraphicsManager.Instance.Graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsManager.Instance.Graphics.GraphicsDevice.Clear(Color.Transparent);

            if (texture != null)
            {
                GraphicsManager.Instance.SpriteBatch.Begin();
                GraphicsManager.Instance.SpriteBatch.Draw(texture, Vector2.Zero, Color.White);
                GraphicsManager.Instance.SpriteBatch.End();
            }

            texture = renderTarget;

            GraphicsManager.Instance.Graphics.GraphicsDevice.SetRenderTarget(null);

            SetEffect(ref fadeEffect);
            SetEffect(ref rotationEffect);
            SetEffect(ref zoomEffect);
            SetEffect(ref animationEffect);

            if (!string.IsNullOrEmpty(Effects))
            {
                List<string> split = Effects.Split(':').ToList();

                split.ForEach(ActivateEffect);
            }
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            List<string> effectKeys = effectList.Keys.ToList();

            effectKeys.ForEach(DeactivateEffect);
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            LoadContentFile();
            LoadAlphaMaskFile();

            List<CustomSpriteEffect> activeEffects = effectList.Values.Where(effect => effect.Active).ToList();

            activeEffects.ForEach(effect => effect.Update(gameTime));
        }

        /// <summary>
        /// Draws the content.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            origin = new Vector2(SourceRectangle.Width / 2,
                                 SourceRectangle.Height / 2);

            if (!string.IsNullOrEmpty(Text))
            {
                DrawString(spriteBatch, font, StringUtils.WrapText(font, Text, SpriteSize.X), ClientRectangle,
                           TextHorizontalAlignment, TextVerticalAlignment, Tint * Opacity);
            }

            // TODO: Do not do this for every Draw call
            Texture2D textureToDraw = texture;

            // TODO: Find a better way to do this, because this one doesn't keep the mipmaps
            if (alphaMask != null)
            {
                textureToDraw = TextureBlend(texture, alphaMask);
            }
            
            if (TextureLayout == TextureLayout.Stretch)
            {
                spriteBatch.Draw(textureToDraw, new Vector2(Position.X + ClientRectangle.Width / 2, Position.Y + ClientRectangle.Height / 2), SourceRectangle,
                    Tint * Opacity, Rotation,
                    origin, Scale * Zoom,
                    SpriteEffects.None, 0.0f);
            }
            else if (TextureLayout == TextureLayout.Tile)
            {
                GraphicsDevice gd = GraphicsManager.Instance.Graphics.GraphicsDevice;

                Rectangle rec = new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height);

                // TODO: Is it ok to End and Begin again? Does it affect performance?
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);

                spriteBatch.Draw(textureToDraw, Position.ToVector2(), rec, Tint * Opacity);

                spriteBatch.End();
                spriteBatch.Begin();
            }
        }

        /// <summary>
        /// Activates the effect.
        /// </summary>
        /// <param name="effect">Effect.</param>
        public void ActivateEffect(string effect)
        {
            if (effectList.ContainsKey(effect))
            {
                var obj = this;

                effectList[effect].Active = true;
                effectList[effect].LoadContent(ref obj);
            }
        }

        /// <summary>
        /// Deactivates the effect.
        /// </summary>
        /// <param name="effect">Effect.</param>
        public void DeactivateEffect(string effect)
        {
            if (effectList.ContainsKey(effect))
            {
                effectList[effect].Active = false;
                effectList[effect].UnloadContent();
            }
        }

        /// <summary>
        /// Sets the effect.
        /// </summary>
        /// <param name="effect">Effect.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        void SetEffect<T>(ref T effect) where T : CustomSpriteEffect
        {
            if (effect == null)
            {
                effect = (T)Activator.CreateInstance(typeof(T));
            }
            else
            {
                var obj = this;

                effect.Active = true;
                effect.LoadContent(ref obj);
            }

            effectList.Add(effect.Key, effect);
        }

        /// <summary>
        /// Stores the effects.
        /// </summary>
        public void StoreEffects()
        {
            List<CustomSpriteEffect> activeEffects = effectList.Values.Where(effect => effect.Active).ToList();
            Effects = string.Empty;

            activeEffects.ForEach(effect => Effects += effect.Key + ":");

            Effects.TrimEnd(':');
        }

        /// <summary>
        /// Restores the effects.
        /// </summary>
        public void RestoreEffects()
        {
            List<string> effectKeys = effectList.Keys.ToList();
            List<string> split = Effects.Split(':').ToList();

            effectKeys.ForEach(DeactivateEffect);
            split.ForEach(ActivateEffect);
        }
        
        void DrawString(SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Rectangle bounds, HorizontalAlignment hAlign, VerticalAlignment vAlign, Color colour)
        {
            Vector2 textOrigin = Vector2.Zero;
            Vector2 totalSize = font.MeasureString(text);

            string[] lines = text.Split('\n');

            if (hAlign == HorizontalAlignment.Center)
            {
                textOrigin.Y = bounds.Height / 2 - totalSize.Y / 2;
            }
            else if (hAlign == HorizontalAlignment.Bottom)
            {
                textOrigin.Y = bounds.Height - totalSize.Y;
            }

            foreach (string line in lines)
            {
                Vector2 lineSize = font.MeasureString(line);

                if (vAlign == VerticalAlignment.Center)
                {
                    textOrigin.X = bounds.Width / 2 - lineSize.X / 2;
                }
                else if (vAlign == VerticalAlignment.Right)
                {
                    textOrigin.X = bounds.Width - lineSize.X;
                }

                textOrigin = new Vector2((int)Math.Round(textOrigin.X),
                                         (int)Math.Round(textOrigin.Y));

                if (TextOutline)
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            Vector2 pos = new Vector2(Position.X + dx + textOrigin.X,
                                                      Position.Y + dy + textOrigin.Y);

                            // TODO: Do not hardcode the outline colour
                            spriteBatch.DrawString(spriteFont, line, pos, Color.Black);
                        }
                    }
                }

                spriteBatch.DrawString(spriteFont, line, Position.ToVector2() + textOrigin, colour);
                
                textOrigin.Y += lineSize.Y;
            }
        }

        Texture2D TextureBlend(Texture2D source, Texture2D mask)
        {
            Color[] textureBits = new Color[source.Width * source.Height];
            Color[] maskBits = new Color[mask.Width * mask.Height];

            source.GetData(textureBits);
            mask.GetData(maskBits);

            int startX, startY, endX, endY;

            if (mask.Width > source.Width)
            {
                startX = mask.Width - source.Width;
                endX = startX + source.Width;
            }
            else
            {
                startX = source.Width - mask.Width;
                endX = startX + mask.Width;
            }

            if (mask.Height > source.Height)
            {
                startY = mask.Height - source.Height;
                endY = startY + source.Height;
            }
            else
            {
                startY = source.Height - mask.Height;
                endY = startY + mask.Height;
            }

            Parallel.For(startY, endY,
                y => Parallel.For(startX, endX,
                    x =>
                    {
                        int indexTexture = x - startX + (y - startY) * source.Width;
                        int indexMask = x - startX + (y - startY) * mask.Width;

                        textureBits[indexTexture] = Color.FromNonPremultiplied(textureBits[indexTexture].R,
                                                                               textureBits[indexTexture].G,
                                                                               textureBits[indexTexture].B,
                                                                               textureBits[indexTexture].A - 255 + maskBits[indexMask].R);
                    }));

            Texture2D blendedTexture = new Texture2D(source.GraphicsDevice, source.Width, source.Height);
            blendedTexture.SetData(textureBits);

            return blendedTexture;
        }

        void LoadContentFile()
        {
            if (loadedContentFile == ContentFile || string.IsNullOrEmpty(ContentFile))
            {
                return;
            }

            texture = ResourceManager.Instance.LoadTexture2D(ContentFile);

            loadedContentFile = ContentFile;
        }

        void LoadAlphaMaskFile()
        {
            if (loadedAlphaMaskFile == AlphaMaskFile || string.IsNullOrEmpty(AlphaMaskFile))
            {
                return;
            }

            alphaMask = ResourceManager.Instance.LoadTexture2D(AlphaMaskFile);

            loadedAlphaMaskFile = AlphaMaskFile;
        }
    }
}
