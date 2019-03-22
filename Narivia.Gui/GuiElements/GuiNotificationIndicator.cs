using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.GuiElements;
using NuciXNA.Input;
using NuciXNA.Primitives;

using Narivia.Audio;
using Narivia.Gui.GuiElements.Enumerations;

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

        public GuiNotificationIndicator()
        {
            Size = new Size2D(48, 48);
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            background = new GuiImage
            {
                ContentFile = "Interface/Notification/icon_background"
            };

            icon = new GuiImage
            {
                ContentFile = "Interface/Notification/icons",
                SourceRectangle = CalculateIconSourceRectangle(Icon),
                Location = new Point2D((Size.Width - 20) / 2, (Size.Height - 20) / 2),
                Size = new Size2D(20, 20)
            };
            
            RegisterChildren();
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

        }

        /// <summary>
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }

        void RegisterChildren()
        {
            AddChild(background);
            AddChild(icon);
        }

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
            return new Rectangle2D((int)notificationIcon % 8 * 20,
                                   (int)notificationIcon / 8 * 20,
                                   20,
                                   20);
        }
    }
}
