using System.Collections.Generic;

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
        List<GuiNotificationIndicator> indicators;
        
        public string ProvinceId { get; private set; }

        IGameManager game;

        public GuiNotificationBar(IGameManager game)
        {
            this.game = game;
        }

        public override void LoadContent()
        {
            indicators = new List<GuiNotificationIndicator>();

            base.LoadContent();
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
                Location = new Point2D(
                    GameDefines.GuiSpacing,
                    Size.Height - (indicators.Count + 1) * (GameDefines.GuiTileSize + GameDefines.GuiSpacing)),
                Size = new Size2D(GameDefines.GuiTileSize, GameDefines.GuiTileSize)
            };

            notificationButton.LoadContent();
            indicators.Add(notificationButton);
            AddChild(notificationButton);

            return notificationButton;
        }

        public void Clear()
        {
            indicators.ForEach(x => x.Dispose());
        }
        
        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            int i = 0;
            foreach(GuiNotificationIndicator indicator in indicators)
            {
                indicator.Location = new Point2D(
                    GameDefines.GuiSpacing,
                    Size.Height - (i + 1) * (GameDefines.GuiTileSize + GameDefines.GuiSpacing));

                i += 1;
            }
        }
    }
}
