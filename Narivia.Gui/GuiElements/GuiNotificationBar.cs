using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Gui.GuiElements.Enumerations;

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

        int spacing;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            spacing = 4;

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            AlignItems();

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
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
                Position = new Vector2(Position.X + spacing,
                                       Position.Y + Size.Y - (Children.Count + 1) * (32 + spacing)),
                Icon = icon
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

        void AlignItems()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Position = new Vector2(
                    Position.X + spacing,
                    Position.Y + Size.Y - (i + 1) * (32 + spacing));
            }
        }
    }
}
