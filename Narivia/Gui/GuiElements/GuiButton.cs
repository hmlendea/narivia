using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Narivia.Audio;
using Narivia.Gui.GuiElements.Enumerations;
using Narivia.Infrastructure.Helpers;
using Narivia.Input.Enumerations;
using Narivia.Input.Events;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Button GUI element.
    /// </summary>
    public class GuiButton : GuiElement
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
        public Colour TextColour { get; set; }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        public string FontName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiButton"/> is hovered.
        /// </summary>
        /// <value><c>true</c> if hovered; otherwise, <c>false</c>.</value>
        public bool Hovered { get; set; }

        const int tileSize = 32;

        List<GuiImage> images;
        GuiText text;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiButton"/> class.
        /// </summary>
        public GuiButton()
        {
            FontName = "ButtonFont";
            TextColour = Colour.ChromeYellow;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            images = new List<GuiImage>();
            text = new GuiText();

            for (int x = 0; x < ButtonSize; x++)
            {
                GuiImage image = new GuiImage
                {
                    ContentFile = "Interface/button",
                    SourceRectangle = CalculateSourceRectangle(x, Style)
                };

                images.Add(image);
            }

            Children.AddRange(images);
            Children.Add(text);

            base.LoadContent();
        }

        /// <summary>
        /// Fired by the Clicked event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void OnClicked(object sender, MouseButtonEventArgs e)
        {
            base.OnClicked(sender, e);

            if (!ScreenArea.Contains(e.MousePosition) || e.Button != MouseButton.LeftButton)
            {
                return;
            }

            AudioManager.Instance.PlaySound("Interface/click");
        }

        /// <summary>
        /// Fired by the MouseMoved event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void OnMouseMoved(object sender, MouseEventArgs e)
        {
            base.OnMouseMoved(sender, e);

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

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            for (int i = 0; i < images.Count; i++)
            {
                images[i].Position = new Vector2(Position.X + i * tileSize, Position.Y);
                images[i].SourceRectangle = CalculateSourceRectangle(i, Style);
            }

            text.Text = Text;
            text.TextColour = TextColour;
            text.FontName = FontName;
            text.Position = Position;
            text.Size = Size;
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
    }
}
