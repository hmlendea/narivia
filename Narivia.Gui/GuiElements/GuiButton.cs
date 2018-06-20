using System.Collections.Generic;

using NuciXNA.Gui.GuiElements;
using NuciXNA.Input.Enumerations;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;

using Narivia.Audio;
using Narivia.Gui.GuiElements.Enumerations;
using Narivia.Settings;

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
        public int ButtonSize => Size.Width / ButtonTileSize;

        public int ButtonTileSize { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        List<GuiImage> images;
        GuiText text;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiButton"/> class.
        /// </summary>
        public GuiButton()
        {
            FontName = "ButtonFont";
            ButtonTileSize = GameDefines.GuiTileSize;
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
                    SourceRectangle = CalculateSourceRectangle(x, Style),
                    Size = new Size2D(GameDefines.GuiTileSize, GameDefines.GuiTileSize)
                };

                images.Add(image);
            }

            images.ForEach(AddChild);
            AddChild(text);

            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            for (int i = 0; i < images.Count; i++)
            {
                images[i].Location = new Point2D(i * GameDefines.GuiTileSize, 0);
                images[i].SourceRectangle = CalculateSourceRectangle(i, Style);
            }

            text.Text = Text;
            text.Size = Size;
        }

        /// <summary>
        /// Fired by the Clicked event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void OnClicked(object sender, MouseButtonEventArgs e)
        {
            base.OnClicked(sender, e);

            if (e.Button != MouseButton.LeftButton)
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
        protected override void OnMouseEntered(object sender, MouseEventArgs e)
        {
            base.OnMouseMoved(sender, e);

            AudioManager.Instance.PlaySound("Interface/select");
        }

        Rectangle2D CalculateSourceRectangle(int x, ButtonStyle style)
        {
            int sx = 1;

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

            if (Hovered)
            {
                sx += 4;
            }

            return new Rectangle2D(
                sx * GameDefines.GuiTileSize, (int)style * GameDefines.GuiTileSize,
                GameDefines.GuiTileSize, GameDefines.GuiTileSize);
        }
    }
}
