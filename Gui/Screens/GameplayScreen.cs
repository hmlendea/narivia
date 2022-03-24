using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Gui;
using NuciXNA.Gui.Screens;
using NuciXNA.Input;
using NuciXNA.Primitives;

using Narivia.GameLogic.Enumerations;
using Narivia.GameLogic.Exceptions;
using Narivia.GameLogic.Events;
using Narivia.GameLogic.GameManagers;
using Narivia.Gui.Controls;
using Narivia.Gui.Controls.Enumerations;
using Narivia.Models;

namespace Narivia.Gui.Screens
{
    /// <summary>
    /// Gameplay screen.
    /// </summary>
    public class GameplayScreen : Screen
    {
        IAttackManager AttackManager;
        IDiplomacyManager DiplomacyManager;
        IEconomyManager EconomyManager;
        IHoldingManager HoldingManager;
        IMilitaryManager MilitaryManager;
        IWorldManager WorldManager;
        IGameManager GameManager;

        GuiWorldmap gameMap;
        GuiAdministrationBar administrationBar;
        GuiInfoBar infoBar;
        GuiFactionBar factionBar;
        GuiProvincePanel provincePanel;
        GuiNotificationBar notificationBar;
        GuiRecruitmentPanel recruitmentPanel;
        GuiBuildingPanel buildingPanel;

        Dictionary<string, int> troopsOld;
        Dictionary<string, int> relationsOld;
        int provincesOld, holdingsOld, wealthOld, incomeOld, outcomeOld, recruitmentOld;

        string worldId;
        string playerFactionId;

        public GameplayScreen(string worldId, string playerFactionId)
        {
            this.worldId = worldId;
            this.playerFactionId = playerFactionId;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            LoadGameManagers();

            administrationBar = new GuiAdministrationBar
            {
                Id = nameof(administrationBar),
                Location = Point2D.Empty
            };
            infoBar = new GuiInfoBar(
                GameManager,
                WorldManager,
                HoldingManager,
                MilitaryManager)
            {
                Id = nameof(infoBar),
                Location = new Point2D(ScreenManager.Instance.Size.Width - 166, 0),
                Size = new Size2D(166, 82)
            };
            factionBar = new GuiFactionBar(GameManager)
            {
                Id = nameof(factionBar),
                Location = new Point2D((ScreenManager.Instance.Size.Width - 242) / 2, 0),
                Size = new Size2D(242, 94)
            };
            gameMap = new GuiWorldmap(GameManager, WorldManager)
            {
                Id = nameof(gameMap),
                Size = ScreenManager.Instance.Size
            };
            notificationBar = new GuiNotificationBar(GameManager)
            {
                Id = nameof(notificationBar),
                Location = new Point2D(
                    ScreenManager.Instance.Size.Width - 48,
                    infoBar.Size.Height),
                Size = new Size2D(48, ScreenManager.Instance.Size.Height - infoBar.Size.Height)
            };
            provincePanel = new GuiProvincePanel(GameManager, WorldManager, HoldingManager)
            {
                Id = nameof(provincePanel),
                Location = new Point2D(0, 296)
            };

            recruitmentPanel = new GuiRecruitmentPanel(GameManager, WorldManager, MilitaryManager)
            {
                Id = nameof(recruitmentPanel)
            };
            buildingPanel = new GuiBuildingPanel(GameManager, WorldManager, HoldingManager)
            {
                Id = nameof(buildingPanel)
            };

            troopsOld = new Dictionary<string, int>();
            troopsOld = MilitaryManager.GetUnits().ToDictionary(x => x.Name, x => MilitaryManager.GetArmy(playerFactionId, x.Id).Size);

            LoadRelations();

            GuiManager.Instance.RegisterControls(
                gameMap,
                administrationBar,
                infoBar,
                factionBar,
                provincePanel,
                notificationBar,
                recruitmentPanel,
                buildingPanel);

            RegisterEvents();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            UnloadGameManagers();
            UnregisterEvents();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            playerFactionId = GameManager.PlayerFactionId;

            recruitmentPanel.Location = new Point2D(
                gameMap.Location.X + (gameMap.Size.Width - recruitmentPanel.Size.Width) / 2,
                gameMap.Location.Y + (gameMap.Size.Height - recruitmentPanel.Size.Height) / 2);
            buildingPanel.Location = new Point2D(
                gameMap.Location.X + (gameMap.Size.Width - buildingPanel.Size.Width) / 2,
                gameMap.Location.Y + (gameMap.Size.Height - buildingPanel.Size.Height) / 2);
        }
        
        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }

