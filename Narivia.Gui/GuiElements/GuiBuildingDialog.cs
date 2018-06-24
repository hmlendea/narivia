using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using NuciXNA.Graphics.Enumerations;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Common.Extensions;
using Narivia.Models;
using Narivia.Models.Enumerations;
using Narivia.Settings;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Unit recruitment dialog GUI element.
    /// </summary>
    public class GuiBuildingDialog : GuiElement
    {
        IGameManager game;

        GuiImage background;
        GuiImage holdingBackground;
        GuiImage holdingImage;

        GuiText holdingText;
        GuiText provinceText;

        GuiImage priceIcon;
        GuiText priceText;

        GuiButton previousHoldingButton;
        GuiButton nextHoldingButton;
        GuiButton previouseProvinceButton;
        GuiButton nextProvinceButton;

        GuiButton buildButton;
        GuiButton cancelButton;

        List<HoldingType> holdingTypes;
        List<Province> provinces;

        int currentHoldingTypeIndex;
        int currentProvinceIndex;

        public GuiBuildingDialog(IGameManager game)
        {
            this.game = game;
            ForegroundColour = Colour.Gold;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            holdingTypes = Enum.GetValues(typeof(HoldingType)).Cast<HoldingType>().Where(x => x != HoldingType.Empty).ToList();

            background = new GuiImage
            {
                ContentFile = "Interface/Backgrounds/stone-bricks",
                TextureLayout = TextureLayout.Tile
            };
            holdingBackground = new GuiImage
            {
                ContentFile = "ScreenManager/FillImage",
                TextureLayout = TextureLayout.Tile,
                Size = new Size2D(100, 100)
            };
            holdingImage = new GuiImage
            {
                ContentFile = $"World/Assets/{game.GetWorld().Id}/holdings/generic",
                Size = new Size2D(64, 64)
            };

            holdingText = new GuiText
            {
                Size = new Size2D(holdingBackground.Size.Width, 18),
                FontName = "InfoBarFont"
            };
            provinceText = new GuiText
            {
                Size = new Size2D(holdingBackground.Size.Height, 18),
                FontName = "InfoBarFont"
            };

            priceIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(66, 0, 22, 22),
                Size = new Size2D(22, 22)
            };
            priceText = new GuiText
            {
                Size = new Size2D(priceIcon.SourceRectangle.Width * 2,
                                  priceIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };

            previousHoldingButton = new GuiButton
            {
                Text = "<",
                Size = new Size2D(GameDefines.GuiTileSize, GameDefines.GuiTileSize)
            };
            nextHoldingButton = new GuiButton
            {
                Text = ">",
                Size = new Size2D(GameDefines.GuiTileSize, GameDefines.GuiTileSize)
            };
            previouseProvinceButton = new GuiButton
            {
                Text = "<",
                Size = new Size2D(GameDefines.GuiTileSize, GameDefines.GuiTileSize)
            };
            nextProvinceButton = new GuiButton
            {
                Text = ">",
                Size = new Size2D(GameDefines.GuiTileSize, GameDefines.GuiTileSize)
            };
            buildButton = new GuiButton
            {
                Text = "Build",
                Size = new Size2D(GameDefines.GuiTileSize * 4, GameDefines.GuiTileSize)
            };
            cancelButton = new GuiButton
            {
                Text = "Cancel",
                Size = new Size2D(GameDefines.GuiTileSize * 2, GameDefines.GuiTileSize)
            };

            AddChild(background);
            AddChild(holdingBackground);
            AddChild(holdingImage);

            AddChild(holdingText);
            AddChild(provinceText);

            AddChild(priceIcon);
            AddChild(priceText);

            AddChild(nextHoldingButton);
            AddChild(previousHoldingButton);
            AddChild(nextProvinceButton);
            AddChild(previouseProvinceButton);
            AddChild(buildButton);
            AddChild(cancelButton);

            base.LoadContent();

            UpdateProvinceList();
            SelectHolding(0);
            SelectProvince(0);
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            UpdateProvinceList();

            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the province list.
        /// </summary>
        void UpdateProvinceList()
        {
            provinces = game.GetFactionProvinces(game.PlayerFactionId).Where(r => game.ProvinceHasEmptyHoldingSlots(r.Id)).ToList();
            SelectProvince(currentProvinceIndex);
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            buildButton.Clicked += OnBuildButtonClicked;
            cancelButton.Clicked += OnCancelButtonClicked;
            nextHoldingButton.Clicked += OnNextHoldingButtonClicked;
            nextProvinceButton.Clicked += OnNextProvinceButtonClicked;
            previousHoldingButton.Clicked += OnPreviousHoldingButtonClicked;
            previouseProvinceButton.Clicked += OnPreviousProvinceButtonClicked;
        }

        protected override void UnregisterEvents()
        {
            base.UnregisterEvents();

            buildButton.Clicked -= OnBuildButtonClicked;
            cancelButton.Clicked -= OnCancelButtonClicked;
            nextHoldingButton.Clicked -= OnNextHoldingButtonClicked;
            nextProvinceButton.Clicked -= OnNextProvinceButtonClicked;
            previousHoldingButton.Clicked -= OnPreviousHoldingButtonClicked;
            previouseProvinceButton.Clicked -= OnPreviousProvinceButtonClicked;
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            background.Size = Size;

            holdingBackground.Location = new Point2D(
                (Size.Width - holdingBackground.Size.Width) / 2,
                holdingText.Size.Height + GameDefines.GuiSpacing);
            holdingBackground.TintColour = BackgroundColour;

            holdingImage.Location = new Point2D(
                holdingBackground.Location.X + (holdingBackground.Size.Width - holdingImage.SourceRectangle.Width) / 2,
                holdingBackground.Location.Y + (holdingBackground.Size.Height - holdingImage.SourceRectangle.Height) / 2);
            holdingImage.SourceRectangle = new Rectangle2D(64 * currentHoldingTypeIndex, 0, 64, 64);

            holdingText.Location = holdingBackground.Location;
            holdingText.Text = holdingTypes[currentHoldingTypeIndex].GetDisplayName();

            provinceText.Location = new Point2D(
                holdingBackground.ClientRectangle.Left,
                holdingBackground.ClientRectangle.Bottom - provinceText.ClientRectangle.Height);

            priceIcon.Location = new Point2D(
                holdingBackground.ClientRectangle.Left,
                holdingBackground.ClientRectangle.Bottom + GameDefines.GuiSpacing);

            priceText.Location = new Point2D(priceIcon.ClientRectangle.Right + GameDefines.GuiSpacing, priceIcon.ClientRectangle.Top);
            priceText.Text = game.GetWorld().HoldingsPrice.ToString();

            previousHoldingButton.Location = new Point2D(
                holdingBackground.ClientRectangle.Left - previousHoldingButton.ClientRectangle.Width - GameDefines.GuiSpacing,
                holdingBackground.ClientRectangle.Top);

            nextHoldingButton.Location = new Point2D(
                holdingBackground.ClientRectangle.Right + GameDefines.GuiSpacing,
                holdingBackground.ClientRectangle.Top);

            previouseProvinceButton.Location = new Point2D(
                holdingBackground.ClientRectangle.Left - previouseProvinceButton.ClientRectangle.Width - GameDefines.GuiSpacing,
                holdingBackground.ClientRectangle.Bottom - previouseProvinceButton.ClientRectangle.Height);

            nextProvinceButton.Location = new Point2D(
                holdingBackground.ClientRectangle.Right + GameDefines.GuiSpacing,
                holdingBackground.ClientRectangle.Bottom - nextProvinceButton.ClientRectangle.Height);

            buildButton.Location = new Point2D(
                GameDefines.GuiSpacing,
                Size.Height - buildButton.Size.Height - GameDefines.GuiSpacing);
            ;
            cancelButton.Location = new Point2D(
                Size.Width - cancelButton.Size.Width - GameDefines.GuiSpacing,
                Size.Height - buildButton.Size.Height - GameDefines.GuiSpacing);
        }

        void SelectHolding(int index)
        {
            if (index > holdingTypes.Count - 1)
            {
                currentHoldingTypeIndex = 0;
            }
            else if (index < 0)
            {
                currentHoldingTypeIndex = holdingTypes.Count - 1;
            }
            else
            {
                currentHoldingTypeIndex = index;
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

            if (game.GetFaction(game.PlayerFactionId).Wealth >= game.GetWorld().HoldingsPrice)
            {
                game.BuildHolding(provinces[currentProvinceIndex].Id, holdingTypes[currentHoldingTypeIndex]);
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
            SelectHolding(currentHoldingTypeIndex - 1);
        }

        void OnPreviousProvinceButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectProvince(currentProvinceIndex - 1);
        }

        void OnNextHoldingButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectHolding(currentHoldingTypeIndex + 1);
        }

        void OnNextProvinceButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectProvince(currentProvinceIndex + 1);
        }
    }
}
