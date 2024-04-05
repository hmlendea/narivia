using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.Controls;
using NuciXNA.Input;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers;
using Narivia.Models;
using Narivia.Settings;

namespace Narivia.Gui.Controls
{
    public class GuiRecruitmentPanel : GuiPanel
    {
        readonly IGameManager gameManager;
        readonly IWorldManager worldManager;
        readonly IMilitaryManager militaryManager;

        World world;

        GuiImage unitBackground;
        GuiImage unitImage;
        GuiImage paper;
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

        List<Unit> units;

        int currentUnitIndex;
        int troopsAmount;

        public GuiRecruitmentPanel(
            IGameManager gameManager,
            IWorldManager worldManager,
            IMilitaryManager militaryManager)
        {
            this.gameManager = gameManager;
            this.worldManager = worldManager;
            this.militaryManager = militaryManager;

            Title = "Recruitment";
            FontName = "ButtonFont";
        }

        protected override void DoLoadContent()
        {
            base.DoLoadContent();

            world = gameManager.GetWorld();

            units = militaryManager.GetUnits().OrderBy(u => u.Price).ToList();

            unitBackground = new GuiImage
            {
                Id = $"{Id}_{nameof(unitBackground)}",
                ContentFile = "ScreenManager/FillImage",
                TextureLayout = TextureLayout.Tile,
                TintColour = Colour.Black,
                Size = new Size2D(100, 100),
                Location = new Point2D((Size.Width - 100) / 2, 64)
            };
            unitImage = new GuiImage
            {
                Id = $"{Id}_{nameof(unitImage)}",
                ContentFile = $"World/Assets/{world.AssetsPack}/units/{units[currentUnitIndex].Id}",
                SourceRectangle = new Rectangle2D(0, 0, 64, 64),
                Size = new Size2D(64, 64),
                Location = new Point2D(
                    unitBackground.Location.X + (unitBackground.Size.Width - 64) / 2,
                    unitBackground.Location.Y + (unitBackground.Size.Height - 64) / 2)
            };
            paper = new GuiImage
            {
                Id = $"{Id}_{nameof(paper)}",
                ContentFile = "Interface/ProvincePanel/paper",
                Size = new Size2D(248, 80),
                Location = new Point2D(
                    (Size.Width - 248) / 2,
                    unitBackground.ClientRectangle.Bottom + GameDefines.GuiSpacing)
            };

            unitText = new GuiText
            {
                Id = $"{Id}_{nameof(unitText)}",
                ForegroundColour = Colour.Gold,
                Size = new Size2D(unitBackground.Size.Width, 18),
                Location = unitBackground.Location
            };

            healthIcon = new GuiImage
            {
                Id = $"{Id}_{nameof(healthIcon)}",
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(0, 0, GameDefines.GuiIconSize, GameDefines.GuiIconSize),
                Size = new Size2D(GameDefines.GuiIconSize),
                Location = new Point2D(
                    paper.Location.X + GameDefines.GuiSpacing * 2,
                    paper.Location.Y + GameDefines.GuiSpacing * 2)
            };
            powerIcon = new GuiImage
            {
                Id = $"{Id}_{nameof(powerIcon)}",
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(GameDefines.GuiIconSize, 0, GameDefines.GuiIconSize, GameDefines.GuiIconSize),
                Size = new Size2D(GameDefines.GuiIconSize),
                Location = new Point2D(
                    paper.ClientRectangle.Left + GameDefines.GuiSpacing * 2,
                    paper.ClientRectangle.Bottom - GameDefines.GuiSpacing * 2 - GameDefines.GuiIconSize)
            };
            priceIcon = new GuiImage
            {
                Id = $"{Id}_{nameof(priceIcon)}",
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(GameDefines.GuiIconSize * 3, 0, GameDefines.GuiIconSize, GameDefines.GuiIconSize),
                Size = new Size2D(GameDefines.GuiIconSize),
                Location = new Point2D(
                    paper.ClientRectangle.Right - GameDefines.GuiSpacing * 2 - GameDefines.GuiIconSize,
                    paper.ClientRectangle.Top + GameDefines.GuiSpacing * 2)
            };
            maintenanceIcon = new GuiImage
            {
                Id = $"{Id}_{nameof(maintenanceIcon)}",
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(GameDefines.GuiIconSize * 3, 0, GameDefines.GuiIconSize, GameDefines.GuiIconSize),
                Size = new Size2D(GameDefines.GuiIconSize),
                Location = paper.ClientRectangle.BottomRight - new Point2D(
                    GameDefines.GuiSpacing * 2 + GameDefines.GuiIconSize,
                    GameDefines.GuiSpacing * 2 + GameDefines.GuiIconSize)
            };

            healthText = new GuiText
            {
                Id = $"{Id}_{nameof(healthText)}",
                HorizontalAlignment = Alignment.Beginning,
                Size = new Size2D(
                    healthIcon.Size.Width * 2,
                    healthIcon.Size.Height),
                Location = healthIcon.Location + new Point2D(healthIcon.Size.Width + GameDefines.GuiSpacing, 0)
            };
            powerText = new GuiText
            {
                Id = $"{Id}_{nameof(powerText)}",
                HorizontalAlignment = Alignment.Beginning,
                Size = new Size2D(
                    powerIcon.Size.Width * 2,
                    powerIcon.Size.Height),
                Location = powerIcon.Location + new Point2D(healthIcon.Size.Width + GameDefines.GuiSpacing, 0)
            };
            priceText = new GuiText
            {
                Id = $"{Id}_{nameof(priceText)}",
                HorizontalAlignment = Alignment.End,
                Size = new Size2D(
                    priceIcon.Size.Width * 2,
                    priceIcon.Size.Height),
                Location = priceIcon.Location - new Point2D(priceIcon.Size.Width * 2 + GameDefines.GuiSpacing, 0)
            };
            maintenanceText = new GuiText
            {
                Id = $"{Id}_{nameof(maintenanceText)}",
                HorizontalAlignment = Alignment.End,
                Size = new Size2D(
                    maintenanceIcon.Size.Width * 2,
                    maintenanceIcon.Size.Height),
                Location = maintenanceIcon.Location - new Point2D(maintenanceIcon.Size.Width * 2 + GameDefines.GuiSpacing, 0)
            };
            troopsText = new GuiText
            {
                Id = $"{Id}_{nameof(troopsText)}",
                ForegroundColour = Colour.Gold,
                Size = new Size2D(unitBackground.Size.Height, 18),
                Location = new Point2D(
                    unitBackground.ClientRectangle.Left,
                    unitBackground.ClientRectangle.Bottom - 18)
            };

            previousUnitButton = new GuiButton
            {
                Id = $"{Id}_{nameof(previousUnitButton)}",
                ContentFile = "Interface/Buttons/button-minus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    unitBackground.Location.X - GameDefines.GuiSpacing - 24,
                    unitBackground.Location.Y)
            };
            nextUnitButton = new GuiButton
            {
                Id = $"{Id}_{nameof(nextUnitButton)}",
                ContentFile = "Interface/Buttons/button-plus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    unitBackground.Location.X + unitBackground.Size.Width + GameDefines.GuiSpacing,
                    unitBackground.Location.Y)
            };
            substractUnitButton = new GuiButton
            {
                Id = $"{Id}_{nameof(substractUnitButton)}",
                ContentFile = "Interface/Buttons/button-minus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    unitBackground.Location.X - GameDefines.GuiSpacing - 24,
                    unitBackground.Location.Y + unitBackground.Size.Height - 24)
            };
            addUnitButton = new GuiButton
            {
                Id = $"{Id}_{nameof(addUnitButton)}",
                ContentFile = "Interface/Buttons/button-plus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    unitBackground.Location.X + unitBackground.Size.Width + GameDefines.GuiSpacing,
                    unitBackground.Location.Y + unitBackground.Size.Height - 24)
            };

            recruitButton = new GuiButton
            {
                Id = $"{Id}_{nameof(recruitButton)}",
                ContentFile = "Interface/Buttons/green-button-large",
                ForegroundColour = Colour.White,
                Size = new Size2D(128, 26),
                Location = new Point2D(
                    (Size.Width - 128) / 2,
                    Size.Height - 42 - GameDefines.GuiSpacing)
            };

            RegisterChildren(unitBackground, unitImage, paper);
            RegisterChildren(unitText, troopsText);
            RegisterChildren(healthIcon, powerIcon, priceIcon, maintenanceIcon);
            RegisterChildren(healthText, powerText, priceText, maintenanceText);
            RegisterChildren(nextUnitButton, previousUnitButton, addUnitButton, substractUnitButton, recruitButton);

            RegisterEvents();
            SetChildrenProperties();
        }

        protected override void DoUnloadContent()
        {
            base.DoUnloadContent();

            UnregisterEvents();
        }

        protected override void DoUpdate(GameTime gameTime)
        {
            base.DoUpdate(gameTime);

            SetChildrenProperties();
        }

        void RegisterEvents()
        {
            addUnitButton.Clicked += OnAddUnitButtonClicked;
            nextUnitButton.Clicked += OnNextUnitButtonClicked;
            previousUnitButton.Clicked += OnPreviousUnitButtonClicked;
            recruitButton.Clicked += OnRecruitButtonClick;
            substractUnitButton.Clicked += OnSusbstractUnitButtonClicked;
        }

        void UnregisterEvents()
        {
            addUnitButton.Clicked -= OnAddUnitButtonClicked;
            nextUnitButton.Clicked -= OnNextUnitButtonClicked;
            previousUnitButton.Clicked -= OnPreviousUnitButtonClicked;
            recruitButton.Clicked -= OnRecruitButtonClick;
            substractUnitButton.Clicked -= OnSusbstractUnitButtonClicked;
        }

        void SetChildrenProperties()
        {
            Unit unit = units[currentUnitIndex];

            unitText.Text = unit.Name;
            troopsText.Text = $"x{troopsAmount}";
            healthText.Text = unit.Health.ToString();
            powerText.Text = unit.Power.ToString();
            priceText.Text = unit.Price.ToString();
            maintenanceText.Text = unit.Maintenance.ToString();
            recruitButton.Text = $"Recruit ({unit.Price * troopsAmount}g)";

            unitImage.ContentFile = $"World/Assets/{world.AssetsPack}/units/{unit.Id}";
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
            int wealth = worldManager.GetFaction(gameManager.PlayerFactionId).Wealth;

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

        void OnRecruitButtonClick(object sender, MouseButtonEventArgs e)
        {
            militaryManager.RecruitUnits(gameManager.PlayerFactionId, units[currentUnitIndex].Id, troopsAmount);
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
