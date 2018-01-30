using System.Collections.Generic;

using NuciXNA.Gui;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Gui.Screens;
using NuciXNA.Primitives;

using Narivia.Gui.GuiElements;
using Narivia.Gui.GuiElements.Enumerations;

namespace Narivia.Gui
{
    /// <summary>
    /// Notification manager.
    /// </summary>
    public class NotificationManager
    {
        static volatile NotificationManager instance;
        static object syncRoot = new object();

        /// <summary>
        /// Gets or sets the GUI elements.
        /// </summary>
        /// <value>The GUI elements.</value>
        public List<GuiElement> GuiElements { get; set; }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static NotificationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new NotificationManager();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Shows a notification.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="type">Type.</param>
        /// <param name="style">Style.</param>
        /// <param name="size">Size.</param>
        public void ShowNotification(string title, string text, NotificationType type, NotificationStyle style, Size2D size)
        {
            GuiNotificationDialog notification = new GuiNotificationDialog
            {
                Title = title,
                Text = text,
                Type = type,
                Style = style,
                Location = new Point2D((ScreenManager.Instance.Size.Width - size.Width) / 2,
                                       (ScreenManager.Instance.Size.Height - size.Height) / 2),
                Size = size
            };

            notification.LoadContent();

            GuiManager.Instance.GuiElements.Add(notification);
        }
    }
}
