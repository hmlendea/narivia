using System;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Audio;
using Narivia.Graphics;
using Narivia.Graphics.ImageEffects;
using Narivia.Input;
using Narivia.Input.Enumerations;

namespace Narivia.Interface.Widgets
{
    /// <summary>
    /// Tool tip widget.
    /// </summary>
    public class ToolTip : Widget
    {
        const int TEXT_MARGINS = 2;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the background colour.
        /// </summary>
        /// <value>The background colour.</value>
        public Color BackgroundColour { get; set; }

        /// <summary>
        /// Gets or sets the text colour.
        /// </summary>
        /// <value>The text colour.</value>
        public Color TextColour { get; set; }

        Image background;
        Image text;

        public ToolTip()
        {
            BackgroundColour = Color.DarkRed;
            TextColour = Color.Gold;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            background = new Image
            {
                Tint = BackgroundColour,
                ImagePath = "ScreenManager/FillImage",
                SourceRectangle = new Rectangle(0, 0, 1, 1),
                Scale = Size
            };

            text = new Image
            {
                SpriteSize = new Vector2(Size.X - TEXT_MARGINS * 2, Size.Y - TEXT_MARGINS * 2),
                Tint = TextColour,
                FontName = "ToolTipFont",
                TextVerticalAlignment = VerticalAlignment.Center,
                TextHorizontalAlignment = HorizontalAlignment.Center
            };

            background.LoadContent();
            text.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            background.UnloadContent();
            text.UnloadContent();

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

            background.Position = Position;
            text.Position = new Vector2(Position.X + TEXT_MARGINS, Position.Y + TEXT_MARGINS);

            text.Text = Text;

            background.Update(gameTime);
            text.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }

            background.Draw(spriteBatch);
            text.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
