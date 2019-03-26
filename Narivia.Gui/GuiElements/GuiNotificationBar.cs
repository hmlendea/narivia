using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers;
using Narivia.Gui.Controls.Enumerations;
using Narivia.Settings;

namespace Narivia.Gui.Controls
{
    /// <summary>
    /// Notification bar GUI element.
    /// </summary>
    public class GuiNotificationBar : GuiControl, IGuiControl
    {
        List<GuiNotificationIndicator> indicators;
        
        public string ProvinceId { get; private set; }

        IGameManager game;

        public GuiNotificationBar(IGameManager game)
        {
            this.game = game;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            indicators = new List<GuiNotificationIndicator>();

            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {

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

        // TODO: Handle this better
        /// <summary>
        /// Adds the notification.
        /// </summary>
        /// <param name="icon">Icon.</param>
        public GuiNotificationIndicator AddNotification(NotificationIcon icon)
        {
            GuiNotificationIndicator notificationButton = new GuiNotificationIndicator
            {
                Id = $"{Id}_{indicators.Count}_{icon.ToString()}",
                Icon = icon,
                Location = new Point2D(
                    GameDefines.GuiSpacing,
                    Size.Height - (indicators.Count + 1) * (GameDefines.GuiTileSize + GameDefines.GuiSpacing)),
                Size = new Size2D(GameDefines.GuiTileSize, GameDefines.GuiTileSize)
            };

            notificationButton.LoadContent();
            indicators.Add(notificationButton);
            RegisterChild(notificationButton);

            return notificationButton;
        }

        public void Clear()
        {
            indicators.ForEach(x => x.Dispose());
        }
        
        void SetChildrenProperties()
        {
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
