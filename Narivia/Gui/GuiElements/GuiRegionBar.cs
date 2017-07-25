using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics;
using Narivia.Infrastructure.Helpers;
using Narivia.Models;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Region bar GUI element.
    /// </summary>
    public class GuiRegionBar : GuiElement
    {
        const int HOLDING_SPACING_HORIZONTAL = 5;

        /// <summary>
        /// Gets the region identifier.
        /// </summary>
        /// <value>The region identifier.</value>
        [XmlIgnore]
        public string RegionId { get; private set; }

        /// <summary>
        /// Gets or sets the text colour.
        /// </summary>
        /// <value>The text colour.</value>
        public Colour TextColour { get; set; }

        IGameManager game;

        GuiImage background;
        GuiImage resourceImage;
        GuiFactionFlag factionImage;
        List<GuiImage> holdingImages;

        GuiText regionNameText;
        GuiText resourceText;
        List<GuiText> holdingTexts;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            holdingImages = new List<GuiImage>();
            holdingTexts = new List<GuiText>();

            background = new GuiImage
            {
                ContentFile = "Interface/backgrounds",
                SourceRectangle = new Rectangle(32, 0, 32, 32),
                FillMode = TextureFillMode.Tile
            };

            regionNameText = new GuiText
            {
                Text = "Region",
                Size = new Vector2(240, 32),
                FontName = "SideBarFont" // TODO: Consider providing a dedicated font
            };
            resourceText = new GuiText
            {
                Position = new Vector2(Position.X, Position.Y + 2),
                Text = "Resource",
                Size = new Vector2(64 + HOLDING_SPACING_HORIZONTAL * 2, Size.Y - 74),
                FontName = "RegionBarHoldingFont",
                TextColour = Colour.Black,
                HorizontalAlignment = HorizontalAlignment.Top
            };

            SetChildrenProperties();

            Children.Add(background);

            Children.Add(regionNameText);
            Children.Add(resourceText);

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            SetChildrenProperties();

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        /// <summary>
        /// Sets the region.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        public void SetRegion(string regionId)
        {
            if (string.IsNullOrWhiteSpace(regionId) ||
                RegionId == regionId)
            {
                return;
            }

            RegionId = regionId;

            holdingTexts.ForEach(w => w.Destroy());
            holdingImages.ForEach(w => w.Destroy());
            holdingTexts.Clear();
            holdingImages.Clear();

            if (resourceImage != null)
            {
                resourceImage.Destroy();
            }

            List<Holding> holdings = game.GetRegionHoldings(RegionId).ToList();

            holdingImages = new List<GuiImage>();

            foreach (Holding holding in holdings)
            {
                GuiImage holdingImage = new GuiImage
                {
                    ContentFile = $"World/Assets/{game.WorldId}/holdings/generic",
                    SourceRectangle = new Rectangle(64 * ((int)holding.Type - 1), 0, 64, 64),
                    Position = new Vector2(Position.X + HOLDING_SPACING_HORIZONTAL * (holdingImages.Count + 2) + 64 * (holdingImages.Count + 1),
                                           Position.Y + Size.Y - 64)
                };

                GuiText holdingText = new GuiText
                {
                    Position = new Vector2(holdingImage.Position.X - HOLDING_SPACING_HORIZONTAL, Position.Y + 2),
                    Text = holding.Name,
                    Size = new Vector2(holdingImage.SourceRectangle.Width + HOLDING_SPACING_HORIZONTAL * 2,
                                             Size.Y - holdingImage.SourceRectangle.Height + 10),
                    FontName = "RegionBarHoldingFont",
                    TextColour = Colour.Black,
                    HorizontalAlignment = HorizontalAlignment.Top
                };

                holdingTexts.Add(holdingText);
                holdingText.LoadContent();
                Children.Add(holdingText);

                holdingImages.Add(holdingImage);
                holdingImage.LoadContent();
                Children.Add(holdingImage);
            }

            string resourceId = game.GetRegionResource(RegionId);
            string factionId = game.GetRegionFaction(RegionId);

            resourceImage = new GuiImage
            {
                ContentFile = $"World/Assets/{game.WorldId}/resources/{resourceId}_big",
                SourceRectangle = new Rectangle(0, 0, 64, 64),
                Position = new Vector2(Position.X + HOLDING_SPACING_HORIZONTAL, Position.Y + Size.Y - 64)
            };

            factionImage = new GuiFactionFlag
            {
                Size = new Vector2(regionNameText.Size.Y, regionNameText.Size.Y)
            };

            resourceImage.LoadContent();
            factionImage.LoadContent();

            Children.Add(resourceImage);
            Children.Add(factionImage);
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

        void SetChildrenProperties()
        {
            background.Position = Position;
            background.Scale = Size / background.SourceRectangle.Width;

            regionNameText.TextColour = TextColour;
            regionNameText.Position = new Vector2(Position.X + (Size.X - regionNameText.ScreenArea.Width) / 2,
                                                  Position.Y - regionNameText.ScreenArea.Height);

            if (!string.IsNullOrWhiteSpace(RegionId))
            {
                string factionId = game.GetRegionFaction(RegionId);

                Colour factionColour = game.GetFactionColour(game.GetRegionFaction(RegionId));

                regionNameText.Text = game.GetRegionName(RegionId);
                regionNameText.BackgroundColour = new Colour(factionColour.R, factionColour.G, factionColour.B);

                resourceText.Text = game.GetResourceName(game.GetRegionResource(RegionId));

                Flag factionFlag = game.GetFactionFlag(factionId);

                factionImage.Position = new Vector2(regionNameText.Position.X - 64, regionNameText.Position.Y - 48);
                factionImage.Background = factionFlag.Background;
                factionImage.Emblem = factionFlag.Emblem;
                factionImage.Skin = factionFlag.Skin;
                factionImage.BackgroundPrimaryColour = factionFlag.BackgroundPrimaryColour;
                factionImage.BackgroundSecondaryColour = factionFlag.BackgroundSecondaryColour;
                factionImage.EmblemColour = factionFlag.EmblemColour;
            }
        }
    }
}
