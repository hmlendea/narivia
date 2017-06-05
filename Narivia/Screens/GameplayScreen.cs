using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.BusinessLogic.GameManagers;
using Narivia.Interface.Widgets;
using Narivia.WorldMap;

namespace Narivia.Screens
{
    /// <summary>
    /// Gameplay screen.
    /// </summary>
    public class GameplayScreen : Screen
    {
        public GameMap GameMap { get; set; }

        public InfoBar InfoBar { get; set; }

        public SideBar SideBar { get; set; }

        GameDomainService game;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            game = new GameDomainService();
            game.NewGame("narivia", "alpalet");

            GameMap.AssociateGameDomainService(ref game);

            SideBar.WorldId = game.WorldId;
            SideBar.FactionId = game.PlayerFactionId;
            SideBar.FactionName = game.GetFactionName(game.PlayerFactionId);

            GameMap.LoadContent();
            InfoBar.LoadContent();
            SideBar.LoadContent();

            base.LoadContent();

            SideBar.TurnButtonClicked += SideBar_TurnButtonClicked;
            SideBar.StatsButtonClicked += SideBar_StatsButtonClicked;
            SideBar.RelationsButtonClicked += SideBar_RelationsButtonClicked;

            ShowNotification("Welcome",
                             $"Welcome to the world of {game.WorldName} " +
                             Environment.NewLine +
                             Environment.NewLine +
                             "This is still a very WIP project !!!",
                             NotificationType.Informational,
                             NotificationStyle.Big,
                             new Vector2(256, 128));
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            GameMap.UnloadContent();
            InfoBar.UnloadContent();
            SideBar.UnloadContent();

            base.UnloadContent();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            int playerSize = game.GetFactionSize(game.PlayerFactionId);
            int playerWealth = game.GetFactionWealth(game.PlayerFactionId);
            int playerTroops = game.GetFactionTroops(game.PlayerFactionId);

            InfoBar.RealmSize = playerSize;
            InfoBar.Wealth = playerWealth;
            InfoBar.Troops = playerTroops;

            GameMap.Update(gameTime);
            InfoBar.Update(gameTime);
            SideBar.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <returns>The draw.</returns>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            GameMap.Draw(spriteBatch);
            InfoBar.Draw(spriteBatch);
            SideBar.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        void SideBar_TurnButtonClicked(object sender, EventArgs e)
        {
            // TODO: Pass the turn
        }

        void SideBar_StatsButtonClicked(object sender, EventArgs e)
        {
            ShowNotification("Statistics",
                             "Comming soon :)",
                             NotificationType.Informational,
                             NotificationStyle.Big,
                             new Vector2(256, 128));
        }

        void SideBar_RelationsButtonClicked(object sender, EventArgs e)
        {
            ShowNotification("Diplomatic Relations",
                             "Comming soon :)",
                             NotificationType.Informational,
                             NotificationStyle.Big,
                             new Vector2(256, 128));
        }
    }
}