        /// <summary>
        /// Loads the game managers.
        /// </summary>
        void LoadGameManagers()
        {
            WorldManager = new WorldManager(worldId);
            DiplomacyManager = new DiplomacyManager(WorldManager);
            HoldingManager = new HoldingManager(worldId, WorldManager);
            MilitaryManager = new MilitaryManager(HoldingManager, WorldManager);
            EconomyManager = new EconomyManager(HoldingManager, MilitaryManager, WorldManager);
            AttackManager = new AttackManager(DiplomacyManager, HoldingManager, MilitaryManager, WorldManager);
            GameManager = new GameManager(AttackManager, DiplomacyManager, EconomyManager, HoldingManager, MilitaryManager, WorldManager);

            WorldManager.LoadContent();
            DiplomacyManager.LoadContent();
            HoldingManager.LoadContent();
            MilitaryManager.LoadContent();
            EconomyManager.LoadContent();
            AttackManager.LoadContent();
            GameManager.LoadContent(worldId, playerFactionId);
        }
        
        /// <summary>
        /// Loads the relations.
        /// </summary>
        void LoadRelations()
        {
            relationsOld = new Dictionary<string, int>();

            foreach (Relation relation in DiplomacyManager.GetFactionRelations(playerFactionId))
            {
                if (relationsOld.ContainsKey(relation.TargetFactionId))
                {
                    relationsOld[relation.TargetFactionId] = relation.Value;
                }
                else
                {
                    relationsOld.Add(relation.TargetFactionId, relation.Value);
                }
            }
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            ContentLoaded += OnContentLoaded;

            GameManager.PlayerProvinceAttacked += OnGamePlayerProvinceAttacked;
            GameManager.FactionDestroyed += OnGameFactionDestroyed;
            GameManager.FactionRevived += OnGameFactionRevived;
            GameManager.FactionWon += OnGameFactionWon;

            gameMap.Clicked += OnGameMapClicked;

            infoBar.TurnButtonClicked += OnSideBarTurnButtonClicked;
            administrationBar.BuildButtonClicked += OnAdministrationBarBuildButtonClicked;
            administrationBar.RecruitButtonClicked += OnAdministrationBarRecruitButtonClicked;
            administrationBar.StatsButtonClicked += OnAdministrationBarStatsButtonClicked;
            provincePanel.AttackButtonClicked += OnProvincePanelAttackButtonClicked;
            provincePanel.BuildButtonClicked += OnProvincePanelBuildButtonClicked;
        }

        /// <summary>
        /// Unloads the game managers.
        /// </summary>
        void UnloadGameManagers()
        {
            WorldManager.UnloadContent();
            DiplomacyManager.UnloadContent();
            HoldingManager.UnloadContent();
            MilitaryManager.UnloadContent();
            EconomyManager.UnloadContent();
            AttackManager.UnloadContent();
            GameManager.UnloadContent();
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            ContentLoaded -= OnContentLoaded;

            GameManager.PlayerProvinceAttacked -= OnGamePlayerProvinceAttacked;
            GameManager.FactionDestroyed -= OnGameFactionDestroyed;
            GameManager.FactionRevived -= OnGameFactionRevived;
            GameManager.FactionWon -= OnGameFactionWon;

            gameMap.Clicked -= OnGameMapClicked;

            infoBar.TurnButtonClicked -= OnSideBarTurnButtonClicked;
            administrationBar.BuildButtonClicked -= OnAdministrationBarBuildButtonClicked;
            administrationBar.RecruitButtonClicked -= OnAdministrationBarRecruitButtonClicked;
            administrationBar.StatsButtonClicked -= OnAdministrationBarStatsButtonClicked;
            provincePanel.AttackButtonClicked -= OnProvincePanelAttackButtonClicked;
            provincePanel.BuildButtonClicked -= OnProvincePanelBuildButtonClicked;
        }

