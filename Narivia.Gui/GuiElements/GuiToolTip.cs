using Narivia.Graphics;

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

        /// <summary>
        /// Gets or sets the background colour.
        /// </summary>
        /// <value>The background colour.</value>
        public Colour BackgroundColour { get; set; }

        /// <summary>
        /// Gets or sets the text colour.
        /// </summary>
        /// <value>The text colour.</value>
        public Colour TextColour { get; set; }

        GuiText text;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiTooltip"/> class.
        /// </summary>
        public GuiTooltip()
        {
            BackgroundColour = Colour.DarkRed;
            TextColour = Colour.ChromeYellow;
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
            text.TextColour = TextColour;
            text.BackgroundColour = BackgroundColour;
            text.Position = Position;
            text.Size = Size;
        }
    }
}
