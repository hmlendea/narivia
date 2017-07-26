using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.GameLogic.Enumerations;
using Narivia.GameLogic.Events;
using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.GameLogic.GameManagers;
using Narivia.DataAccess.Exceptions;
using Narivia.Input.Events;
using Narivia.Gui;
using Narivia.Gui.GuiElements;
using Narivia.Gui.GuiElements.Enumerations;

namespace Narivia.Screens
{
    /// <summary>
    /// Gameplay screen.
    /// </summary>
    public class GameplayScreen : Screen
    {
        /// <summary>
        /// Gets or sets the game map.
        /// </summary>
        /// <value>The game map.</value>
        public GuiWorldmap GameMap { get; set; }

        /// <summary>
        /// Gets or sets the info bar.
        /// </summary>
        /// <value>The info bar.</value>
        public GuiInfoBar InfoBar { get; set; }

        /// <summary>
        /// Gets or sets the region bar.
        /// </summary>
        /// <value>The region bar.</value>
        public GuiRegionBar RegionBar { get; set; }

        /// <summary>
        /// Gets or sets the side bar.
        /// </summary>
        /// <value>The side bar.</value>
        public GuiSideBar SideBar { get; set; }

        /// <summary>
        /// Gets or sets the notification bar.
        /// </summary>
        /// <value>The notification bar.</value>
        public GuiNotificationBar NotificationBar { get; set; }

        IGameManager game;

        GuiRecruitmentDialog recruitmentDialog;
        GuiBuildingDialog buildDialog;

        Dictionary<string, int> troopsOld;
        Dictionary<string, int> relationsOld;
        int regionsOld, holdingsOld, wealthOld, incomeOld, outcomeOld, recruitmentOld;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            string initialWorldId = "narivia";
            string initialFactionId = "alpalet";

            if (ScreenArgs != null && ScreenArgs.Length >= 2)
            {
                initialWorldId = ScreenArgs[0];
                initialFactionId = ScreenArgs[1];
            }

            recruitmentDialog = new GuiRecruitmentDialog { Size = new Vector2(256, 288) };
            buildDialog = new GuiBuildingDialog { Size = new Vector2(256, 224) };

            recruitmentDialog.Hide();
            buildDialog.Hide();

            game = new GameManager();
            game.NewGame(initialWorldId, initialFactionId);

            troopsOld = new Dictionary<string, int>();
            relationsOld = new Dictionary<string, int>();

            game.GetUnits().ToList().ForEach(u => troopsOld.Add(u.Name, game.GetFactionArmySize(game.PlayerFactionId, u.Id)));
            game.GetFactionRelations(game.PlayerFactionId).ToList().ForEach(r => relationsOld.Add(r.TargetFactionId, r.Value));

            GameMap.AssociateGameManager(ref game);
            RegionBar.AssociateGameManager(ref game);
            SideBar.AssociateGameManager(ref game);
            recruitmentDialog.AssociateGameManager(ref game);
            buildDialog.AssociateGameManager(ref game);
            
            SideBar.FactionId = game.PlayerFactionId;

            GuiManager.Instance.GuiElements.Add(GameMap);
            GuiManager.Instance.GuiElements.Add(InfoBar);
            GuiManager.Instance.GuiElements.Add(RegionBar);
            GuiManager.Instance.GuiElements.Add(SideBar);
            GuiManager.Instance.GuiElements.Add(NotificationBar);
            GuiManager.Instance.GuiElements.Add(recruitmentDialog);
            GuiManager.Instance.GuiElements.Add(buildDialog);

            base.LoadContent();

            string factionName = game.GetFactionName(game.PlayerFactionId);

            ShowNotification($"Welcome to {game.WorldName}",
                             $"The era of peace has ended! Old rivalries remerged, and a global war broke out." + Environment.NewLine +
                             $"Conquer the world in the name of {factionName}, and secure its place in the golden pages of history!",
                             NotificationType.Informational,
                             NotificationStyle.Big,
                             new Vector2(256, 256));

            RegionBar.SetRegion(game.GetFactionCapital(game.PlayerFactionId));

