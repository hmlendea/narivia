using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;
using NuciXNA.Primitives;

using Narivia.Gui.Controls.Enumerations;
using Narivia.Settings;

namespace Narivia.Gui.Controls
{
    /// <summary>
    /// Notification bar GUI element.
    /// </summary>
    public class GuiNotificationBar() : GuiControl, IGuiControl
    {
        List<GuiNotificationIndicator> indicators;

        public string ProvinceId { get; private set; }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            indicators = [];

            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent() { }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime) => SetChildrenProperties();

        /// <summary>
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch) { }

        // TODO: Handle this better
        /// <summary>
        /// Adds the notification.
        /// </summary>
        /// <param name="icon">Icon.</param>
        public GuiNotificationIndicator AddNotification(NotificationIcon icon)
        {
            GuiNotificationIndicator indicator = new()
            {
                Id = $"{Id}_{indicators.Count}_{icon}",
                Icon = icon,
                Size = new Size2D(Size.Width, Size.Width)
            };

            indicator.LoadContent();
            indicators.Add(indicator);

            RegisterChild(indicator);

            return indicator;
        }

        public void Clear() => indicators.ForEach(x => x.Dispose());

        void SetChildrenProperties()
        {
            int i = 0;

            foreach (GuiNotificationIndicator indicator in indicators)
            {
                indicator.Location = new Point2D(
                    0,
                    Size.Height - (i + 1) * indicator.Size.Height);

                i += 1;
            }
        }
    }
}
