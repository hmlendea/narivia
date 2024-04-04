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
    public class GuiBuildBuildingPanel : GuiPanel
    {
        readonly IGameManager gameManager;
        readonly IWorldManager worldManager;
        readonly IBuildingManager buildingManager;

        GuiBuildingCard buildingCard;
        GuiImage paper;

        GuiText buildingText;

        GuiImage priceIcon;
        GuiText priceText;

        GuiButton previousBuildingButton;
        GuiButton nextBuildingButton;

        GuiButton buildButton;

        IList<BuildingType> buildingTypes;

        int currentBuildingTypeIndex;

        public string HoldingId { get; set; }

        public GuiBuildBuildingPanel(
            IGameManager gameManager,
            IWorldManager worldManager,
            IBuildingManager buildingManager)
        {
            this.gameManager = gameManager;
            this.worldManager = worldManager;
            this.buildingManager = buildingManager;

            Title = "Building";
            FontName = "ButtonFont";
        }

        protected override void DoLoadContent()
        {
            base.DoLoadContent();

            buildingTypes = buildingManager.GetBuildingTypes().ToList();

            buildingText = new GuiText
            {
                Id = $"{Id}_{nameof(buildingText)}",
                ForegroundColour = Colour.Gold,
                Size = new Size2D(140, 18),
                Location = new Point2D((Size.Width - 140) / 2, 64)
            };
            previousBuildingButton = new GuiButton
            {
                Id = $"{Id}_{nameof(previousBuildingButton)}",
                ContentFile = "Interface/Buttons/button-minus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    buildingText.Location.X - GameDefines.GuiSpacing - 24,
                    buildingText.Location.Y)
            };
            nextBuildingButton = new GuiButton
            {
                Id = $"{Id}_{nameof(nextBuildingButton)}",
                ContentFile = "Interface/Buttons/button-plus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    buildingText.ClientRectangle.Right + GameDefines.GuiSpacing,
                    buildingText.Location.Y)
            };
            buildingCard = new GuiBuildingCard()
            {
                Id = $"{Id}_{nameof(buildingCard)}",
                Size = new Size2D(100, 100),
                Location = new Point2D((Size.Width - 100) / 2, buildingText.ClientRectangle.Bottom + GameDefines.GuiSpacing)
            };

            paper = new GuiImage
            {
                Id = $"{Id}_{nameof(paper)}",
                ContentFile = "Interface/ProvincePanel/paper",
                Size = new Size2D(248, 80),
                Location = new Point2D(
                    (Size.Width - 248) / 2,
                    buildingCard.ClientRectangle.Bottom + GameDefines.GuiSpacing)
            };

            priceIcon = new GuiImage
            {
                Id = $"{Id}_{nameof(priceIcon)}",
                ContentFile = "Interface/Icons/wealth",
                Size = new Size2D(GameDefines.GuiIconSize, GameDefines.GuiIconSize),
                Location = paper.Location + new Point2D(
                    (paper.Size.Width - GameDefines.GuiIconSize * 3 - GameDefines.GuiSpacing) / 2,
                    (paper.Size.Height - GameDefines.GuiIconSize) / 2)
            };
            priceText = new GuiText
            {
                Id = $"{Id}_{nameof(priceText)}",
                HorizontalAlignment = Alignment.Beginning,
                Size = new Size2D(priceIcon.Size.Width * 2, priceIcon.Size.Height),
                Location = new Point2D(
                    priceIcon.ClientRectangle.Right + GameDefines.GuiSpacing,
                    priceIcon.Location.Y)
            };

            buildButton = new GuiButton
            {
                Id = $"{Id}_{nameof(buildButton)}",
                ContentFile = "Interface/Buttons/green-button-large",
                ForegroundColour = Colour.White,
                Text = "Build",
                Size = new Size2D(128, 26),
                Location = new Point2D(
                    (Size.Width - 128) / 2,
                    Size.Height - 42 - GameDefines.GuiSpacing)
            };

            SelectBuilding(0);

            RegisterChildren(paper, buildingCard, priceIcon);
            RegisterChildren(buildingText, priceText);
            RegisterChildren(nextBuildingButton, previousBuildingButton, buildButton);

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
            buildButton.Clicked += OnBuildButtonClicked;
            nextBuildingButton.Clicked += OnNextBuildingButtonClicked;
            previousBuildingButton.Clicked += OnPreviousBuildingButtonClicked;
        }

        void UnregisterEvents()
        {
            buildButton.Clicked -= OnBuildButtonClicked;
            nextBuildingButton.Clicked -= OnNextBuildingButtonClicked;
            previousBuildingButton.Clicked -= OnPreviousBuildingButtonClicked;
        }

        void SetChildrenProperties()
        {
            BuildingType buildingType = buildingTypes.ElementAt(currentBuildingTypeIndex);

            buildingText.Text = buildingType.Name;

            buildingCard.BuildingTypeId = buildingType.Id;
            buildingCard.CultureId = worldManager.GetFaction(gameManager.PlayerFactionId).CultureId;

            priceText.Text = buildingType.Price.ToString();
        }

        void SelectBuilding(int index)
        {
            if (index > buildingTypes.Count - 1)
            {
                currentBuildingTypeIndex = 0;
            }
            else if (index < 0)
            {
                currentBuildingTypeIndex = buildingTypes.Count - 1;
            }
            else
            {
                currentBuildingTypeIndex = index;
            }
        }

        void OnBuildButtonClicked(object sender, MouseButtonEventArgs e)
        {
            BuildingType buildingType = buildingTypes[currentBuildingTypeIndex];

            if (worldManager.GetFaction(gameManager.PlayerFactionId).Wealth >= buildingType.Price)
            {
                buildingManager.BuildBuilding(HoldingId, buildingType.Id);
            }
        }

        void OnPreviousBuildingButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectBuilding(currentBuildingTypeIndex - 1);
        }

        void OnNextBuildingButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectBuilding(currentBuildingTypeIndex + 1);
        }
    }
}
