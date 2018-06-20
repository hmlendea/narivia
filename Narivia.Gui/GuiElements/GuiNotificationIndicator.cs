using NuciXNA.Gui.GuiElements;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;

using Narivia.Audio;
using Narivia.Gui.GuiElements.Enumerations;
using Narivia.Settings;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Notification indicator GUI element.
    /// </summary>
    public class GuiNotificationIndicator : GuiElement
    {
        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public NotificationIcon Icon { get; set; }

        GuiImage background;
        GuiImage icon;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            background = new GuiImage
            {
                ContentFile = "Interface/notification_small",
                SourceRectangle = new Rectangle2D(
                    GameDefines.GuiTileSize * 3, GameDefines.GuiTileSize * 3,
                    GameDefines.GuiTileSize, GameDefines.GuiTileSize)
            };

            icon = new GuiImage
            {
                ContentFile = "Interface/notification_icons",
                SourceRectangle = CalculateIconSourceRectangle(Icon),
                Location = new Point2D(6, 6)
            };

            AddChild(background);
            AddChild(icon);

            base.LoadContent();
        }

        Rectangle2D CalculateIconSourceRectangle(NotificationIcon notificationIcon)
        {
            return new Rectangle2D((int)notificationIcon % 8 * 20,
                                   (int)notificationIcon / 8 * 20,
                                   20,
                                   20);
        }

        /// <summary>
        /// Fired by the Clicked event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void OnClicked(object sender, MouseButtonEventArgs e)
        {
            base.OnClicked(sender, e);

            AudioManager.Instance.PlaySound("Interface/click");
            Dispose();
        }

        /// <summary>
        /// Fired by the MouseEntered event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void OnMouseEntered(object sender, MouseEventArgs e)
        {
            AudioManager.Instance.PlaySound("Interface/select");
        }
    }
}
