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

        public RegionBar RegionBar { get; set; }

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

            RegionBar.GameManager = game;

            GameMap.LoadContent();
            InfoBar.LoadContent();
            RegionBar.LoadContent();
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

            RegionBar.SetRegion(game.GetFactionCapital(game.PlayerFactionId));
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            GameMap.UnloadContent();
            InfoBar.UnloadContent();
            RegionBar.UnloadContent();
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
            InfoBar.Regions = game.GetFactionRegionsCount(game.PlayerFactionId);
            InfoBar.Holdings = game.GetFactionHoldingsCount(game.PlayerFactionId);
            InfoBar.Wealth = game.GetFactionWealth(game.PlayerFactionId);
            InfoBar.Troops = game.GetFactionTroopsCount(game.PlayerFactionId);

            if (!string.IsNullOrEmpty(GameMap.SelectedRegionId))
            {
                RegionBar.SetRegion(GameMap.SelectedRegionId);
            }

            SideBar.Turn = game.Turn;
            SideBar.FactionId = game.PlayerFactionId;
            SideBar.FactionName = game.GetFactionName(game.PlayerFactionId);

            GameMap.Update(gameTime);
            InfoBar.Update(gameTime);
            RegionBar.Update(gameTime);
            SideBar.Update(gameTime);

            base.Update(gameTime);

            Console.WriteLine(GameMap.SelectedRegionId);
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
            RegionBar.Draw(spriteBatch);
            SideBar.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        void SideBar_TurnButtonClicked(object sender, EventArgs e)
        {
            // TODO: Pass the turn
            game.NextTurn();
        }

        void SideBar_StatsButtonClicked(object sender, EventArgs e)
        {
            ShowNotification("Statistics",
                             $"Income: {game.GetFactionIncome(game.PlayerFactionId)}" + Environment.NewLine +
                             $"Outcome: {game.GetFactionOutcome(game.PlayerFactionId)}" + Environment.NewLine +
                             $"Militia Recruitment: {game.GetFactionRecruitment(game.PlayerFactionId)}",
                             NotificationType.Informational,
                             NotificationStyle.Big,
                             new Vector2(256, 160));
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

