using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Gui;
using NuciXNA.Gui.Screens;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;

using Narivia.GameLogic.Enumerations;
using Narivia.GameLogic.Exceptions;
using Narivia.GameLogic.Events;
using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.GameLogic.GameManagers;
using Narivia.Gui.GuiElements;
using Narivia.Gui.GuiElements.Enumerations;
using Narivia.Models;

namespace Narivia.Gui.Screens
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

        public GuiAdministrationBar AdministrationBar { get; set; }

        /// <summary>
        /// Gets or sets the info bar.
        /// </summary>
        /// <value>The info bar.</value>
        public GuiInfoBar InfoBar { get; set; }

        /// <summary>
        /// Gets or sets the province bar.
        /// </summary>
        /// <value>The province bar.</value>
        public GuiProvinceBar ProvinceBar { get; set; }

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
        int provincesOld, holdingsOld, wealthOld, incomeOld, outcomeOld, recruitmentOld;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            string initialWorldId = "narivia";
            string initialFactionId = "f_alpalet";

            game = new GameManager(initialWorldId, initialFactionId);
            game.LoadContent();

            AdministrationBar = new GuiAdministrationBar
            {
                Location = Point2D.Empty,
            };
            InfoBar = new GuiInfoBar
            {
                Location = new Point2D(130, 0),
                Size = new Size2D(870, 20),
                BackgroundColour = Colour.Black
            };
            GameMap = new GuiWorldmap(game)
            {
                Location = new Point2D(0, 20),
                Size = new Size2D(1000, 640)
            };
            SideBar = new GuiSideBar(game)
            {
                Location = new Point2D(1000, 0),
                Size = new Size2D(280, 720),
                ForegroundColour = new Colour(255, 215, 0)
            };
            NotificationBar = new GuiNotificationBar(game)
            {
                Location = new Point2D(0, 20),
                Size = new Size2D(40, 610)
            };
            ProvinceBar = new GuiProvinceBar(game)
            {
                Location = new Point2D(0, 640),
                Size = new Size2D(1000, 80),
                ForegroundColour = new Colour(255, 215, 0)
            };

            if (ScreenArgs != null && ScreenArgs.Length >= 2)
            {
                initialWorldId = ScreenArgs[0];
                initialFactionId = ScreenArgs[1];
            }

            recruitmentDialog = new GuiRecruitmentDialog(game)
            {
                Size = new Size2D(256, 288)
            };
            buildDialog = new GuiBuildingDialog(game)
            {
                Size = new Size2D(256, 224)
            };

            recruitmentDialog.Hide();
            buildDialog.Hide();

            troopsOld = new Dictionary<string, int>();
            relationsOld = new Dictionary<string, int>();

            game.GetUnits().ToList().ForEach(u => troopsOld.Add(u.Name, game.GetArmy(game.PlayerFactionId, u.Id).Size));
            game.GetFactionRelations(game.PlayerFactionId).ToList().ForEach(r => relationsOld.Add(r.TargetFactionId, r.Value));
            
            SideBar.FactionId = game.PlayerFactionId;

            GuiManager.Instance.GuiElements.Add(GameMap);
            GuiManager.Instance.GuiElements.Add(InfoBar);
            GuiManager.Instance.GuiElements.Add(AdministrationBar);
            GuiManager.Instance.GuiElements.Add(ProvinceBar);
            GuiManager.Instance.GuiElements.Add(SideBar);
            GuiManager.Instance.GuiElements.Add(NotificationBar);
            GuiManager.Instance.GuiElements.Add(recruitmentDialog);
            GuiManager.Instance.GuiElements.Add(buildDialog);

            base.LoadContent();

            string factionName = game.GetFaction(game.PlayerFactionId).Name;

            NotificationManager.Instance.ShowNotification(
                            $"Welcome to {game.GetWorld().Name}",
                            $"The era of peace has ended! Old rivalries remerged, and a global war broke out." + Environment.NewLine +
                            $"Conquer the world in the name of {factionName}, and secure its place in the golden pages of history!",
                            NotificationType.Informational,
                            NotificationStyle.Big,
                            new Size2D(256, 256));

            ProvinceBar.SetProvince(game.GetFactionCapital(game.PlayerFactionId).Id);

            LinkEvents();

            GameMap.CentreCameraOnLocation(game.GetFactionCentreX(game.PlayerFactionId),
                                           game.GetFactionCentreY(game.PlayerFactionId));
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            Dictionary<string, int> troops = new Dictionary<string, int>();

            game.GetUnits().ToList().ForEach(u => troops.Add(u.Name, game.GetArmy(game.PlayerFactionId, u.Id).Size));

            recruitmentDialog.Location = new Point2D(
                GameMap.Location.X + (GameMap.Size.Width - recruitmentDialog.Size.Width) / 2,
                GameMap.Location.Y + (GameMap.Size.Height - recruitmentDialog.Size.Height) / 2);
            buildDialog.Location = new Point2D(
                GameMap.Location.X + (GameMap.Size.Width - buildDialog.Size.Width) / 2,
                GameMap.Location.Y + (GameMap.Size.Height - buildDialog.Size.Height) / 2);

            InfoBar.Provinces = game.GetFactionProvinces(game.PlayerFactionId).Count();
            InfoBar.Holdings = game.GetFactionHoldings(game.PlayerFactionId).Count();
            InfoBar.Wealth = game.GetFaction(game.PlayerFactionId).Wealth;
            InfoBar.Troops = troops;

            if (!string.IsNullOrEmpty(GameMap.SelectedProvinceId))
            {
                ProvinceBar.SetProvince(GameMap.SelectedProvinceId);
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
            game.PlayerProvinceAttacked += game_OnPlayerProvinceAttacked;
            game.FactionDestroyed += game_OnFactionDestroyed;
            game.FactionRevived += game_OnFactionRevived;
            game.FactionWon += game_OnFactionWon;

            GameMap.Clicked += GameMap_Clicked;

            SideBar.TurnButton.Clicked += SideBar_TurnButtonClicked;
            AdministrationBar.BuildButton.Clicked += AdministrationBar_BuildButtonClicked;
            AdministrationBar.RecruitButton.Clicked += AdministrationBar_RecruitButtonClicked;
            AdministrationBar.StatsButton.Clicked += AdministrationBar_StatsButtonClicked;
        }

        void NextTurn()
        {
            NotificationBar.Clear();

            game.NextTurn();

            Dictionary<string, int> troopsNew = new Dictionary<string, int>();
            Dictionary<string, int> relationsNew = new Dictionary<string, int>();

            game.GetUnits().ToList().ForEach(u => troopsNew.Add(u.Name, game.GetArmy(game.PlayerFactionId, u.Id).Size));
            game.GetFactionRelations(game.PlayerFactionId).ToList().ForEach(r => relationsNew.Add(r.TargetFactionId, r.Value));

            int provincesNew = game.GetFactionProvinces(game.PlayerFactionId).Count();
            int holdingsNew = game.GetFactionHoldings(game.PlayerFactionId).Count();
            int wealthNew = game.GetFaction(game.PlayerFactionId).Wealth;
            int incomeNew = game.GetFactionIncome(game.PlayerFactionId);
            int outcomeNew = game.GetFactionOutcome(game.PlayerFactionId);
            int recruitmentNew = game.GetFactionRecruitment(game.PlayerFactionId);

            string recruitmentBody = string.Empty;
            string relationsBody = string.Empty;
            string turnBody = $"Provinces: {provincesNew} ({(provincesNew - provincesOld).ToString("+0;-#")})" + Environment.NewLine +
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

                relationsBody += $"{game.GetFaction(targetfactionId).Name}: {relationsNew[targetfactionId].ToString("+0;-#")} " +
                                 $"({delta.ToString("+0;-#")})" + Environment.NewLine;
            }

            NotificationBar.AddNotification(NotificationIcon.TurnReport).Clicked += delegate
            {
                NotificationManager.Instance.ShowNotification($"Turn {game.Turn} Report", turnBody, NotificationType.Informational, NotificationStyle.Big, new Size2D(256, 224));
            };

            NotificationBar.AddNotification(NotificationIcon.RecruitmentReport).Clicked += delegate
            {
                NotificationManager.Instance.ShowNotification($"Recruitment Report", recruitmentBody, NotificationType.Informational, NotificationStyle.Big, new Size2D(256, 224));
            };

            if (!string.IsNullOrWhiteSpace(relationsBody))
            {
                NotificationBar.AddNotification(NotificationIcon.RelationsReport).Clicked += delegate
                {
                    NotificationManager.Instance.ShowNotification($"Relations Report", relationsBody, NotificationType.Informational, NotificationStyle.Big, new Size2D(256, 196));
                };
            }

            troopsOld = troopsNew;
            relationsOld = relationsNew;

            provincesOld = provincesNew;
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

        void AdministrationBar_StatsButtonClicked(object sender, MouseButtonEventArgs e)
        {
            NotificationManager.Instance.ShowNotification(
                "Statistics",
                $"Income: {game.GetFactionIncome(game.PlayerFactionId)}" + Environment.NewLine +
                $"Outcome: {game.GetFactionOutcome(game.PlayerFactionId)}" + Environment.NewLine +
                $"Militia Recruitment: {game.GetFactionRecruitment(game.PlayerFactionId)}",
                NotificationType.Informational,
                NotificationStyle.Big,
                new Size2D(256, 160));
        }

        void AdministrationBar_RecruitButtonClicked(object sender, MouseButtonEventArgs e)
        {
            recruitmentDialog.Show();
        }

        void AdministrationBar_BuildButtonClicked(object sender, MouseButtonEventArgs e)
        {
            buildDialog.Show();
        }

        void GameMap_Clicked(object sender, MouseButtonEventArgs e)
        {
            string provinceId = GameMap.SelectedProvinceId;

            if (string.IsNullOrEmpty(provinceId))
            {
                return;
            }

            if (game.GetFactionTroopsAmount(game.PlayerFactionId) < game.GetWorld().MinTroopsPerAttack)
            {
                NotificationManager.Instance.ShowNotification(
                    $"Not enough troops!",
                    $"Sorry!" + Environment.NewLine + Environment.NewLine +
                    $"You do need at least {game.GetWorld().MinTroopsPerAttack} troops to attack any province.",
                    NotificationType.Informational,
                    NotificationStyle.Big,
                    new Size2D(256, 192));

                return;
            }

            try
            {
                Province province = game.GetProvince(provinceId);

                string provinceName = province.Name;
                string defenderFactionName = game.GetFaction(province.FactionId).Name;

                BattleResult result = game.PlayerAttackProvince(provinceId);

                NextTurn();

                if (result == BattleResult.Victory)
                {
                    NotificationBar.AddNotification(NotificationIcon.BattleVictory).Clicked += delegate
                        {
                            NotificationManager.Instance.ShowNotification(
                                $"Victory in {provinceName}!",
                                $"Good news!" + Environment.NewLine + Environment.NewLine +
                                $"Our troops attacking {defenderFactionName} in {provinceName} " +
                                $"have managed to break the defence and occupy the province!",
                                NotificationType.Informational,
                                NotificationStyle.Big,
                                new Size2D(256, 224));
                        };
                }
                else
                {
                    NotificationBar.AddNotification(NotificationIcon.BattleDefeat).Clicked += delegate
                        {
                            NotificationManager.Instance.ShowNotification(
                                $"Defeat in {provinceName}!",
                                $"Bad news!" + Environment.NewLine + Environment.NewLine +
                                $"Our troops attacking {defenderFactionName} in {provinceName} " +
                                $"were defeated by the defending forces!",
                                NotificationType.Informational,
                                NotificationStyle.Big,
                                new Size2D(256, 224));
                        };
                }
            }
            catch (InvalidTargetProvinceException)
            {
                NotificationManager.Instance.ShowNotification(
                    $"Invalid target!",
                    $"Sorry!" + Environment.NewLine + Environment.NewLine +
                    $"You have chosen an invalid target that cannot be attacked.",
                    NotificationType.Informational,
                    NotificationStyle.Big,
                    new Size2D(256, 192));
            }
        }

        void game_OnPlayerProvinceAttacked(object sender, BattleEventArgs e)
        {
            string provinceName = game.GetProvince(e.ProvinceId).Name;
            string attackerFactionName = game.GetFaction(e.AttackerFactionId).Name;

            if (e.BattleResult == BattleResult.Victory)
            {
                NotificationBar.AddNotification(NotificationIcon.ProvinceLost).Clicked += delegate
                    {
                        NotificationManager.Instance.ShowNotification(
                            $"{provinceName} province lost!",
                            $"Bad news!" + Environment.NewLine + Environment.NewLine +
                            $"One of our provinces, {provinceName}, was attacked by {attackerFactionName}, " +
                            $"who managed to break the defence and occupy it!",
                            NotificationType.Informational,
                            NotificationStyle.Big,
                            new Size2D(256, 224));
                    };
            }
            else
            {
                NotificationBar.AddNotification(NotificationIcon.ProvinceDefended).Clicked += delegate
                    {
                        NotificationManager.Instance.ShowNotification(
                            $"{provinceName} province defended!",
                            $"Important news!" + Environment.NewLine + Environment.NewLine +
                            $"One of our provinces, {provinceName}, was attacked by {attackerFactionName}, " +
                            $"but our brave troops managed to sucesfully defend it!",
                            NotificationType.Informational,
                            NotificationStyle.Big,
                            new Size2D(256, 224));
                    };
            }
        }

        void game_OnFactionDestroyed(object sender, FactionEventArgs e)
        {
            string factionName = game.GetFaction(e.FactionId).Name;

            NotificationBar.AddNotification(NotificationIcon.FactionDestroyed).Clicked += delegate
                {
                    NotificationManager.Instance.ShowNotification(
                        $"{factionName} destroyed!",
                        $"A significant development in the war just took place, " +
                        $"as {factionName} was destroyed!",
                        NotificationType.Informational,
                        NotificationStyle.Big,
                        new Size2D(256, 192));
                };
        }

        void game_OnFactionRevived(object sender, FactionEventArgs e)
        {
            string factionName = game.GetFaction(e.FactionId).Name;

            NotificationBar.AddNotification(NotificationIcon.FactionDestroyed).Clicked += delegate
                {
                    NotificationManager.Instance.ShowNotification(
                        $"{factionName} revived!",
                        $"A significant development in the war just took place, " +
                        $"as {factionName} was revived!",
                        NotificationType.Informational,
                        NotificationStyle.Big,
                        new Size2D(256, 192));
                };
        }

        void game_OnFactionWon(object sender, FactionEventArgs e)
        {
            string factionName = game.GetFaction(e.FactionId).Name;

            NotificationBar.AddNotification(NotificationIcon.GameFinished).Clicked += delegate
                {
                    NotificationManager.Instance.ShowNotification(
                        $"{factionName} has won!",
                        $"The war is over!" + Environment.NewLine +
                        $"{factionName} has conquered the world and established a new world order!",
                        NotificationType.Informational,
                        NotificationStyle.Big,
                        new Size2D(256, 192));
                };
        }
    }
}

