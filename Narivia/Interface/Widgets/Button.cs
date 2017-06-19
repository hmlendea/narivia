using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Audio;
using Narivia.Graphics;
using Narivia.Input;
using Narivia.Input.Enumerations;
using Narivia.Input.Events;
using Narivia.Interface.Widgets.Enumerations;

namespace Narivia.Interface.Widgets
{
    /// <summary>
    /// Button widget.
    /// </summary>
    public class Button : Widget
    {
        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        /// <value>The style.</value>
        public ButtonStyle Style { get; set; }

        /// <summary>
        /// Gets or sets the size of the button.
        /// </summary>
        /// <value>The size of the button.</value>
        public int ButtonSize
        {
            get
            {
                return (int)Math.Round(Size.X / tileSize);
            }
        }

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
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        public string FontName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Button"/> is hovered.
        /// </summary>
        /// <value><c>true</c> if hovered; otherwise, <c>false</c>.</value>
        public bool Hovered { get; set; }

        const int tileSize = 32;

        Image[] images;
        Image textImage;

        public Button()
        {
            FontName = "ButtonFont";
            TextColour = Color.Gold;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            images = new Image[ButtonSize];
            textImage = new Image();

            for (int x = 0; x < ButtonSize; x++)
            {
                images[x] = new Image
                {
                    ImagePath = "Interface/button",
                    SourceRectangle = CalculateSourceRectangle(x, Style)
                };

                images[x].LoadContent();
            }

            textImage.Text = Text;
            textImage.TextVerticalAlignment = VerticalAlignment.Center;
            textImage.TextHorizontalAlignment = HorizontalAlignment.Center;
            textImage.Tint = TextColour;
            textImage.FontName = FontName;
            textImage.SpriteSize = Size;

            textImage.LoadContent();

            base.LoadContent();

            InputManager.Instance.MouseButtonPressed += InputManager_OnMouseButtonPressed;
            InputManager.Instance.MouseMoved += InputManager_OnMouseMoved;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            foreach (Image image in images)
            {
                image.UnloadContent();
            }

            textImage.UnloadContent();

            base.UnloadContent();

            InputManager.Instance.MouseButtonPressed -= InputManager_OnMouseButtonPressed;
            InputManager.Instance.MouseMoved -= InputManager_OnMouseMoved;
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

            for (int x = 0; x < ButtonSize; x++)
            {
                images[x].Position = new Vector2(Position.X + x * tileSize, Position.Y);
            }

            textImage.Position = Position;
            textImage.Text = Text;

            for (int x = 0; x < ButtonSize; x++)
            {
                Image image = images[x];

                image.Update(gameTime);
                image.SourceRectangle = CalculateSourceRectangle(x, Style);
            }

            textImage.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Image image in images)
            {
                image.Draw(spriteBatch);
            }

            textImage.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        Rectangle CalculateSourceRectangle(int x, ButtonStyle style)
        {
            int sx = 1;
            int sy = 0;

            if (ButtonSize == 1)
            {
                sx = 3;
            }
            else if (x == 0)
            {
                sx = 0;
            }
            else if (x == ButtonSize - 1)
            {
                sx = 2;
            }

            switch (Style)
            {
                default:
                    sy = 0;
                    break;
            }

            if (Hovered)
            {
                sx += 4;
            }

            return new Rectangle(sx * tileSize, sy * tileSize, tileSize, tileSize);
        }

        void InputManager_OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (!ScreenArea.Contains(e.MousePosition) || e.Button != MouseButton.LeftButton)
            {
                return;
            }

            AudioManager.Instance.PlaySound("Interface/click");
        }

        void InputManager_OnMouseMoved(object sender, MouseEventArgs e)
        {
            if (ScreenArea.Contains(e.CurrentMousePosition))
            {
                Hovered = true;

                if (!ScreenArea.Contains(e.PreviousMousePosition))
                {
                    AudioManager.Instance.PlaySound("Interface/select");
                }
            }
            else
            {
                Hovered = false;
            }
        }
    }
}
