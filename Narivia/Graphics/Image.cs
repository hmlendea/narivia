using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Screens;
using Narivia.Graphics.ImageEffects;
using Narivia.Helpers;

namespace Narivia.Graphics
{
    /// <summary>
    /// Image.
    /// </summary>
    public class Image
    {
        ContentManager content;
        Vector2 origin;
        RenderTarget2D renderTarget;
        SpriteFont font;

        FadeEffect fadeEffect;
        RotationEffect rotationEffect;
        ZoomEffect zoomEffect;
        AnimationEffect animationEffect;

        readonly Dictionary<string, ImageEffect> effectList;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Image"/> is active.
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
        /// Gets or sets the image path.
        /// </summary>
        /// <value>The image path.</value>
        public string ImagePath { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public Vector2 SpriteSize { get; set; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>The scale.</value>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Gets the covered screen area.
        /// </summary>
        /// <value>The covered screen area.</value>
        public Rectangle ScreenArea
        {
            get
            {
                return new Rectangle(
                    (int)(Position.X - (Scale.X * Zoom * SpriteSize.X - SpriteSize.X) / 2),
                    (int)(Position.Y - (Scale.Y * Zoom * SpriteSize.Y - SpriteSize.Y) / 2),
                    (int)(SourceRectangle.Width * Scale.X),
                    (int)(SourceRectangle.Height * Scale.Y));
            }
        }

        /// <summary>
        /// Gets or sets the source rectangle.
        /// </summary>
        /// <value>The source rectangle.</value>
        [XmlIgnore]
        public Rectangle SourceRectangle { get; set; }

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        /// <value>The texture.</value>
        [XmlIgnore]
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Gets or sets the fill mode.
        /// </summary>
        /// <value>The fill mode.</value>
        public TextureFillMode TextureFillMode { get; set; }

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
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        public Image()
        {
            Active = true;

            effectList = new Dictionary<string, ImageEffect>();
            Effects = string.Empty;

            ImagePath = string.Empty;
            Text = string.Empty;
            FontName = "MenuFont";

            Position = Vector2.Zero;
            SourceRectangle = Rectangle.Empty;

            Opacity = 1.0f;
            Zoom = 1.0f;
            Scale = Vector2.One;
            TextureFillMode = TextureFillMode.Stretch;

            Tint = Color.White;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            if (string.IsNullOrWhiteSpace(Text))
            {
                Text = string.Empty;
            }

            if (!string.IsNullOrEmpty(ImagePath))
            {
                Texture = content.Load<Texture2D>(ImagePath);
            }

            font = content.Load<SpriteFont>("Fonts/" + FontName);

            if (SpriteSize == Vector2.Zero)
            {
                Vector2 size = Vector2.Zero;

                if (Texture != null)
                {
                    size.X = Texture.Width;
                    size.Y = Texture.Height;
                }
                else
                {
                    size.X = font.MeasureString(Text).X;
                    size.Y = font.MeasureString(Text).Y;
                }

                SpriteSize = size;
            }

            if (SourceRectangle == Rectangle.Empty)
            {
                SourceRectangle = new Rectangle(0, 0, (int)SpriteSize.X, (int)SpriteSize.Y);
            }

            renderTarget = new RenderTarget2D(
                ScreenManager.Instance.GraphicsDevice,
                (int)SpriteSize.X, (int)SpriteSize.Y);

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);

            ScreenManager.Instance.SpriteBatch.Begin();

            if (Texture != null)
            {
                ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            }

            ScreenManager.Instance.SpriteBatch.End();

            Texture = renderTarget;

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

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
            content.Unload();

            List<string> effectKeys = effectList.Keys.ToList();

            effectKeys.ForEach(DeactivateEffect);
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            List<ImageEffect> activeEffects = effectList.Values.Where(effect => effect.Active).ToList();

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
                DrawString(font, StringUtils.WrapText(font, Text, SpriteSize.X), ScreenArea,
                           TextHorizontalAlignment, TextVerticalAlignment, Tint * Opacity);
            }

            if (TextureFillMode == TextureFillMode.Stretch)
            {
                spriteBatch.Draw(Texture, Position + origin, SourceRectangle,
                    Tint * Opacity, Rotation,
                    origin, Scale * Zoom,
                    SpriteEffects.None, 0.0f);
            }
            else if (TextureFillMode == TextureFillMode.Tile)
            {
                int tilesX = (int)Math.Ceiling((float)ScreenArea.Width / SourceRectangle.Width);
                int tilesY = (int)Math.Ceiling((float)ScreenArea.Height / SourceRectangle.Height);

                for (int y = 0; y < tilesY; y++)
                {
                    for (int x = 0; x < tilesX; x++)
                    {
                        Vector2 pos = new Vector2(Position.X + origin.X + x * SourceRectangle.Width,
                                                  Position.Y + origin.Y + y * SourceRectangle.Height);

                        spriteBatch.Draw(Texture, pos, SourceRectangle,
                            Tint * Opacity, Rotation,
                            origin, 1.0f,
                            SpriteEffects.None, 0.0f);
                    }
                }
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
        void SetEffect<T>(ref T effect) where T : ImageEffect
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
            List<ImageEffect> activeEffects = effectList.Values.Where(effect => effect.Active).ToList();
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

        void DrawString(SpriteFont spriteFont, string text, Rectangle bounds, HorizontalAlignment hAlign, VerticalAlignment vAlign, Color color)
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

                ScreenManager.Instance.SpriteBatch.DrawString(spriteFont, line, Position + textOrigin, color);

                textOrigin.Y += lineSize.Y;
            }
        }
    }
}
