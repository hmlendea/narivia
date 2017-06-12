using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics;
using Narivia.Models;

namespace Narivia.Interface.Widgets
{
    public class NotificationBar : Widget
    {
        [XmlIgnore]
        public string RegionId { get; private set; }

        IGameManager game;

        List<NotificationButton> notificationButtons;

        int spacing;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            spacing = 4;

            notificationButtons = new List<NotificationButton>();

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            notificationButtons.ForEach(x => x.UnloadContent());
            notificationButtons.Clear();

            base.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }

            notificationButtons.RemoveAll(x => x.Destroyed);

            AlignItems();

            notificationButtons.ForEach(x => x.Update(gameTime));

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }

            notificationButtons.ForEach(x => x.Draw(spriteBatch));

            base.Draw(spriteBatch);
        }

        // TODO: Handle this better
        /// <summary>
        /// Adds the notification.
        /// </summary>
        /// <param name="icon">Icon.</param>
        public NotificationButton AddNotification(NotificationIcon icon)
        {
            NotificationButton notificationButton = new NotificationButton
            {
                Position = new Vector2(Position.X + spacing,
                                       Position.Y + Size.Y - (notificationButtons.Count + 1) * (32 + spacing)),
                Icon = icon
            };

            notificationButtons.Add(notificationButton);
            notificationButton.LoadContent();

            return notificationButton;
        }

        public void Clear()
        {
            notificationButtons.ForEach(x => x.Destroy());
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
            for (int i = 0; i < notificationButtons.Count; i++)
            {
                notificationButtons[i].Position = new Vector2(
                    Position.X + spacing,
                    Position.Y + Size.Y - (i + 1) * (32 + spacing));
            }
        }
    }
}
