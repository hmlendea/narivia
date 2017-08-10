using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics.Enumerations;
using Narivia.Graphics.Geometry;
using Narivia.Common.Extensions;
using Narivia.Input.Events;
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
        GuiText regionText;

        GuiImage priceIcon;
        GuiText priceText;

        GuiButton previousHoldingButton;
        GuiButton nextHoldingButton;
        GuiButton previouseRegionButton;
        GuiButton nextRegionButton;

        GuiButton buildButton;
        GuiButton cancelButton;

        List<HoldingType> holdingTypes;
        List<Region> regions;

        int currentHoldingTypeIndex;
        int currentRegionIndex;

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
                ContentFile = $"World/Assets/{game.GetWorld().Id}/holdings/generic"
            };

            holdingText = new GuiText
            {
                Size = new Size2D(holdingBackground.Size.Width, 18),
                FontName = "InfoBarFont"
            };
            regionText = new GuiText
            {
                Size = new Size2D(holdingBackground.Size.Height, 18),
                FontName = "InfoBarFont"
            };

            priceIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(16, 0, 16, 16)
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
                Size = new Size2D(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            nextHoldingButton = new GuiButton
            {
                Text = ">",
                Size = new Size2D(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            previouseRegionButton = new GuiButton
            {
                Text = "<",
                Size = new Size2D(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            nextRegionButton = new GuiButton
            {
                Text = ">",
                Size = new Size2D(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            buildButton = new GuiButton
            {
                Text = "Build",
                Size = new Size2D(GameDefines.GUI_TILE_SIZE * 4, GameDefines.GUI_TILE_SIZE)
            };
            cancelButton = new GuiButton
            {
                Text = "Cancel",
                Size = new Size2D(GameDefines.GUI_TILE_SIZE * 2, GameDefines.GUI_TILE_SIZE)
            };

            Children.Add(background);
            Children.Add(holdingBackground);
            Children.Add(holdingImage);

            Children.Add(holdingText);
            Children.Add(regionText);

            Children.Add(priceIcon);
            Children.Add(priceText);

            Children.Add(nextHoldingButton);
            Children.Add(previousHoldingButton);
            Children.Add(nextRegionButton);
            Children.Add(previouseRegionButton);
            Children.Add(buildButton);
            Children.Add(cancelButton);

            base.LoadContent();

            UpdateRegionList();
            SelectHolding(0);
            SelectRegion(0);
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            UpdateRegionList();

            base.Update(gameTime);
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

        /// <summary>
        /// Updates the region list.
        /// </summary>
        void UpdateRegionList()
        {
            regions = game.GetFactionRegions(game.PlayerFactionId).Where(r => game.RegionHasEmptyHoldingSlots(r.Id)).ToList();
            SelectRegion(currentRegionIndex);
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            buildButton.Clicked += OnBuildButtonClicked;
            cancelButton.Clicked += OnCancelButtonClicked;
            nextHoldingButton.Clicked += OnNextHoldingButtonClicked;
            nextRegionButton.Clicked += OnNextRegionButtonClicked;
            previousHoldingButton.Clicked += OnPreviousHoldingButtonClicked;
            previouseRegionButton.Clicked += OnPreviousRegionButtonClicked;
        }

        protected override void UnregisterEvents()
        {
            base.UnregisterEvents();

            buildButton.Clicked -= OnBuildButtonClicked;
            cancelButton.Clicked -= OnCancelButtonClicked;
            nextHoldingButton.Clicked -= OnNextHoldingButtonClicked;
            nextRegionButton.Clicked -= OnNextRegionButtonClicked;
            previousHoldingButton.Clicked -= OnPreviousHoldingButtonClicked;
            previouseRegionButton.Clicked -= OnPreviousRegionButtonClicked;
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            background.Location = Location;
            background.Size = Size;

            holdingBackground.Location = new Point2D(Location.X + (Size.Width - holdingBackground.Size.Width) / 2,
                                                   Location.Y + holdingText.Size.Height + GameDefines.GUI_SPACING);
            holdingBackground.TintColour = BackgroundColour;

            holdingImage.Location = new Point2D(holdingBackground.Location.X + (holdingBackground.Size.Width - holdingImage.SourceRectangle.Width) / 2,
                                              holdingBackground.Location.Y + (holdingBackground.Size.Height - holdingImage.SourceRectangle.Height) / 2);
            holdingImage.SourceRectangle = new Rectangle2D(64 * currentHoldingTypeIndex, 0, 64, 64);

            holdingText.ForegroundColour = ForegroundColour;
            holdingText.Location = holdingBackground.Location;
            holdingText.Text = holdingTypes[currentHoldingTypeIndex].GetDisplayName();

            regionText.ForegroundColour = ForegroundColour;
            regionText.Location = new Point2D(holdingBackground.ClientRectangle.Left,
                                              holdingBackground.ClientRectangle.Bottom - regionText.ClientRectangle.Height);

            priceIcon.Location = new Point2D(holdingBackground.ClientRectangle.Left,
                                             holdingBackground.ClientRectangle.Bottom + GameDefines.GUI_SPACING);

            priceText.ForegroundColour = ForegroundColour;
            priceText.Location = new Point2D(priceIcon.ClientRectangle.Right + GameDefines.GUI_SPACING, priceIcon.ClientRectangle.Top);
            priceText.Text = game.GetWorld().HoldingsPrice.ToString();

            previousHoldingButton.ForegroundColour = ForegroundColour;
            previousHoldingButton.Location = new Point2D(holdingBackground.ClientRectangle.Left - previousHoldingButton.ClientRectangle.Width - GameDefines.GUI_SPACING,
                                                         holdingBackground.ClientRectangle.Top);

            nextHoldingButton.ForegroundColour = ForegroundColour;
            nextHoldingButton.Location = new Point2D(holdingBackground.ClientRectangle.Right + GameDefines.GUI_SPACING,
                                                     holdingBackground.ClientRectangle.Top);

            previouseRegionButton.ForegroundColour = ForegroundColour;
            previouseRegionButton.Location = new Point2D(holdingBackground.ClientRectangle.Left - previouseRegionButton.ClientRectangle.Width - GameDefines.GUI_SPACING,
                                                         holdingBackground.ClientRectangle.Bottom - previouseRegionButton.ClientRectangle.Height);

            nextRegionButton.ForegroundColour = ForegroundColour;
            nextRegionButton.Location = new Point2D(holdingBackground.ClientRectangle.Right + GameDefines.GUI_SPACING,
                                                    holdingBackground.ClientRectangle.Bottom - nextRegionButton.ClientRectangle.Height);

            buildButton.ForegroundColour = ForegroundColour;
            buildButton.Location = new Point2D(Location.X + GameDefines.GUI_SPACING,
                                               Location.Y + Size.Height - buildButton.Size.Height - GameDefines.GUI_SPACING);

            cancelButton.ForegroundColour = ForegroundColour;
            cancelButton.Location = new Point2D(Location.X + Size.Width - cancelButton.Size.Width - GameDefines.GUI_SPACING,
                                                Location.Y + Size.Height - buildButton.Size.Height - GameDefines.GUI_SPACING);
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

        void SelectRegion(int index)
        {
            if (regions.Count == 0)
            {
                // TODO: Handle this properly

                return;
            }

            if (index > regions.Count - 1)
            {
                currentRegionIndex = 0;
            }
            else if (index < 0)
            {
                currentRegionIndex = regions.Count - 1;
            }
            else
            {
                currentRegionIndex = index;
            }

            regionText.Text = regions[currentRegionIndex].Name;
        }

        void OnBuildButtonClicked(object sender, MouseButtonEventArgs e)
        {
            if (regions.Count == 0)
            {
                // TODO: Handle this properly

                return;
            }

            if (game.GetFaction(game.PlayerFactionId).Wealth >= game.GetWorld().HoldingsPrice)
            {
                game.BuildHolding(regions[currentRegionIndex].Id, holdingTypes[currentHoldingTypeIndex]);
            }
        }

        void OnCancelButtonClicked(object sender, MouseButtonEventArgs e)
        {
            Hide();
            SelectHolding(0);

            currentRegionIndex = 0;
        }

        void OnPreviousHoldingButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectHolding(currentHoldingTypeIndex - 1);
        }

        void OnPreviousRegionButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectRegion(currentRegionIndex - 1);
        }

        void OnNextHoldingButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectHolding(currentHoldingTypeIndex + 1);
        }

        void OnNextRegionButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectRegion(currentRegionIndex + 1);
        }
    }
}
