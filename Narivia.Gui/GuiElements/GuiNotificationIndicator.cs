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
                SourceRectangle = new Rectangle2D(GameDefines.GUI_TILE_SIZE * 3, GameDefines.GUI_TILE_SIZE * 3,
                                                  GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };

            icon = new GuiImage
            {
                ContentFile = "Interface/notification_icons",
                SourceRectangle = CalculateIconSourceRectangle(Icon)
            };

            AddChild(background);
            AddChild(icon);

            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            background.Location = Location;
            icon.Location = new Point2D(Location.X + 6, Location.Y + 6);
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
