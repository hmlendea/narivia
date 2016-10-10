using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Narivia.UI.Screens;
using Narivia.UI.Graphics.ImageEffects;

using Effect = Narivia.UI.Graphics.ImageEffects.Effect;

namespace Narivia.UI.Graphics
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

        readonly Dictionary<string, Effect> effectList;
        FadeEffect fadeEffect;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Narivia.UI.Graphics.Image"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        public float Opacity { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        public string FontName{ get; set; }

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
        /// Gets or sets the texture.
        /// </summary>
        /// <value>The texture.</value>
        [XmlIgnore]
        public Texture2D Texture { get; set; }

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
        /// Initializes a new instance of the <see cref="Narivia.UI.Graphics.Image"/> class.
        /// </summary>
        public Image()
        {
            Active = true;

            effectList = new Dictionary<string, Effect>();
            Effects = string.Empty;

            ImagePath = string.Empty;
            Text = string.Empty;
            FontName = "Fonts/Palatino Linotype";

            Position = Vector2.Zero;
            Scale = Vector2.One;
            SourceRectangle = Rectangle.Empty;
            Opacity = 1.0f;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            if (!string.IsNullOrEmpty(ImagePath))
                Texture = content.Load<Texture2D>(ImagePath);

            font = content.Load<SpriteFont>(FontName);

            Vector2 dimensions = Vector2.Zero;

            if (Texture != null)
                dimensions.X += Texture.Width;
            dimensions.X += font.MeasureString(Text).X;

            if (Texture != null)
                dimensions.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y);
            else
                dimensions.Y = font.MeasureString(Text).Y;

            if (SourceRectangle == Rectangle.Empty)
                SourceRectangle = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

            renderTarget = new RenderTarget2D(
                ScreenManager.Instance.GraphicsDevice, 
                (int)dimensions.X, (int)dimensions.Y);

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);

            ScreenManager.Instance.SpriteBatch.Begin();

            if (Texture != null)
                ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            
            if (!string.IsNullOrEmpty(Text))
                ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);

            ScreenManager.Instance.SpriteBatch.End();

            Texture = renderTarget;

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

            SetEffect<FadeEffect>(ref fadeEffect);

            if (!string.IsNullOrEmpty(Effects))
            {
                string[] split = Effects.Split(':');

                foreach (string item in split)
                    ActivateEffect(item);
            }
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            content.Unload();

            foreach (string effectKey in effectList.Keys)
                DeactivateEffect(effectKey);
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            foreach (Effect effect in effectList.Values)
                if (effect.Active)
                    effect.Update(gameTime);
        }

        /// <summary>
        /// Draws the content.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            origin = new Vector2(
                SourceRectangle.Width / 2,
                SourceRectangle.Height / 2);

            spriteBatch.Draw(Texture, Position + origin, SourceRectangle,
                Color.White * Opacity, 0.0f,
                origin, Scale,
                SpriteEffects.None, 0.0f);
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
        void SetEffect<T>(ref T effect) where T:Effect
        {

            if (effect == null)
                effect = (T)Activator.CreateInstance(typeof(T));
            else
            {
                var obj = this;

                effect.Active = true;
                effect.LoadContent(ref obj);
            }

            effectList.Add(effect.Key, effect);
        }
        
        public void StoreEffects()
        {
            Effects = string.Empty;

            foreach (var effect in effectList)
            {
                if (effect.Value.Active)
                    Effects += effect.Key + ":";
            }

            Effects.TrimEnd(':');
        }

        public void RestoreEffects()
        {
            foreach (var effect in effectList)
                DeactivateEffect(effect.Key);

            string[] split = Effects.Split(':');
            foreach (string effectKey in split)
                ActivateEffect(effectKey);
        }
    }
}
