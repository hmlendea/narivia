using NuciXNA.Gui.GuiElements;
using NuciXNA.Input.Enumerations;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;

using Narivia.Audio;
using Narivia.Gui.GuiElements.Enumerations;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Button GUI element.
    /// </summary>
    public class GuiSimpleButton : GuiElement
    {
        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        /// <value>The style.</value>
        public ButtonStyle Style { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        public string ContentFile { get; set; }

        GuiImage image;
        GuiText text;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiSimpleButton"/> class.
        /// </summary>
        public GuiSimpleButton()
        {
            FontName = "ButtonFont";
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            image = new GuiImage
            {
                ContentFile = ContentFile
            };
            text = new GuiText();

            Children.Add(image);
            Children.Add(text);

            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            image.Location = Location;

            if (!Hovered)
            {
                image.SourceRectangle = new Rectangle2D(0, 0, Size.Width, Size.Height);
            }
            else
            {
                image.SourceRectangle = new Rectangle2D(Size.Width, 0, Size.Width, Size.Height);
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
    }
}