        void NextTurn()
        {
            notificationBar.Clear();

            GameManager.PassTurn();

            Dictionary<string, int> troopsNew = new Dictionary<string, int>();
            Dictionary<string, int> relationsNew = new Dictionary<string, int>();

            MilitaryManager.GetUnits().ToList().ForEach(u => troopsNew.Add(u.Name, MilitaryManager.GetArmy(playerFactionId, u.Id).Size));

            foreach (Relation relation in DiplomacyManager.GetFactionRelations(playerFactionId))
            {
                if (relationsNew.ContainsKey(relation.TargetFactionId))
                {
                    relationsNew[relation.TargetFactionId] = relation.Value;
                }
                else
                {
                    relationsNew.Add(relation.TargetFactionId, relation.Value);
                }
            }

            int provincesNew = WorldManager.GetFactionProvinces(playerFactionId).Count();
            int holdingsNew = HoldingManager.GetFactionHoldings(playerFactionId).Count();
            int wealthNew = WorldManager.GetFaction(playerFactionId).Wealth;
            int incomeNew = EconomyManager.GetFactionIncome(playerFactionId);
            int outcomeNew = EconomyManager.GetFactionOutcome(playerFactionId);
            int recruitmentNew = MilitaryManager.GetFactionRecruitment(playerFactionId);

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

                relationsBody += $"{WorldManager.GetFaction(targetfactionId).Name}: {relationsNew[targetfactionId].ToString("+0;-#")} " +
                                 $"({delta.ToString("+0;-#")})" + Environment.NewLine;
            }

            notificationBar.AddNotification(NotificationIcon.TurnReport).Clicked += delegate
            {
                NotificationManager.Instance.ShowNotification($"Turn {GameManager.Turn} Report", turnBody);
            };

            notificationBar.AddNotification(NotificationIcon.RecruitmentReport).Clicked += delegate
            {
                NotificationManager.Instance.ShowNotification($"Recruitment Report", recruitmentBody);
            };

