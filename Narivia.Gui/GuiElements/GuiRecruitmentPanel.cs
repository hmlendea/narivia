using System.Collections.Generic;
using System.Linq;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Models;
using Narivia.Settings;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Unit recruitment panel GUI element.
    /// </summary>
    public class GuiRecruitmentPanel : GuiPanel
    {
        const int IconSize = 22;

        IGameManager game;
        
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

        public GuiRecruitmentPanel(IGameManager game)
        {
            this.game = game;
            
            Title = "Recruitment";
            FontName = "ButtonFont";
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            units = game.GetUnits().OrderBy(u => u.Price).ToList();
            
            unitBackground = new GuiImage
            {
                ContentFile = "ScreenManager/FillImage",
                TextureLayout = TextureLayout.Tile,
                TintColour = Colour.Black,
                Size = new Size2D(100, 100),
                Location = new Point2D((Size.Width - 100) / 2, 64)
            };
            unitImage = new GuiImage
            {
                ContentFile = $"World/Assets/{game.GetWorld().Id}/units/{units[currentUnitIndex].Id}",
                SourceRectangle = new Rectangle2D(0, 0, 64, 64),
                Size = new Size2D(64, 64),
                Location = new Point2D(
                    unitBackground.Location.X + (unitBackground.Size.Width - 64) / 2,
                    unitBackground.Location.Y + (unitBackground.Size.Height - 64) / 2)
            };
            paper = new GuiImage
            {
                ContentFile = "Interface/ProvincePanel/paper",
                Size = new Size2D(248, 80),
                Location = new Point2D(
                    (Size.Width - 248) / 2,
                    unitBackground.ClientRectangle.Bottom + GameDefines.GuiSpacing)
            };

            unitText = new GuiText
            {
                ForegroundColour = Colour.Gold,
                Size = new Size2D(unitBackground.Size.Width, 18),
                Location = unitBackground.Location
            };

            healthIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(0, 0, IconSize, IconSize),
                Size = new Size2D(IconSize, IconSize),
                Location = new Point2D(
                    paper.Location.X + GameDefines.GuiSpacing * 2,
                    paper.Location.Y + GameDefines.GuiSpacing * 2)
            };
            powerIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(IconSize, 0, IconSize, IconSize),
                Size = new Size2D(IconSize, IconSize),
                Location = new Point2D(
                    paper.ClientRectangle.Left + GameDefines.GuiSpacing * 2,
                    paper.ClientRectangle.Bottom - GameDefines.GuiSpacing * 2 - IconSize)
            };
            priceIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(IconSize * 3, 0, IconSize, IconSize),
                Size = new Size2D(IconSize, IconSize),
                Location = new Point2D(
                    paper.ClientRectangle.Right - GameDefines.GuiSpacing * 2 - IconSize,
                    paper.ClientRectangle.Top + GameDefines.GuiSpacing * 2)
            };
            maintenanceIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(IconSize * 3, 0, IconSize, IconSize),
                Size = new Size2D(IconSize, IconSize),
                Location = new Point2D(
                    paper.ClientRectangle.Right - GameDefines.GuiSpacing * 2 - IconSize,
                    paper.ClientRectangle.Bottom - GameDefines.GuiSpacing * 2 - IconSize)
            };

            healthText = new GuiText
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Size = new Size2D(
                    healthIcon.Size.Width * 2,
                    healthIcon.Size.Height),
                Location = new Point2D(
                    healthIcon.Location.X + healthIcon.Size.Width + GameDefines.GuiSpacing,
                    healthIcon.Location.Y)
            };
            powerText = new GuiText
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Size = new Size2D(
                    powerIcon.Size.Width * 2,
                    powerIcon.Size.Height),
                Location = new Point2D(
                    powerIcon.Location.X + healthIcon.Size.Width + GameDefines.GuiSpacing,
                    powerIcon.Location.Y)
            };
            priceText = new GuiText
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Size = new Size2D(
                    priceIcon.Size.Width * 2,
                    priceIcon.Size.Height),
                Location = new Point2D(
                    priceIcon.Location.X - priceIcon.Size.Width * 2 - GameDefines.GuiSpacing,
                    priceIcon.Location.Y)
            };
            maintenanceText = new GuiText
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Size = new Size2D(
                    maintenanceIcon.Size.Width * 2,
                    maintenanceIcon.Size.Height),
                Location = new Point2D(
                    maintenanceIcon.Location.X - maintenanceIcon.Size.Width * 2 - GameDefines.GuiSpacing,
                    maintenanceIcon.Location.Y)
            };
            troopsText = new GuiText
            {
                Size = new Size2D(unitBackground.Size.Height, 18),
                Location = new Point2D(
                    unitBackground.ClientRectangle.Left,
                    unitBackground.ClientRectangle.Bottom - 18)
            };

            previousUnitButton = new GuiButton
            {
                ContentFile = "Interface/Buttons/button-minus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    unitBackground.Location.X - GameDefines.GuiSpacing - 24,
                    unitBackground.Location.Y)
            };
            nextUnitButton = new GuiButton
            {
                ContentFile = "Interface/Buttons/button-plus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    unitBackground.Location.X + unitBackground.Size.Width + GameDefines.GuiSpacing,
                    unitBackground.Location.Y)
            };
            substractUnitButton = new GuiButton
            {
                ContentFile = "Interface/Buttons/button-minus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    unitBackground.Location.X - GameDefines.GuiSpacing - 24,
                    unitBackground.Location.Y + unitBackground.Size.Height - 24)
            };
            addUnitButton = new GuiButton
            {
                ContentFile = "Interface/Buttons/button-plus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    unitBackground.Location.X + unitBackground.Size.Width + GameDefines.GuiSpacing,
                    unitBackground.Location.Y + unitBackground.Size.Height - 24)
            };

            recruitButton = new GuiButton
            {
                ContentFile = "Interface/Buttons/green-button-large",
                ForegroundColour = Colour.White,
                Size = new Size2D(128, 26),
                Location = new Point2D(
                    (Size.Width - 128) / 2,
                    Size.Height - 42 - GameDefines.GuiSpacing)
            };

            base.LoadContent();
        }

        protected override void RegisterChildren()
        {
            base.RegisterChildren();
            
            AddChild(unitBackground);
            AddChild(unitImage);
            AddChild(paper);

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
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            addUnitButton.Clicked += OnAddUnitButtonClicked;
            nextUnitButton.Clicked += OnNextUnitButtonClicked;
            previousUnitButton.Clicked += OnPreviousUnitButtonClicked;
            recruitButton.Clicked += OnRecruitButtonClick;
            substractUnitButton.Clicked += OnSusbstractUnitButtonClicked;
        }

        protected override void UnregisterEvents()
        {
            base.UnregisterEvents();

            addUnitButton.Clicked -= OnAddUnitButtonClicked;
            nextUnitButton.Clicked -= OnNextUnitButtonClicked;
            previousUnitButton.Clicked -= OnPreviousUnitButtonClicked;
            recruitButton.Clicked -= OnRecruitButtonClick;
            substractUnitButton.Clicked -= OnSusbstractUnitButtonClicked;
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();
            
            unitText.Text = units[currentUnitIndex].Name;
            troopsText.Text = $"x{troopsAmount}";
            healthText.Text = units[currentUnitIndex].Health.ToString();
            powerText.Text = units[currentUnitIndex].Power.ToString();
            priceText.Text = units[currentUnitIndex].Price.ToString();
            maintenanceText.Text = units[currentUnitIndex].Maintenance.ToString();
            recruitButton.Text = $"Recruit ({units[currentUnitIndex].Price * troopsAmount}g)";

            unitImage.ContentFile = $"World/Assets/{game.GetWorld().Id}/units/{units[currentUnitIndex].Id}";
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
