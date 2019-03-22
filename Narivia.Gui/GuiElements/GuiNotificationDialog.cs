using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.GuiElements;
using NuciXNA.Input;
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
        protected override void DoLoadContent()
        {
            background = new GuiImage
            {
                Id = $"{Id}_{nameof(background)}",
                ContentFile = "Interface/Notification/background"
            };
            title = new GuiText
            {
                Id = $"{Id}_{nameof(title)}",
                FontName = "NotificationTitleFont",
                Location = new Point2D(15, 45),
                Size = new Size2D(Size.Width - 30, 20),
                ForegroundColour = Colour.DarkRed
            };
            text = new GuiText
            {
                Id = $"{Id}_{nameof(text)}",
                FontName = "NotificationFont",
                Location = new Point2D(15, 70),
                Size = new Size2D(Size.Width - 30, 270)
            };
            acceptButton = new GuiButton
            {
                Id = $"{Id}_{nameof(acceptButton)}",
                ContentFile = "Interface/Buttons/green-button",
                Text = "Accept",
                ForegroundColour = Colour.White,
                Location = new Point2D(15, 335),
                Size = new Size2D(88, 26)
            };
            
            RegisterChildren();
            RegisterEvents();
            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            UnregisterEvents();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            SetChildrenProperties();
        }

        /// <summary>
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }

        void RegisterChildren()
        {
            AddChild(background);
            AddChild(title);
            AddChild(text);
            AddChild(acceptButton);
        }

        void RegisterEvents()
        {
            acceptButton.Clicked += OnYesButtonClicked;
            acceptButton.MouseEntered += OnYesNoButtonEntered;
        }

        void UnregisterEvents()
        {
            acceptButton.Clicked -= OnYesButtonClicked;
            acceptButton.MouseEntered -= OnYesNoButtonEntered;
        }

        void SetChildrenProperties()
        {
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
