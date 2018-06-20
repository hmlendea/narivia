using System.Xml.Serialization;

using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Gui.GuiElements.Enumerations;
using Narivia.Settings;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Notification bar GUI element.
    /// </summary>
    public class GuiNotificationBar : GuiElement
    {
        [XmlIgnore]
        public string ProvinceId { get; private set; }

        IGameManager game;

        public GuiNotificationBar(IGameManager game)
        {
            this.game = game;
        }

        // TODO: Handle this better
        /// <summary>
        /// Adds the notification.
        /// </summary>
        /// <param name="icon">Icon.</param>
        public GuiNotificationIndicator AddNotification(NotificationIcon icon)
        {
            GuiNotificationIndicator notificationButton = new GuiNotificationIndicator
            {
                Icon = icon,
                Location = new Point2D(Location.X + GameDefines.GuiSpacing,
                                       Location.Y + Size.Height - (Children.Count + 1) * (GameDefines.GuiTileSize + GameDefines.GuiSpacing)),
                Size = new Size2D(GameDefines.GuiTileSize, GameDefines.GuiTileSize)
            };

            notificationButton.LoadContent();
            AddChild(notificationButton);

            return notificationButton;
        }

        public void Clear()
        {
            Children.ForEach(x => x.Dispose());
        }
        
        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Location = new Point2D(Location.X + GameDefines.GuiSpacing,
                                                 Location.Y + Size.Height - (i + 1) * (GameDefines.GuiTileSize + GameDefines.GuiSpacing));
            }
        }
    }
}
