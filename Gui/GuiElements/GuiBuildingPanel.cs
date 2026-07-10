using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.Controls;
using NuciXNA.Input;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers;
using Narivia.Models;
using Narivia.Models.Enumerations;
using Narivia.Settings;

namespace Narivia.Gui.Controls
{
    /// <summary>
    /// Unit recruitment panel GUI element.
    /// </summary>
    public class GuiBuildingPanel : GuiPanel
    {
        const int IconSize = 22;

        IGameManager gameManager;
        IWorldManager worldManager;
        IHoldingManager holdingManager;

        GuiHoldingCard holdingCard;
        GuiImage paper;

        GuiText holdingText;
        GuiText provinceText;

        GuiImage priceIcon;
        GuiText priceText;

        GuiButton previousHoldingButton;
        GuiButton nextHoldingButton;
        GuiButton previouseProvinceButton;
        GuiButton nextProvinceButton;

        GuiButton buildButton;

        IList<HoldingType> holdings;
        IList<Province> provinces;

        int currentHoldingIndex;
        int currentProvinceIndex;

        public GuiBuildingPanel(
            IGameManager gameManager,
            IWorldManager worldManager,
            IHoldingManager holdingManager)
        {
            this.gameManager = gameManager;
            this.worldManager = worldManager;
            this.holdingManager = holdingManager;

            Title = "Building";
            FontName = "ButtonFont";
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            base.DoLoadContent();

            holdings = HoldingType
                .GetValues()
                .Cast<HoldingType>()
                .Where(x => x != HoldingType.Empty)
                .ToList();

            holdingText = new GuiText
            {
                Id = $"{Id}_{nameof(holdingText)}",
                ForegroundColour = Colour.Gold,
                Size = new Size2D(100, 18),
                Location = new Point2D((Size.Width - 100) / 2, 64)
            };
            previousHoldingButton = new GuiButton
            {
                Id = $"{Id}_{nameof(previousHoldingButton)}",
                ContentFile = "Interface/Buttons/button-minus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    holdingText.Location.X - GameDefines.GuiSpacing - 24,
                    holdingText.Location.Y)
            };
            nextHoldingButton = new GuiButton
            {
                Id = $"{Id}_{nameof(nextHoldingButton)}",
                ContentFile = "Interface/Buttons/button-plus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    holdingText.ClientRectangle.Right + GameDefines.GuiSpacing,
                    holdingText.Location.Y)
            };
            holdingCard = new GuiHoldingCard()
            {
                Id = $"{Id}_{nameof(holdingCard)}",
                Size = new Size2D(100, 100),
                Location = new Point2D(holdingText.Location.X, holdingText.ClientRectangle.Bottom + GameDefines.GuiSpacing)
            };

