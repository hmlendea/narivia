using System.Xml.Serialization;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics.Geometry;
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
        public string RegionId { get; private set; }

        IGameManager game;

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
                Location = new Point2D(Location.X + GameDefines.GUI_SPACING,
                                       Location.Y + Size.Height - (Children.Count + 1) * (GameDefines.GUI_TILE_SIZE + GameDefines.GUI_SPACING)),
                Size = new Size2D(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };

            notificationButton.LoadContent();
            Children.Add(notificationButton);

            return notificationButton;
        }

        public void Clear()
        {
            Children.ForEach(x => x.Dispose());
        }
        
        // TODO: Handle this better
        /// <summary>
        /// Associates the game manager.
        /// </summary>
        /// <param name="game">Game.</param>
        public void AssociateGameManager(ref IGameManager game)
        {
            this.game = game;
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Location = new Point2D(Location.X + GameDefines.GUI_SPACING,
                                                 Location.Y + Size.Height - (i + 1) * (GameDefines.GUI_TILE_SIZE + GameDefines.GUI_SPACING));
            }
        }
    }
}
