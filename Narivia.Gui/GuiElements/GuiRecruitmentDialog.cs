using System.Collections.Generic;
using System.Linq;

using NuciXNA.Graphics.Enumerations;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers.Interfaces;
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

        public GuiRecruitmentDialog(IGameManager game)
        {
            this.game = game;
            ForegroundColour = Colour.Gold;
        }

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
                Size = new Size2D(100, 100)
            };
            unitImage = new GuiImage
            {
                ContentFile = $"World/Assets/{game.GetWorld().Id}/units/{units[currentUnitIndex].Id}",
                SourceRectangle = new Rectangle2D(0, 0, 64, 64),
                Size = new Size2D(64, 64)
            };

            unitText = new GuiText
            {
                Size = new Size2D(unitBackground.Size.Width, 18),
                FontName = "InfoBarFont"
            };

            healthIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(0, 0, 22, 22),
                Size = new Size2D(22, 22)
            };
            powerIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(22, 0, 22, 22),
                Size = new Size2D(22, 22)
            };
            priceIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(66, 0, 22, 22),
                Size = new Size2D(22, 22)
            };
            maintenanceIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(66, 0, 22, 22),
                Size = new Size2D(22, 22)
            };

            healthText = new GuiText
            {
                Size = new Size2D(healthIcon.SourceRectangle.Width * 2, healthIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            powerText = new GuiText
            {
                Size = new Size2D(powerIcon.SourceRectangle.Width * 2, powerIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            priceText = new GuiText
            {
                Size = new Size2D(priceIcon.SourceRectangle.Width * 2, priceIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            maintenanceText = new GuiText
            {
                Size = new Size2D(maintenanceIcon.SourceRectangle.Width * 2, maintenanceIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            troopsText = new GuiText
            {
                Size = new Size2D(unitBackground.Size.Height, 18),
                FontName = "InfoBarFont"
            };

            nextUnitButton = new GuiButton
            {
                Text = ">",
                Size = new Size2D(GameDefines.GuiTileSize, GameDefines.GuiTileSize)
            };
            previousUnitButton = new GuiButton
            {
                Text = "<",
                Size = new Size2D(GameDefines.GuiTileSize, GameDefines.GuiTileSize)
            };
            addUnitButton = new GuiButton
            {
                Text = "+",
                Size = new Size2D(GameDefines.GuiTileSize, GameDefines.GuiTileSize)
            };
            substractUnitButton = new GuiButton
            {
                Text = "-",
                Size = new Size2D(GameDefines.GuiTileSize, GameDefines.GuiTileSize)
            };
            recruitButton = new GuiButton
            {
                Text = "Recruit",
                Size = new Size2D(GameDefines.GuiTileSize * 4, GameDefines.GuiTileSize)
            };
            cancelButton = new GuiButton
            {
                Text = "Cancel",
                Size = new Size2D(GameDefines.GuiTileSize * 2, GameDefines.GuiTileSize)
            };

            base.LoadContent();
        }

        protected override void RegisterChildren()
        {
            base.RegisterChildren();

            AddChild(background);
            AddChild(unitBackground);
            AddChild(unitImage);

            AddChild(unitText);
            AddChild(troopsText);

            AddChild(healthIcon);
            AddChild(powerIcon);
            AddChild(priceIcon);
            AddChild(maintenanceIcon);

            AddChild(healthText);
            AddChild(powerText);
            AddChild(priceText);
            AddChild(maintenanceText);

            AddChild(nextUnitButton);
            AddChild(previousUnitButton);
            AddChild(addUnitButton);
            AddChild(substractUnitButton);
            AddChild(recruitButton);
            AddChild(cancelButton);
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

            background.Size = Size;

            unitBackground.Location = new Point2D(
                (Size.Width - unitBackground.Size.Width) / 2,
                unitText.Size.Height + GameDefines.GuiSpacing);

            unitText.Location = unitBackground.Location;
            unitText.Text = units[currentUnitIndex].Name;

            troopsText.Location = new Point2D(
                unitBackground.ClientRectangle.Left,
                unitBackground.ClientRectangle.Bottom - troopsText.ClientRectangle.Height);
            troopsText.Text = $"x{troopsAmount}";

            healthIcon.Location = new Point2D(
                unitBackground.ClientRectangle.Left,
                unitBackground.ClientRectangle.Bottom + GameDefines.GuiSpacing);
            powerIcon.Location = new Point2D(
                healthIcon.ClientRectangle.Left,
                healthIcon.ClientRectangle.Bottom + GameDefines.GuiSpacing);
            priceIcon.Location = new Point2D(
                powerIcon.ClientRectangle.Left,
                powerIcon.ClientRectangle.Bottom + GameDefines.GuiSpacing);
            maintenanceIcon.Location = new Point2D(
                priceIcon.ClientRectangle.Left,
                priceIcon.ClientRectangle.Bottom + GameDefines.GuiSpacing);

            healthText.Location = new Point2D(
                healthIcon.ClientRectangle.Right + GameDefines.GuiSpacing,
                healthIcon.ClientRectangle.Top);
            healthText.Text = units[currentUnitIndex].Health.ToString();

            powerText.Location = new Point2D(
                powerIcon.ClientRectangle.Right + GameDefines.GuiSpacing,
                powerIcon.ClientRectangle.Top);
            powerText.Text = units[currentUnitIndex].Power.ToString();

            priceText.Location = new Point2D(
                priceIcon.ClientRectangle.Right + GameDefines.GuiSpacing,
                priceIcon.ClientRectangle.Top);
            priceText.Text = units[currentUnitIndex].Price.ToString();

            maintenanceText.Location = new Point2D(
                maintenanceIcon.ClientRectangle.Right + GameDefines.GuiSpacing,
                maintenanceIcon.ClientRectangle.Top);
            maintenanceText.Text = units[currentUnitIndex].Maintenance.ToString();

            previousUnitButton.Location = new Point2D(
                unitBackground.ClientRectangle.Left - previousUnitButton.ClientRectangle.Width - GameDefines.GuiSpacing,
                unitBackground.ClientRectangle.Top);

            nextUnitButton.Location = new Point2D(
                unitBackground.ClientRectangle.Right + GameDefines.GuiSpacing,
                unitBackground.ClientRectangle.Top);

            addUnitButton.Location = new Point2D(
                unitBackground.ClientRectangle.Right + GameDefines.GuiSpacing,
                unitBackground.ClientRectangle.Bottom - addUnitButton.ClientRectangle.Height);

            substractUnitButton.Location = new Point2D(
                unitBackground.ClientRectangle.Left - substractUnitButton.ClientRectangle.Width - GameDefines.GuiSpacing,
                unitBackground.ClientRectangle.Bottom - substractUnitButton.ClientRectangle.Height);

            recruitButton.Text = $"Recruit ({units[currentUnitIndex].Price * troopsAmount}g)";
            recruitButton.Location = new Point2D(
                GameDefines.GuiSpacing,
                Size.Height - recruitButton.Size.Height - GameDefines.GuiSpacing);

            cancelButton.Location = new Point2D(
                Size.Width - cancelButton.Size.Width - GameDefines.GuiSpacing,
                Size.Height - recruitButton.Size.Height - GameDefines.GuiSpacing);

            unitImage.ContentFile = $"World/Assets/{game.GetWorld().Id}/units/{units[currentUnitIndex].Id}";
            unitImage.Location = new Point2D(
                unitBackground.Location.X + (unitBackground.Size.Width - unitImage.SourceRectangle.Width) / 2,
                unitBackground.Location.Y + (unitBackground.Size.Height - unitImage.SourceRectangle.Height) / 2);
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