            LinkEvents();

            GameMap.CentreCameraOnPosition(game.GetFactionCentreX(game.PlayerFactionId),
                                           game.GetFactionCentreY(game.PlayerFactionId));
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            Dictionary<string, int> troops = new Dictionary<string, int>();

            game.GetUnits().ToList().ForEach(u => troops.Add(u.Name, game.GetFactionArmySize(game.PlayerFactionId, u.Id)));

            recruitmentDialog.Position = new Vector2(GameMap.Position.X + (GameMap.Size.X - recruitmentDialog.Size.X) / 2,
                                                     GameMap.Position.Y + (GameMap.Size.Y - recruitmentDialog.Size.Y) / 2);
            buildDialog.Position = new Vector2(GameMap.Position.X + (GameMap.Size.X - buildDialog.Size.X) / 2,
                                               GameMap.Position.Y + (GameMap.Size.Y - buildDialog.Size.Y) / 2);

            InfoBar.Regions = game.GetFactionRegionsCount(game.PlayerFactionId);
            InfoBar.Holdings = game.GetFactionHoldingsCount(game.PlayerFactionId);
            InfoBar.Wealth = game.GetFactionWealth(game.PlayerFactionId);
            InfoBar.Troops = troops;

            if (!string.IsNullOrEmpty(GameMap.SelectedRegionId))
            {
                RegionBar.SetRegion(GameMap.SelectedRegionId);
            }
            
            SideBar.FactionId = game.PlayerFactionId;

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <returns>The draw.</returns>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        void LinkEvents()
        {
            game.PlayerRegionAttacked += game_OnPlayerRegionAttacked;
            game.FactionDestroyed += game_OnFactionDestroyed;
            game.FactionRevived += game_OnFactionRevived;
            game.FactionWon += game_OnFactionWon;

            GameMap.Clicked += GameMap_Clicked;

            SideBar.TurnButton.Clicked += SideBar_TurnButtonClicked;
            SideBar.StatsButton.Clicked += SideBar_StatsButtonClicked;
            SideBar.RecruitButton.Clicked += SideBar_RecruitButtonClicked;
            SideBar.BuildButton.Clicked += SideBar_BuildButtonClicked;
        }

