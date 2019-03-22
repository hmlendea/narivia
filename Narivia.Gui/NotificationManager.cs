using System.Collections.Generic;

using NuciXNA.Gui;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Gui.Screens;
using NuciXNA.Primitives;

using Narivia.Gui.GuiElements;

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
        public void ShowNotification(string title, string text)
        {
            GuiNotificationDialog notification = new GuiNotificationDialog
            {
                Id = $"{nameof(notification)}_{title.ToLower().Replace(' ', '-').Replace('\t', '-')}",
                Title = title,
                Text = text
            };

            notification.Location = new Point2D(
                    (ScreenManager.Instance.Size.Width - notification.Size.Width) / 2,
                    (ScreenManager.Instance.Size.Height - notification.Size.Height) / 2);

            notification.LoadContent();

            GuiManager.Instance.GuiElements.Add(notification);
        }
    }
}
