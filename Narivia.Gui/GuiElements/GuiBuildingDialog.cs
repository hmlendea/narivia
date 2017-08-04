using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics.Enumerations;
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
        /// <summary>
        /// Gets or sets the text colour.
        /// </summary>
        /// <value>The text colour.</value>
        public Color TextColour { get; set; }

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

        const int SPACING = 8;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiBuildingDialog"/> class.
        /// </summary>
        public GuiBuildingDialog()
        {
            TextColour = Color.Gold;
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
                FillMode = TextureFillMode.Tile
            };
            holdingBackground = new GuiImage
            {
                ContentFile = "ScreenManager/FillImage",
                FillMode = TextureFillMode.Tile,
                Size = new Point(100, 100),
                TintColour = Color.Black
            };
            holdingImage = new GuiImage
            {
                ContentFile = $"World/Assets/{game.GetWorld().Id}/holdings/generic"
            };

            holdingText = new GuiText
            {
                Size = new Point(holdingBackground.Size.X, 18),
                FontName = "InfoBarFont"
            };
            regionText = new GuiText
            {
                Size = new Point(holdingBackground.Size.X, 18),
                FontName = "InfoBarFont"
            };

            priceIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(16, 0, 16, 16)
            };
            priceText = new GuiText
            {
                Size = new Point(priceIcon.SourceRectangle.Width * 2, priceIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };

            previousHoldingButton = new GuiButton
            {
                Text = "<",
                TextColour = TextColour,
                Size = new Point(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            nextHoldingButton = new GuiButton
            {
                Text = ">",
                TextColour = TextColour,
                Size = new Point(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            previouseRegionButton = new GuiButton
            {
                Text = "<",
                TextColour = TextColour,
                Size = new Point(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            nextRegionButton = new GuiButton
            {
                Text = ">",
                TextColour = TextColour,
                Size = new Point(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            buildButton = new GuiButton
            {
                Text = "Build",
                TextColour = TextColour,
                Size = new Point(GameDefines.GUI_TILE_SIZE * 4, GameDefines.GUI_TILE_SIZE)
            };
            cancelButton = new GuiButton
            {
                Text = "Cancel",
                TextColour = TextColour,
                Size = new Point(GameDefines.GUI_TILE_SIZE * 2, GameDefines.GUI_TILE_SIZE)
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

            previousHoldingButton.Clicked += previousHoldingButton_OnClicked;
            nextHoldingButton.Clicked += nextHoldingButton_OnClicked;

            previouseRegionButton.Clicked += previousRegionButton_OnClicked;
            nextRegionButton.Clicked += nextRegionButton_OnClicked;

            buildButton.Clicked += buildButton_OnClicked;
            cancelButton.Clicked += cancelButton_OnClicked;
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

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            background.Position = Position;
            background.Size = Size;

            holdingBackground.Position = new Point(Position.X + (Size.X - holdingBackground.Size.X) / 2,
                                                   Position.Y + holdingText.Size.Y + SPACING);

            holdingImage.SourceRectangle = new Rectangle(64 * currentHoldingTypeIndex, 0, 64, 64);
            holdingImage.Position = new Point(holdingBackground.Position.X + (holdingBackground.Size.X - holdingImage.SourceRectangle.Width) / 2,
                                              holdingBackground.Position.Y + (holdingBackground.Size.Y - holdingImage.SourceRectangle.Height) / 2);

            holdingText.Position = holdingBackground.Position;
            holdingText.Text = holdingTypes[currentHoldingTypeIndex].GetDisplayName();
            holdingText.TextColour = TextColour;

            regionText.Position = new Point(holdingBackground.ScreenArea.Left, holdingBackground.ScreenArea.Bottom - regionText.ScreenArea.Height);
            regionText.TextColour = TextColour;

            priceIcon.Position = new Point(holdingBackground.ScreenArea.Left, holdingBackground.ScreenArea.Bottom + SPACING);

            priceText.Position = new Point(priceIcon.ScreenArea.Right + SPACING, priceIcon.ScreenArea.Top);
            priceText.Text = game.GetWorld().HoldingsPrice.ToString();
            priceText.TextColour = TextColour;

            previousHoldingButton.Position = new Point(holdingBackground.ScreenArea.Left - previousHoldingButton.ScreenArea.Width - SPACING, holdingBackground.ScreenArea.Top);
            nextHoldingButton.Position = new Point(holdingBackground.ScreenArea.Right + SPACING, holdingBackground.ScreenArea.Top);

            previouseRegionButton.Position = new Point(holdingBackground.ScreenArea.Left - previouseRegionButton.ScreenArea.Width - SPACING,
                                                       holdingBackground.ScreenArea.Bottom - previouseRegionButton.ScreenArea.Height);
            nextRegionButton.Position = new Point(holdingBackground.ScreenArea.Right + SPACING,
                                                  holdingBackground.ScreenArea.Bottom - nextRegionButton.ScreenArea.Height);

            buildButton.Position = new Point(Position.X + SPACING, Position.Y + Size.Y - buildButton.Size.Y - SPACING);
            cancelButton.Position = new Point(Position.X + Size.X - cancelButton.Size.X - SPACING, Position.Y + Size.Y - buildButton.Size.Y - SPACING);
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

        void previousHoldingButton_OnClicked(object sender, MouseButtonEventArgs e)
        {
            SelectHolding(currentHoldingTypeIndex - 1);
        }

        void nextHoldingButton_OnClicked(object sender, MouseButtonEventArgs e)
        {
            SelectHolding(currentHoldingTypeIndex + 1);
        }

        void previousRegionButton_OnClicked(object sender, MouseButtonEventArgs e)
        {
            SelectRegion(currentRegionIndex - 1);
        }

        void nextRegionButton_OnClicked(object sender, MouseButtonEventArgs e)
        {
            SelectRegion(currentRegionIndex + 1);
        }

        void buildButton_OnClicked(object sender, MouseButtonEventArgs e)
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

        void cancelButton_OnClicked(object sender, MouseButtonEventArgs e)
        {
            Hide();
            SelectHolding(0);

            currentRegionIndex = 0;
        }
    }
}
