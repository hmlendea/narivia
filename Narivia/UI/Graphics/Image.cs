using System;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Narivia.UI.Screens;

namespace Narivia.UI.Graphics
{
    public class Image
    {
        public float Opacity { get; set; }

        public string Text { get; set; }

        public string FontName{ get; set; }

        public string Path { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Scale { get; set; }

        [XmlIgnore]
        public Rectangle SourceRectangle { get; set; }

        [XmlIgnore]
        public Texture2D Texture { get; set; }

        ContentManager content;
        Vector2 origin;
        RenderTarget2D renderTarget;
        SpriteFont font;

        public Image()
        {
            Path = string.Empty;

            Text = string.Empty;
            FontName = "Fonts/Palatino Linotype";

            Position = Vector2.Zero;
            Scale = Vector2.One;
            SourceRectangle = Rectangle.Empty;
            Opacity = 1.0f;
        }

        public void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            if (!string.IsNullOrEmpty(Path))
                Texture = content.Load<Texture2D>(Path);

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
        }

        public void UnloadContent()
        {
            content.Unload();
        }

        public void Update(GameTime gameTime)
        {

        }

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
    }
}
