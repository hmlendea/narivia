using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics.Enumerations;
using Narivia.Input.Events;
using Narivia.Models;
using Narivia.Settings;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Unit recruitment dialog GUI element.
    /// </summary>
    public class GuiRecruitmentDialog : GuiElement
    {
        IGameManager game;

        GuiImage background;
        GuiImage unitBackground;
        GuiImage unitImage;
        GuiText unitText;
        GuiText troopsText;

        GuiImage healthIcon;
        GuiImage powerIcon;
        GuiImage priceIcon;
        GuiImage maintenanceIcon;

        GuiText healthText;
        GuiText powerText;
        GuiText priceText;
        GuiText maintenanceText;

        GuiButton previousUnitButton;
        GuiButton nextUnitButton;
        GuiButton addUnitButton;
        GuiButton substractUnitButton;

        GuiButton recruitButton;
        GuiButton cancelButton;

        List<Unit> units;

        int currentUnitIndex;
        int troopsAmount;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            units = game.GetUnits().OrderBy(u => u.Price).ToList();

            background = new GuiImage
            {
                ContentFile = "Interface/Backgrounds/stone-bricks",
                TextureLayout = TextureLayout.Tile
            };
            unitBackground = new GuiImage
            {
                ContentFile = "ScreenManager/FillImage",
                TextureLayout = TextureLayout.Tile,
                Size = new Point(100, 100)
            };
            unitImage = new GuiImage
            {
                ContentFile = $"World/Assets/{game.GetWorld().Id}/units/{units[currentUnitIndex].Id}",
                SourceRectangle = new Rectangle(0, 0, 64, 64)
            };

            unitText = new GuiText
            {
                Size = new Point(unitBackground.Size.X, 18),
                FontName = "InfoBarFont"
            };

            healthIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(64, 0, 16, 16)
            };
            powerIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(80, 0, 16, 16)
            };
            priceIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(16, 0, 16, 16)
            };
            maintenanceIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(96, 0, 16, 16)
            };

            healthText = new GuiText
            {
                Size = new Point(healthIcon.SourceRectangle.Width * 2, healthIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            powerText = new GuiText
            {
                Size = new Point(powerIcon.SourceRectangle.Width * 2, powerIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            priceText = new GuiText
            {
                Size = new Point(priceIcon.SourceRectangle.Width * 2, priceIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            maintenanceText = new GuiText
            {
                Size = new Point(maintenanceIcon.SourceRectangle.Width * 2, maintenanceIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            troopsText = new GuiText
            {
                Size = new Point(unitBackground.Size.Y, 18),
                FontName = "InfoBarFont"
            };

            nextUnitButton = new GuiButton
            {
                Text = ">",
                Size = new Point(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            previousUnitButton = new GuiButton
            {
                Text = "<",
                Size = new Point(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            addUnitButton = new GuiButton
            {
                Text = "+",
                Size = new Point(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            substractUnitButton = new GuiButton
            {
                Text = "-",
                Size = new Point(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            recruitButton = new GuiButton
            {
                Text = "Recruit",
                Size = new Point(GameDefines.GUI_TILE_SIZE * 4, GameDefines.GUI_TILE_SIZE)
            };
            cancelButton = new GuiButton
            {
                Text = "Cancel",
                Size = new Point(GameDefines.GUI_TILE_SIZE * 2, GameDefines.GUI_TILE_SIZE)
            };

            Children.Add(background);
            Children.Add(unitBackground);
            Children.Add(unitImage);

            Children.Add(unitText);
            Children.Add(troopsText);

            Children.Add(healthIcon);
            Children.Add(powerIcon);
            Children.Add(priceIcon);
            Children.Add(maintenanceIcon);

            Children.Add(healthText);
            Children.Add(powerText);
            Children.Add(priceText);
            Children.Add(maintenanceText);

            Children.Add(nextUnitButton);
            Children.Add(previousUnitButton);
            Children.Add(addUnitButton);
            Children.Add(substractUnitButton);
            Children.Add(recruitButton);
            Children.Add(cancelButton);

            base.LoadContent();
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

        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            addUnitButton.Clicked += OnAddUnitButtonClicked;
            cancelButton.Clicked += OnCancelButtonClick;
            nextUnitButton.Clicked += OnNextUnitButtonClicked;
            previousUnitButton.Clicked += OnPreviousUnitButtonClicked;
            recruitButton.Clicked += OnRecruitButtonClick;
            substractUnitButton.Clicked += OnSusbstractUnitButtonClicked;
        }

        protected override void UnregisterEvents()
        {
            base.UnregisterEvents();

            addUnitButton.Clicked -= OnAddUnitButtonClicked;
            cancelButton.Clicked -= OnCancelButtonClick;
            nextUnitButton.Clicked -= OnNextUnitButtonClicked;
            previousUnitButton.Clicked -= OnPreviousUnitButtonClicked;
            recruitButton.Clicked -= OnRecruitButtonClick;
            substractUnitButton.Clicked -= OnSusbstractUnitButtonClicked;
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            background.Position = Position;
            background.Size = Size;

            unitBackground.ForegroundColour = ForegroundColour;
            unitBackground.Position = new Point(Position.X + (Size.X - unitBackground.Size.X) / 2,
                                                Position.Y + unitText.Size.Y + GameDefines.GUI_SPACING);

            unitText.Position = unitBackground.Position;
            unitText.Text = units[currentUnitIndex].Name;
            unitText.ForegroundColour = ForegroundColour;

            troopsText.Position = new Point(unitBackground.ClientRectangle.Left, unitBackground.ClientRectangle.Bottom - troopsText.ClientRectangle.Height);
            troopsText.Text = $"x{troopsAmount}";
            troopsText.ForegroundColour = ForegroundColour;

            healthIcon.Position = new Point(unitBackground.ClientRectangle.Left,
                                            unitBackground.ClientRectangle.Bottom + GameDefines.GUI_SPACING);
            powerIcon.Position = new Point(healthIcon.ClientRectangle.Left,
                                           healthIcon.ClientRectangle.Bottom + GameDefines.GUI_SPACING);
            priceIcon.Position = new Point(powerIcon.ClientRectangle.Left,
                                           powerIcon.ClientRectangle.Bottom + GameDefines.GUI_SPACING);
            maintenanceIcon.Position = new Point(priceIcon.ClientRectangle.Left,
                                                 priceIcon.ClientRectangle.Bottom + GameDefines.GUI_SPACING);

            healthText.ForegroundColour = ForegroundColour;
            healthText.Position = new Point(healthIcon.ClientRectangle.Right + GameDefines.GUI_SPACING,
                                            healthIcon.ClientRectangle.Top);
            healthText.Text = units[currentUnitIndex].Health.ToString();

            powerText.ForegroundColour = ForegroundColour;
            powerText.Position = new Point(powerIcon.ClientRectangle.Right + GameDefines.GUI_SPACING,
                                           powerIcon.ClientRectangle.Top);
            powerText.Text = units[currentUnitIndex].Power.ToString();

            priceText.ForegroundColour = ForegroundColour;
            priceText.Position = new Point(priceIcon.ClientRectangle.Right + GameDefines.GUI_SPACING,
                                           priceIcon.ClientRectangle.Top);
            priceText.Text = units[currentUnitIndex].Price.ToString();

            maintenanceText.ForegroundColour = ForegroundColour;
            maintenanceText.Position = new Point(maintenanceIcon.ClientRectangle.Right + GameDefines.GUI_SPACING,
                                                 maintenanceIcon.ClientRectangle.Top);
            maintenanceText.Text = units[currentUnitIndex].Maintenance.ToString();

            previousUnitButton.ForegroundColour = ForegroundColour;
            previousUnitButton.Position = new Point(unitBackground.ClientRectangle.Left - previousUnitButton.ClientRectangle.Width - GameDefines.GUI_SPACING,
                                                    unitBackground.ClientRectangle.Top);

            nextUnitButton.ForegroundColour = ForegroundColour;
            nextUnitButton.Position = new Point(unitBackground.ClientRectangle.Right + GameDefines.GUI_SPACING,
                                                unitBackground.ClientRectangle.Top);

            addUnitButton.ForegroundColour = ForegroundColour;
            addUnitButton.Position = new Point(unitBackground.ClientRectangle.Right + GameDefines.GUI_SPACING,
                                               unitBackground.ClientRectangle.Bottom - addUnitButton.ClientRectangle.Height);

            substractUnitButton.ForegroundColour = ForegroundColour;
            substractUnitButton.Position = new Point(unitBackground.ClientRectangle.Left - substractUnitButton.ClientRectangle.Width - GameDefines.GUI_SPACING,
                                                     unitBackground.ClientRectangle.Bottom - substractUnitButton.ClientRectangle.Height);

            recruitButton.ForegroundColour = ForegroundColour;
            recruitButton.Text = $"Recruit ({units[currentUnitIndex].Price * troopsAmount}g)";
            recruitButton.Position = new Point(Position.X + GameDefines.GUI_SPACING,
                                               Position.Y + Size.Y - recruitButton.Size.Y - GameDefines.GUI_SPACING);

            cancelButton.ForegroundColour = ForegroundColour;
            cancelButton.Position = new Point(Position.X + Size.X - cancelButton.Size.X - GameDefines.GUI_SPACING,
                                              Position.Y + Size.Y - recruitButton.Size.Y - GameDefines.GUI_SPACING);

            unitImage.ContentFile = $"World/Assets/{game.GetWorld().Id}/units/{units[currentUnitIndex].Id}";
            unitImage.Position = new Point(unitBackground.Position.X + (unitBackground.Size.X - unitImage.SourceRectangle.Width) / 2,
                                           unitBackground.Position.Y + (unitBackground.Size.Y - unitImage.SourceRectangle.Height) / 2);
        }

        void SelectUnit(int index)
        {
            if (index > units.Count - 1)
            {
                currentUnitIndex = 0;
            }
            else if (index < 0)
            {
                currentUnitIndex = units.Count - 1;
            }
            else
            {
                currentUnitIndex = index;
            }
        }

        void AddTroops(int delta)
        {
            int wealth = game.GetFaction(game.PlayerFactionId).Wealth;

            troopsAmount += delta;

            if (troopsAmount < 0)
            {
                troopsAmount = 0;
            }
            else if (wealth < units[currentUnitIndex].Price * troopsAmount)
            {
                troopsAmount = wealth / units[currentUnitIndex].Price;
            }
        }

        void OnCancelButtonClick(object sender, MouseButtonEventArgs e)
        {
            Hide();
            SelectUnit(0);
            troopsAmount = 0;
        }

        void OnRecruitButtonClick(object sender, MouseButtonEventArgs e)
        {
            game.RecruitUnits(game.PlayerFactionId, units[currentUnitIndex].Id, troopsAmount);
        }

        void OnAddUnitButtonClicked(object sender, MouseButtonEventArgs e)
        {
            AddTroops(1);
        }

        void OnSusbstractUnitButtonClicked(object sender, MouseButtonEventArgs e)
        {
            AddTroops(-1);
        }

        void OnPreviousUnitButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectUnit(currentUnitIndex - 1);
        }

        void OnNextUnitButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectUnit(currentUnitIndex + 1);
        }
    }
}
