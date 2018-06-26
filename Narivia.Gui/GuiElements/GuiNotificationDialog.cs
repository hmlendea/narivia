using NuciXNA.Gui.GuiElements;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;

using Narivia.Audio;

namespace Narivia.Gui.GuiElements
{
    // TODO: Requires refactoring
    /// <summary>
    /// Notification GUI element.
    /// </summary>
    public class GuiNotificationDialog : GuiElement
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        GuiImage background;
        GuiText title;
        GuiText text;
        GuiButton acceptButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiNotificationDialog"/> class.
        /// </summary>
        public GuiNotificationDialog()
        {
            Size = new Size2D(245, 370);
            ForegroundColour = Colour.Black;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            background = new GuiImage
            {
                ContentFile = "Interface/Notification/background"
            };
            title = new GuiText
            {
                FontName = "NotificationTitleFont",
                Location = new Point2D(15, 45),
                Size = new Size2D(Size.Width - 30, 20),
                ForegroundColour = Colour.DarkRed
            };
            text = new GuiText
            {
                FontName = "NotificationFont",
                Location = new Point2D(15, 70),
                Size = new Size2D(Size.Width - 30, 270)
            };
            acceptButton = new GuiButton
            {
                ContentFile = "Interface/Buttons/green-button",
                Text = "Accept",
                ForegroundColour = Colour.White,
                Location = new Point2D(15, 335),
                Size = new Size2D(88, 26)
            };

            base.LoadContent();
        }

        protected override void RegisterChildren()
        {
            base.RegisterChildren();

            AddChild(background);
            AddChild(title);
            AddChild(text);
            AddChild(acceptButton);
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            acceptButton.Clicked += OnYesButtonClicked;
            acceptButton.MouseEntered += OnYesNoButtonEntered;
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            title.Text = Title;
            text.Text = Text;
        }

        void OnYesButtonClicked(object sender, MouseButtonEventArgs e)
        {
            AudioManager.Instance.PlaySound("Interface/click");

            Dispose();
        }

        void OnNoButtonClicked(object sender, MouseButtonEventArgs e)
        {
            AudioManager.Instance.PlaySound("Interface/click");

            Dispose();
        }

        void OnYesNoButtonEntered(object sender, MouseEventArgs e)
        {
            AudioManager.Instance.PlaySound("Interface/select");
        }
    }
}
