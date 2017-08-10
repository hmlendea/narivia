using Microsoft.Xna.Framework;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Tool tip GUI element.
    /// </summary>
    public class GuiTooltip : GuiElement
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        GuiText text;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiTooltip"/> class.
        /// </summary>
        public GuiTooltip()
        {
            Visible = false;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            text = new GuiText
            {
                FontName = "ToolTipFont",
                Margins = 2
            };

            Children.Add(text);

            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            text.Text = Text;
            text.ForegroundColour = ForegroundColour;
            text.BackgroundColour = BackgroundColour;
            text.Location = Location;
            text.Size = Size;
        }
    }
}
