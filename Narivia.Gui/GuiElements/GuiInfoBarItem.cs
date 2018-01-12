using NuciXNA.Primitives;

using Narivia.Graphics.Enumerations;
using Narivia.Settings;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Info bar GUI element.
    /// </summary>
    public class GuiInfoBarItem : GuiElement
    {
        GuiImage icon;
        GuiText text;

        /// <summary>
        /// Gets or sets the content file.
        /// </summary>
        /// <value>The content file.</value>
        public string ContentFile { get; set; }

        /// <summary>
        /// Gets or sets the source rectangle.
        /// </summary>
        /// <value>The source rectangle.</value>
        public Rectangle2D SourceRectangle { get; set; }

        /// <summary>
        /// Gets or sets the spacing.
        /// </summary>
        /// <value>The spacing.</value>
        public int Spacing { get; set; }

        /// <summary>
        /// Gets or sets text value.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoBarItem"/> class.
        /// </summary>
        public GuiInfoBarItem()
        {
            Spacing = GameDefines.GUI_SPACING;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            icon = new GuiImage();
            text = new GuiText
            {
                HorizontalAlignment = HorizontalAlignment.Centre,
                VerticalAlignment = VerticalAlignment.Left
            };

            Children.Add(icon);
            Children.Add(text);
            
            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            icon.ContentFile = ContentFile;
            icon.Location = Location;
            icon.Size = new Size2D(Size.Height, Size.Height);
            icon.SourceRectangle = SourceRectangle;

            text.Text = Text;
            text.Location = new Point2D(icon.ClientRectangle.Right + Spacing, Location.Y);
            text.Size = new Size2D(Size.Width - Size.Height - Spacing, Size.Height);
        }
    }
}
