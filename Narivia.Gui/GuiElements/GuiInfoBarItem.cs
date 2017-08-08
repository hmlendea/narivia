using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;

using Narivia.Graphics.Enumerations;
using Narivia.Input.Events;
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
        public Rectangle SourceRectangle { get; set; }

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
                HorizontalAlignment = HorizontalAlignment.Center,
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
            icon.Position = Position;
            icon.Size = new Point(Size.Y, Size.Y);
            icon.SourceRectangle = SourceRectangle;

            text.Text = Text;
            text.Position = new Point(icon.ClientRectangle.Right + Spacing, Position.Y);
            text.Size = new Point(Size.X - Size.Y - Spacing, Size.Y);
        }
    }
}