        void NextTurn()
        {
            NotificationBar.Clear();

            game.NextTurn();

            Dictionary<string, int> troopsNew = new Dictionary<string, int>();
            Dictionary<string, int> relationsNew = new Dictionary<string, int>();

            game.GetUnits().ToList().ForEach(u => troopsNew.Add(u.Name, game.GetFactionArmySize(game.PlayerFactionId, u.Id)));
            game.GetFactionRelations(game.PlayerFactionId).ToList().ForEach(r => relationsNew.Add(r.TargetFactionId, r.Value));

            int regionsNew = game.GetFactionRegionsCount(game.PlayerFactionId);
            int holdingsNew = game.GetFactionHoldingsCount(game.PlayerFactionId);
            int wealthNew = game.GetFactionWealth(game.PlayerFactionId);
            int incomeNew = game.GetFactionIncome(game.PlayerFactionId);
            int outcomeNew = game.GetFactionOutcome(game.PlayerFactionId);
            int recruitmentNew = game.GetFactionRecruitment(game.PlayerFactionId);

            string recruitmentBody = string.Empty;
            string relationsBody = string.Empty;
            string turnBody = $"Regions: {regionsNew} ({(regionsNew - regionsOld).ToString("+0;-#")})" + Environment.NewLine +
                              $"Holdings: {holdingsNew} ({(holdingsNew - holdingsOld).ToString("+0;-#")})" + Environment.NewLine +
                              $"Wealth: {wealthNew} ({(wealthNew - wealthOld).ToString("+0;-#")})" + Environment.NewLine +
                              $"Income: {incomeNew} ({(incomeNew - incomeOld).ToString("+0;-#")})" + Environment.NewLine +
                              $"Outcome: {outcomeNew} ({(outcomeNew - outcomeOld).ToString("+0;-#")})" + Environment.NewLine +
                              $"Militia Recruitment: {recruitmentNew} ({(recruitmentNew - recruitmentOld).ToString("+0;-#")})";


            foreach (string key in troopsNew.Keys)
            {
                recruitmentBody += $"{key}: {troopsNew[key]} ({(troopsNew[key] - troopsOld[key]).ToString("+0;-#")})" + Environment.NewLine;
            }

            foreach (string targetfactionId in relationsNew.Keys.Where(key => relationsNew[key] != relationsOld[key]))
            {
                int delta = relationsNew[targetfactionId] - relationsOld[targetfactionId];

                relationsBody += $"{game.GetFactionName(targetfactionId)}: {relationsNew[targetfactionId].ToString("+0;-#")} " +
                                 $"({delta.ToString("+0;-#")})" + Environment.NewLine;
            }

            NotificationBar.AddNotification(NotificationIcon.TurnReport).Clicked += delegate
            {
                ShowNotification($"Turn {game.Turn} Report", turnBody, NotificationType.Informational, NotificationStyle.Big, new Vector2(256, 224));
            };

            NotificationBar.AddNotification(NotificationIcon.RecruitmentReport).Clicked += delegate
            {
                ShowNotification($"Recruitment Report", recruitmentBody, NotificationType.Informational, NotificationStyle.Big, new Vector2(256, 224));
            };

            if (!string.IsNullOrWhiteSpace(relationsBody))
            {
                NotificationBar.AddNotification(NotificationIcon.RelationsReport).Clicked += delegate
                {
                    ShowNotification($"Relations Report", relationsBody, NotificationType.Informational, NotificationStyle.Big, new Vector2(256, 196));
                };
            }

            troopsOld = troopsNew;
            relationsOld = relationsNew;

            regionsOld = regionsNew;
            holdingsOld = holdingsNew;
            wealthOld = wealthNew;
            incomeOld = incomeNew;
            outcomeOld = outcomeNew;
            recruitmentOld = recruitmentNew;
        }

        void SideBar_TurnButtonClicked(object sender, MouseButtonEventArgs e)
        {
            NextTurn();
        }

        void SideBar_StatsButtonClicked(object sender, MouseButtonEventArgs e)
        {
            ShowNotification("Statistics",
                             $"Income: {game.GetFactionIncome(game.PlayerFactionId)}" + Environment.NewLine +
                             $"Outcome: {game.GetFactionOutcome(game.PlayerFactionId)}" + Environment.NewLine +
                             $"Militia Recruitment: {game.GetFactionRecruitment(game.PlayerFactionId)}",
                             NotificationType.Informational,
                             NotificationStyle.Big,
                             new Vector2(256, 160));
        }

        void SideBar_RecruitButtonClicked(object sender, MouseButtonEventArgs e)
        {
            recruitmentDialog.Show();
        }

        void SideBar_BuildButtonClicked(object sender, MouseButtonEventArgs e)
        {
            buildDialog.Show();
        }

        void GameMap_Clicked(object sender, MouseButtonEventArgs e)
        {
            string regionId = GameMap.SelectedRegionId;

            if (string.IsNullOrEmpty(regionId))
            {
                return;
            }

            if (game.GetFactionTroopsCount(game.PlayerFactionId) < game.MinTroopsPerAttack)
            {
                ShowNotification($"Not enough troops!",
                                 $"Sorry!" + Environment.NewLine + Environment.NewLine +
                                 $"You do need at least {game.MinTroopsPerAttack} troops to attack any region.",
                                 NotificationType.Informational,
                                 NotificationStyle.Big,
                                 new Vector2(256, 192));

                return;
            }

            try
            {
                string regionName = game.GetRegionName(regionId);
                string defenderFactionId = game.GetRegionFaction(regionId);
                string defenderFactionName = game.GetFactionName(defenderFactionId);

                BattleResult result = game.PlayerAttackRegion(regionId);

                NextTurn();

                if (result == BattleResult.Victory)
                {
                    NotificationBar.AddNotification(NotificationIcon.BattleVictory).Clicked += delegate
                        {
                            ShowNotification($"Victory in {regionName}!",
                                             $"Good news!" + Environment.NewLine + Environment.NewLine +
                                             $"Our troops attacking {defenderFactionName} in {regionName} " +
                                             $"have managed to break the defence and occupy the region!",
                                             NotificationType.Informational,
                                             NotificationStyle.Big,
                                             new Vector2(256, 224));
                        };
                }
                else
                {
                    NotificationBar.AddNotification(NotificationIcon.BattleDefeat).Clicked += delegate
                        {
                            ShowNotification($"Defeat in {regionName}!",
                                             $"Bad news!" + Environment.NewLine + Environment.NewLine +
                                             $"Our troops attacking {defenderFactionName} in {regionName} " +
                                             $"were defeated by the defending forces!",
                                             NotificationType.Informational,
                                             NotificationStyle.Big,
                                             new Vector2(256, 224));
                        };
                }
            }
            catch (InvalidTargetRegionException)
            {
                ShowNotification($"Invalid target!",
                                 $"Sorry!" + Environment.NewLine + Environment.NewLine +
                                 $"You have chosen an invalid target that cannot be attacked.",
                                 NotificationType.Informational,
                                 NotificationStyle.Big,
                                 new Vector2(256, 192));
            }
        }

