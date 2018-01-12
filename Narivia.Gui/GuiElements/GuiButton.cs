using System.Collections.Generic;

using NuciXNA.Primitives;

using Narivia.Audio;
using Narivia.Gui.GuiElements.Enumerations;
using Narivia.Input.Enumerations;
using Narivia.Input.Events;
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
        public int ButtonSize => Size.Width / GameDefines.GUI_TILE_SIZE;
        
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

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            for (int i = 0; i < images.Count; i++)
            {
                images[i].Location = new Point2D(Location.X + i * GameDefines.GUI_TILE_SIZE, Location.Y);
                images[i].SourceRectangle = CalculateSourceRectangle(i, Style);
            }

            text.Text = Text;
            text.ForegroundColour = ForegroundColour;
            text.FontName = FontName;
            text.Location = Location;
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

            return new Rectangle2D(sx * GameDefines.GUI_TILE_SIZE, (int)style * GameDefines.GUI_TILE_SIZE,
                                   GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE);
        }
    }
}
