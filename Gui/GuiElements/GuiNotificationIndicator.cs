using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;
using NuciXNA.Input;
using NuciXNA.Primitives;

using Narivia.Audio;
using Narivia.Gui.Controls.Enumerations;

namespace Narivia.Gui.Controls
{
    /// <summary>
    /// Notification indicator GUI element.
    /// </summary>
    public class GuiNotificationIndicator : GuiControl, IGuiControl
    {
        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public NotificationIcon Icon { get; set; }

        GuiImage border;
        GuiImage icon;

        public GuiNotificationIndicator()
        {
            Size = new Size2D(64, 64);
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            border = new GuiImage
            {
                Id = $"{Id}_{nameof(border)}",
                ContentFile = "Interface/Notification/indicator_border",
                SourceRectangle = new Rectangle2D(0, 0, 256, 256),
                Size = Size
            };
            icon = new GuiImage
            {
                Id = $"{Id}_{nameof(icon)}",
                ContentFile = "Interface/Notification/icons",
                SourceRectangle = CalculateIconSourceRectangle(Icon),
                Size = new Size2D(border.Size.Width - 8, border.Size.Height - 8),
                Location = new Point2D(4, 4)
            };

            RegisterChildren(icon, border);
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent() { }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime) { }

        /// <summary>
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch) { }

        void RegisterEvents()
        {
            Clicked += OnClicked;
            MouseEntered += OnMouseEntered;
        }

        void UnregisterEvents()
        {
            Clicked -= OnClicked;
            MouseEntered -= OnMouseEntered;
        }

        /// <summary>
        /// Fired by the Clicked event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnClicked(object sender, MouseButtonEventArgs e)
        {
            AudioManager.Instance.PlaySound("Interface/click");
            Dispose();
        }

        /// <summary>
        /// Fired by the MouseEntered event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnMouseEntered(object sender, MouseEventArgs e)
        {
            AudioManager.Instance.PlaySound("Interface/select");
        }

        Rectangle2D CalculateIconSourceRectangle(NotificationIcon notificationIcon)
        {
            return new Rectangle2D((int)notificationIcon % 8 * 256,
                                   (int)notificationIcon / 8 * 256,
                                   256,
                                   256);
        }
    }
}