            if (!string.IsNullOrWhiteSpace(relationsBody))
            {
                notificationBar.AddNotification(NotificationIcon.RelationsReport).Clicked += delegate
                {
                    NotificationManager.Instance.ShowNotification($"Relations Report", relationsBody);
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

        void AttackProvince(string provinceId)
        {
            if (MilitaryManager.GetFactionTroopsAmount(playerFactionId) < GameManager.GetWorld().MinTroopsPerAttack)
            {
                NotificationManager.Instance.ShowNotification(
                    $"Not enough troops!",
                    $"Sorry!" + Environment.NewLine + Environment.NewLine +
                    $"You do need at least {GameManager.GetWorld().MinTroopsPerAttack} troops to attack any province.");

                return;
            }

            try
            {
                Province province = WorldManager.GetProvince(provinceId);

                string provinceName = province.Name;
                string defenderFactionName = WorldManager.GetFaction(province.FactionId).Name;

                BattleResult result = GameManager.AttackProvinceAsPlayer(provinceId);

                NextTurn();

                if (result == BattleResult.Victory)
                {
                    notificationBar.AddNotification(NotificationIcon.BattleVictory).Clicked += delegate
                    {
                        NotificationManager.Instance.ShowNotification(
                            $"Victory in {provinceName}!",
                            $"Good news!" + Environment.NewLine + Environment.NewLine +
                            $"Our troops attacking {defenderFactionName} in {provinceName} " +
                            $"have managed to break the defence and occupy the province!");
                    };
                }
                else
                {
                    notificationBar.AddNotification(NotificationIcon.BattleDefeat).Clicked += delegate
                    {
                        NotificationManager.Instance.ShowNotification(
                            $"Defeat in {provinceName}!",
                            $"Bad news!" + Environment.NewLine + Environment.NewLine +
                            $"Our troops attacking {defenderFactionName} in {provinceName} " +
                            $"were defeated by the defending forces!");
                    };
                }
            }
            catch (InvalidTargetProvinceException)
            {
                NotificationManager.Instance.ShowNotification(
                    $"Invalid target!",
                    $"Sorry!" + Environment.NewLine + Environment.NewLine +
                    $"You have chosen an invalid target that cannot be attacked.");
            }
        }

        void OnContentLoaded(object sender, EventArgs e)
        {
            provincePanel.ProvinceId = GameManager.GetFactionCapital(playerFactionId).Id;

            provincePanel.Hide();
            recruitmentPanel.Hide();
            buildingPanel.Hide();

            string factionName = WorldManager.GetFaction(playerFactionId).Name;

            NotificationManager.Instance.ShowNotification(
                $"Welcome to {GameManager.GetWorld().Name}",
                $"The era of peace has ended! Old rivalries remerged, and a global war broke out." + Environment.NewLine +
                $"Conquer the world in the name of {factionName}, and secure its place in the golden pages of history!");

            gameMap.CentreCameraOnLocation(WorldManager.GetFactionCentre(playerFactionId));
        }

        void OnSideBarTurnButtonClicked(object sender, MouseButtonEventArgs e)
        {
            NextTurn();
        }

        void OnAdministrationBarStatsButtonClicked(object sender, MouseButtonEventArgs e)
        {
            NotificationManager.Instance.ShowNotification(
                "Statistics",
                $"Income: {EconomyManager.GetFactionIncome(playerFactionId)}" + Environment.NewLine +
                $"Outcome: {EconomyManager.GetFactionOutcome(playerFactionId)}" + Environment.NewLine +
                $"Militia Recruitment: {MilitaryManager.GetFactionRecruitment(playerFactionId)}");
        }

        void OnAdministrationBarBuildButtonClicked(object sender, MouseButtonEventArgs e)
        {
            buildingPanel.Show();
        }

        void OnAdministrationBarRecruitButtonClicked(object sender, MouseButtonEventArgs e)
        {
            recruitmentPanel.Show();
        }

        void OnProvincePanelAttackButtonClicked(object sender, MouseButtonEventArgs e)
        {
            AttackProvince(provincePanel.ProvinceId);
        }

        void OnProvincePanelBuildButtonClicked(object sender, MouseButtonEventArgs e)
        {
            buildingPanel.Show();
        }

        void OnGameMapClicked(object sender, MouseButtonEventArgs e)
        {
            string provinceId = gameMap.SelectedProvinceId;

            if (string.IsNullOrEmpty(provinceId))
            {
                return;
            }

            provincePanel.ProvinceId = provinceId;
            provincePanel.Show();
        }

        void OnGamePlayerProvinceAttacked(object sender, BattleEventArgs e)
        {
            string provinceName = WorldManager.GetProvince(e.ProvinceId).Name;
            string attackerFactionName = WorldManager.GetFaction(e.AttackerFactionId).Name;

            if (e.BattleResult == BattleResult.Victory)
            {
                notificationBar.AddNotification(NotificationIcon.ProvinceLost).Clicked += delegate
                {
                    NotificationManager.Instance.ShowNotification(
                        $"{provinceName} province lost!",
                        $"Bad news!" + Environment.NewLine + Environment.NewLine +
                        $"One of our provinces, {provinceName}, was attacked by {attackerFactionName}, " +
                        $"who managed to break the defence and occupy it!");
                };
            }
            else
            {
                notificationBar.AddNotification(NotificationIcon.ProvinceDefended).Clicked += delegate
                {
                    NotificationManager.Instance.ShowNotification(
                        $"{provinceName} province defended!",
                        $"Important news!" + Environment.NewLine + Environment.NewLine +
                        $"One of our provinces, {provinceName}, was attacked by {attackerFactionName}, " +
                        $"but our brave troops managed to sucesfully defend it!");
                };
            }
        }

        void OnGameFactionDestroyed(object sender, FactionEventArgs e)
        {
            string factionName = WorldManager.GetFaction(e.FactionId).Name;

            notificationBar.AddNotification(NotificationIcon.FactionDestroyed).Clicked += delegate
            {
                NotificationManager.Instance.ShowNotification(
                    $"{factionName} destroyed!",
                    $"A significant development in the war just took place, " +
                    $"as {factionName} was destroyed!");
            };
        }

        void OnGameFactionRevived(object sender, FactionEventArgs e)
        {
            string factionName = WorldManager.GetFaction(e.FactionId).Name;

            notificationBar.AddNotification(NotificationIcon.FactionDestroyed).Clicked += delegate
            {
                NotificationManager.Instance.ShowNotification(
                    $"{factionName} revived!",
                    $"A significant development in the war just took place, " +
                    $"as {factionName} was revived!");
            };
        }

        void OnGameFactionWon(object sender, FactionEventArgs e)
        {
            string factionName = WorldManager.GetFaction(e.FactionId).Name;

            notificationBar.AddNotification(NotificationIcon.GameFinished).Clicked += delegate
            {
                NotificationManager.Instance.ShowNotification(
                    $"{factionName} has won!",
                    $"The war is over!" + Environment.NewLine +
                    $"{factionName} has conquered the world and established a new world order!");
            };
        }
    }
}

