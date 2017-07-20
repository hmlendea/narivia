using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics;
using Narivia.Infrastructure.Helpers;
using Narivia.Input.Events;
using Narivia.Models;
using Narivia.Models.Enumerations;

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
        public Colour TextColour { get; set; }

        IGameManager game;

        Image background;
        Image holdingBackground;
        List<Image> holdingImages;

        GuiText holdingText;
        GuiText regionText;

        Image priceIcon;
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
            TextColour = Colour.Gold;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            holdingImages = new List<Image>();
            holdingTypes = Enum.GetValues(typeof(HoldingType)).Cast<HoldingType>().Where(x => x != HoldingType.Empty).ToList();

            background = new Image
            {
                ImagePath = "Interface/backgrounds",
                SourceRectangle = new Rectangle(0, 0, 32, 32),
                Position = Position,
                TextureFillMode = TextureFillMode.Tile,
                Scale = Size / 32
            };
            holdingBackground = new Image
            {
                ImagePath = "ScreenManager/FillImage",
                SourceRectangle = new Rectangle(0, 0, 1, 1),
                Position = Position,
                Scale = new Vector2(100, 100),
                Tint = Colour.Black
            };

            holdingText = new GuiText
            {
                Text = "Castle",
                Size = new Vector2(holdingBackground.Scale.X, 18),
                FontName = "InfoBarFont"
            };
            regionText = new GuiText
            {
                Text = "Region",
                Size = new Vector2(holdingBackground.Scale.X, 18),
                FontName = "InfoBarFont"
            };

            priceIcon = new Image
            {
                ImagePath = "Interface/game_icons",
                SourceRectangle = new Rectangle(16, 0, 16, 16)
            };
            priceText = new GuiText
            {
                Text = "0",
                Size = new Vector2(priceIcon.SourceRectangle.Width * 2, priceIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };

            previousHoldingButton = new GuiButton
            {
                Text = "<",
                TextColour = TextColour,
                Size = new Vector2(32, 32)
            };
            nextHoldingButton = new GuiButton
            {
                Text = ">",
                TextColour = TextColour,
                Size = new Vector2(32, 32)
            };
            previouseRegionButton = new GuiButton
            {
                Text = "<",
                TextColour = TextColour,
                Size = new Vector2(32, 32)
            };
            nextRegionButton = new GuiButton
            {
                Text = ">",
                TextColour = TextColour,
                Size = new Vector2(32, 32)
            };
            buildButton = new GuiButton
            {
                Text = "Build",
                TextColour = TextColour,
                Size = new Vector2(128, 32)
            };
            cancelButton = new GuiButton
            {
                Text = "Cancel",
                TextColour = TextColour,
                Size = new Vector2(64, 32)
            };

            for (int i = 0; i < holdingTypes.Count; i++)
            {
                Image holdingTypeImage = new Image
                {
                    ImagePath = $"World/Assets/{game.WorldId}/holdings/generic",
                    SourceRectangle = new Rectangle(64 * i, 0, 64, 64)
                };

                holdingImages.Add(holdingTypeImage);
                holdingTypeImage.LoadContent();
            }

            SetChildrenProperties();

            background.LoadContent();
            holdingBackground.LoadContent();

            Children.Add(holdingText);
            Children.Add(regionText);

            priceIcon.LoadContent();
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
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            background.UnloadContent();
            holdingBackground.UnloadContent();

            priceIcon.UnloadContent();

            holdingImages.ForEach(i => i.UnloadContent());

            base.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            SetChildrenProperties();
            UpdateRegionList();

            background.Update(gameTime);
            holdingBackground.Update(gameTime);

            priceIcon.Update(gameTime);

            holdingImages[currentHoldingTypeIndex].Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            holdingBackground.Draw(spriteBatch);

            priceIcon.Draw(spriteBatch);

            holdingImages[currentHoldingTypeIndex].Draw(spriteBatch);

            base.Draw(spriteBatch);
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

        void SetChildrenProperties()
        {
            background.Position = Position;
            holdingBackground.Position = new Vector2(Position.X + (Size.X - holdingBackground.Scale.X) / 2, Position.Y + holdingText.Size.Y + SPACING);

            holdingText.Position = holdingBackground.Position;
            regionText.Position = new Vector2(holdingBackground.ScreenArea.Left, holdingBackground.ScreenArea.Bottom - regionText.ScreenArea.Height);

            priceIcon.Position = new Vector2(holdingBackground.ScreenArea.Left, holdingBackground.ScreenArea.Bottom + SPACING);
            priceText.Position = new Vector2(priceIcon.ScreenArea.Right + SPACING, priceIcon.ScreenArea.Top);

            previousHoldingButton.Position = new Vector2(holdingBackground.ScreenArea.Left - previousHoldingButton.ScreenArea.Width - SPACING, holdingBackground.ScreenArea.Top);
            nextHoldingButton.Position = new Vector2(holdingBackground.ScreenArea.Right + SPACING, holdingBackground.ScreenArea.Top);

            previouseRegionButton.Position = new Vector2(holdingBackground.ScreenArea.Left - previouseRegionButton.ScreenArea.Width - SPACING,
                                                         holdingBackground.ScreenArea.Bottom - previouseRegionButton.ScreenArea.Height);
            nextRegionButton.Position = new Vector2(holdingBackground.ScreenArea.Right + SPACING,
                                                    holdingBackground.ScreenArea.Bottom - nextRegionButton.ScreenArea.Height);

            buildButton.Position = new Vector2(Position.X + SPACING, Position.Y + Size.Y - buildButton.Size.Y - SPACING);
            cancelButton.Position = new Vector2(Position.X + Size.X - cancelButton.Size.X - SPACING, Position.Y + Size.Y - buildButton.Size.Y - SPACING);

            holdingImages.ForEach(i => i.Position = new Vector2(holdingBackground.Position.X + (holdingBackground.Scale.X - i.SourceRectangle.Width) / 2,
                                                             holdingBackground.Position.Y + (holdingBackground.Scale.Y - i.SourceRectangle.Height) / 2));

            holdingText.TextColour = TextColour;
            regionText.TextColour = TextColour;
            priceText.TextColour = TextColour;
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

            holdingText.Text = holdingTypes[currentHoldingTypeIndex].ToString();
            priceText.Text = game.HoldingsPrice.ToString();
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

            if (game.GetFactionWealth(game.PlayerFactionId) >= game.HoldingsPrice)
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