            provinceText = new GuiText
            {
                Id = $"{Id}_{nameof(provinceText)}",
                ForegroundColour = Colour.Gold,
                Size = new Size2D(holdingText.Size.Width, 18),
                Location = new Point2D(
                    holdingCard.Location.X,
                    holdingCard.ClientRectangle.Bottom + GameDefines.GuiSpacing)
            };
            previouseProvinceButton = new GuiButton
            {
                Id = $"{Id}_{nameof(previouseProvinceButton)}",
                ContentFile = "Interface/Buttons/button-minus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    provinceText.Location.X - GameDefines.GuiSpacing - 24,
                    provinceText.Location.Y - (24 - provinceText.Size.Height) / 2)
            };
            nextProvinceButton = new GuiButton
            {
                Id = $"{Id}_{nameof(nextProvinceButton)}",
                ContentFile = "Interface/Buttons/button-plus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    holdingCard.ClientRectangle.Right + GameDefines.GuiSpacing,
                    previouseProvinceButton.Location.Y)
            };

            paper = new GuiImage
            {
                Id = $"{Id}_{nameof(paper)}",
                ContentFile = "Interface/ProvincePanel/paper",
                Size = new Size2D(248, 80),
                Location = new Point2D(
                    (Size.Width - 248) / 2,
                    provinceText.ClientRectangle.Bottom + GameDefines.GuiSpacing)
            };

            priceIcon = new GuiImage
            {
                Id = $"{Id}_{nameof(priceIcon)}",
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(IconSize * 3, 0, IconSize, IconSize),
                Size = new Size2D(IconSize, IconSize),
                Location = new Point2D(
                    paper.Location.X + (paper.Size.Width - IconSize * 3 - GameDefines.GuiSpacing) / 2,
                    paper.Location.Y + (paper.Size.Height - IconSize) / 2)
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

            UpdateProvinceList();
            SelectHolding(0);
            SelectProvince(0);

            RegisterChildren(paper, holdingCard, priceIcon);
            RegisterChildren(holdingText, priceText, provinceText);
            RegisterChildren(nextHoldingButton, previousHoldingButton, nextProvinceButton, previouseProvinceButton, buildButton);

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
            nextHoldingButton.Clicked += OnNextHoldingButtonClicked;
            nextProvinceButton.Clicked += OnNextProvinceButtonClicked;
            previousHoldingButton.Clicked += OnPreviousHoldingButtonClicked;
            previouseProvinceButton.Clicked += OnPreviousProvinceButtonClicked;
        }

        void UnregisterEvents()
        {
            buildButton.Clicked -= OnBuildButtonClicked;
            nextHoldingButton.Clicked -= OnNextHoldingButtonClicked;
            nextProvinceButton.Clicked -= OnNextProvinceButtonClicked;
            previousHoldingButton.Clicked -= OnPreviousHoldingButtonClicked;
            previouseProvinceButton.Clicked -= OnPreviousProvinceButtonClicked;
        }

        void UpdateProvinceList()
        {
            provinces = worldManager
                .GetFactionProvinces(gameManager.PlayerFactionId)
                .Where(r => holdingManager.DoesProvinceHaveEmptyHoldings(r.Id))
                .ToList();

            SelectProvince(currentProvinceIndex);
        }

        void SetChildrenProperties()
        {
            HoldingType holdingType = holdings.ElementAt(currentHoldingIndex);

            holdingText.Text = holdingType.Name;

            holdingCard.HoldingType = holdingType;
            holdingCard.CultureId = worldManager.GetFaction(gameManager.PlayerFactionId).CultureId;// "generic"; // TODO: Use the actual culture

            priceText.Text = gameManager.GetWorld().HoldingsPrice.ToString();
        }

        void SelectHolding(int index)
        {
            if (index > holdings.Count - 1)
            {
                currentHoldingIndex = 0;
            }
            else if (index < 0)
            {
                currentHoldingIndex = holdings.Count - 1;
            }
            else
            {
                currentHoldingIndex = index;
            }
        }

        void SelectProvince(int index)
        {
            if (provinces.Count == 0)
            {
                // TODO: Handle this properly

                return;
            }

            if (index > provinces.Count - 1)
            {
                currentProvinceIndex = 0;
            }
            else if (index < 0)
            {
                currentProvinceIndex = provinces.Count - 1;
            }
            else
            {
                currentProvinceIndex = index;
            }

            provinceText.Text = provinces[currentProvinceIndex].Name;
        }

        void OnBuildButtonClicked(object sender, MouseButtonEventArgs e)
        {
            if (provinces.Count == 0)
            {
                // TODO: Handle this properly

                return;
            }

            if (worldManager.GetFaction(gameManager.PlayerFactionId).Wealth >= gameManager.GetWorld().HoldingsPrice)
            {
                holdingManager.BuildHolding(provinces[currentProvinceIndex].Id, holdings.ElementAt(currentHoldingIndex));
            }
        }

        void OnCancelButtonClicked(object sender, MouseButtonEventArgs e)
        {
            Hide();
            SelectHolding(0);

            currentProvinceIndex = 0;
        }

        void OnPreviousHoldingButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectHolding(currentHoldingIndex - 1);
        }

        void OnPreviousProvinceButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectProvince(currentProvinceIndex - 1);
        }

        void OnNextHoldingButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectHolding(currentHoldingIndex + 1);
        }

        void OnNextProvinceButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectProvince(currentProvinceIndex + 1);
        }
    }
}