        void game_OnPlayerRegionAttacked(object sender, BattleEventArgs e)
        {
            string regionName = game.GetRegionName(e.RegionId);
            string attackerFactionName = game.GetFactionName(e.AttackerFactionId);

            if (e.BattleResult == BattleResult.Victory)
            {
                NotificationBar.AddNotification(NotificationIcon.RegionLost).Clicked += delegate
                    {
                        ShowNotification($"{regionName} region lost!",
                                         $"Bad news!" + Environment.NewLine + Environment.NewLine +
                                         $"One of our regions, {regionName}, was attacked by {attackerFactionName}, " +
                                         $"who managed to break the defence and occupy it!",
                                         NotificationType.Informational,
                                         NotificationStyle.Big,
                                         new Vector2(256, 224));
                    };
            }
            else
            {
                NotificationBar.AddNotification(NotificationIcon.RegionDefended).Clicked += delegate
                    {
                        ShowNotification($"{regionName} region defended!",
                                         $"Important news!" + Environment.NewLine + Environment.NewLine +
                                         $"One of our regions, {regionName}, was attacked by {attackerFactionName}, " +
                                         $"but our brave troops managed to sucesfully defend it!",
                                         NotificationType.Informational,
                                         NotificationStyle.Big,
                                         new Vector2(256, 224));
                    };
            }
        }

        void game_OnFactionDestroyed(object sender, FactionEventArgs e)
        {
            string factionName = game.GetFactionName(e.FactionId);

            NotificationBar.AddNotification(NotificationIcon.FactionDestroyed).Clicked += delegate
                {
                    ShowNotification($"{factionName} destroyed!",
                                     $"A significant development in the war just took place, " +
                                     $"as {factionName} was destroyed!",
                                     NotificationType.Informational,
                                     NotificationStyle.Big,
                                     new Vector2(256, 192));
                };
        }

        void game_OnFactionRevived(object sender, FactionEventArgs e)
        {
            string factionName = game.GetFactionName(e.FactionId);

            NotificationBar.AddNotification(NotificationIcon.FactionDestroyed).Clicked += delegate
                {
                    ShowNotification($"{factionName} revived!",
                                     $"A significant development in the war just took place, " +
                                     $"as {factionName} was revived!",
                                     NotificationType.Informational,
                                     NotificationStyle.Big,
                                     new Vector2(256, 192));
                };
        }

        void game_OnFactionWon(object sender, FactionEventArgs e)
        {
            string factionName = game.GetFactionName(e.FactionId);

            NotificationBar.AddNotification(NotificationIcon.GameFinished).Clicked += delegate
                {
                    ShowNotification($"{factionName} has won!",
                                     $"The war is over!" + Environment.NewLine +
                                     $"{factionName} has conquered the world and established a new world order!",
                                     NotificationType.Informational,
                                     NotificationStyle.Big,
                                     new Vector2(256, 192));
                };
        }
    }
}

