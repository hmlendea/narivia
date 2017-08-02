using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics.Enumerations;
using Narivia.Graphics.Extensions;
using Narivia.Models;

namespace Narivia.Gui.GuiElements
{
    // TODO: Requires more refactoring and cleaning
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
        public Color TextColour { get; set; }

        IGameManager game;

        GuiImage background;

        GuiImage regionNameBackground;
        GuiImage regionNameBackgroundDecor;
        GuiText regionNameText;
        GuiFactionFlag factionImage;

        GuiImage resourceImage;
        GuiText resourceText;

        List<GuiImage> holdingImages;
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

            regionNameBackground = new GuiImage
            {
                Size = new Vector2(256, 48),
                ContentFile = "Interface/region-panel-label",
                SourceRectangle = new Rectangle(0, 0, 256, 48),
            };
            regionNameBackgroundDecor = new GuiImage
            {
                Size = regionNameBackground.Size,
                ContentFile = regionNameBackground.ContentFile,
                SourceRectangle = new Rectangle(0, 48, 256, 48)
            };
            regionNameText = new GuiText
            {
                Size = new Vector2(regionNameBackground.SourceRectangle.Width,
                                   regionNameBackground.SourceRectangle.Height),
                FontName = "SideBarFont" // TODO: Consider providing a dedicated font
            };
            factionImage = new GuiFactionFlag
            {
                Size = new Vector2(regionNameBackground.Size.Y, regionNameBackground.Size.Y)
            };

            resourceImage = new GuiImage
            {
                SourceRectangle = new Rectangle(0, 0, 64, 64)
            };
            resourceText = new GuiText
            {
                FontName = "RegionBarHoldingFont",
                TextColour = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Top
            };

            RegionId = game.GetFactionRegions(game.PlayerFactionId).First().Id;

            Children.Add(background);

            Children.Add(regionNameBackground);
            Children.Add(regionNameBackgroundDecor);
            Children.Add(regionNameText);
            Children.Add(factionImage);

            Children.Add(resourceImage);
            Children.Add(resourceText);

            base.LoadContent();
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

            holdingTexts.ForEach(w => w.Dispose());
            holdingImages.ForEach(w => w.Dispose());
            holdingTexts.Clear();
            holdingImages.Clear();

            List<Holding> holdings = game.GetRegionHoldings(RegionId).OrderBy(h => h.Name).ToList();

            holdingImages = new List<GuiImage>();

            foreach (Holding holding in holdings)
            {
                GuiImage holdingImage = new GuiImage
                {
                    ContentFile = $"World/Assets/{game.GetWorld().Id}/holdings/generic",
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
                    TextColour = Color.Black,
                    HorizontalAlignment = HorizontalAlignment.Top
                };

                holdingTexts.Add(holdingText);
                holdingText.LoadContent();
                Children.Add(holdingText);

                holdingImages.Add(holdingImage);
                holdingImage.LoadContent();
                Children.Add(holdingImage);
            }
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

        protected override void SetChildrenProperties()
        {
            background.Position = Position;
            background.Scale = Size / background.SourceRectangle.Width;

            regionNameBackground.Position = new Vector2(Position.X + (Size.X - regionNameText.ScreenArea.Width) / 2,
                                                        Position.Y - regionNameText.ScreenArea.Height + regionNameBackground.Size.Y * 0.1f);
            regionNameBackgroundDecor.Position = regionNameBackground.Position;
            regionNameText.Position = regionNameBackground.Position;
            regionNameText.TextColour = TextColour;
            
            // TODO: Something's really off (pardon the pun) with factionImage's positioning
            factionImage.Position = new Vector2(regionNameBackground.Position.X - 64, regionNameBackground.Position.Y - 40);
            resourceImage.Position = new Vector2(Position.X + HOLDING_SPACING_HORIZONTAL, Position.Y + Size.Y - 64);
            resourceText.Position = new Vector2(Position.X, Position.Y + 2);
            resourceText.Size = new Vector2(64 + HOLDING_SPACING_HORIZONTAL * 2, Size.Y - 74);

            if (string.IsNullOrWhiteSpace(RegionId))
            {
                return;
            }

            Region region = game.GetRegion(RegionId);
            Resource resource = game.GetResource(region.ResourceId);
            Faction faction = game.GetFaction(region.FactionId);
            Flag flag = game.GetFlag(faction.FlagId);

            regionNameBackground.TintColour = faction.Colour.ToXnaColor();
            regionNameText.Text = region.Name;

            factionImage.Background = flag.Background;
            factionImage.Emblem = flag.Emblem;
            factionImage.Skin = flag.Skin;
            factionImage.BackgroundPrimaryColour = flag.BackgroundPrimaryColour.ToXnaColor();
            factionImage.BackgroundSecondaryColour = flag.BackgroundSecondaryColour.ToXnaColor();
            factionImage.EmblemColour = flag.EmblemColour.ToXnaColor();

            resourceImage.ContentFile = $"World/Assets/{game.GetWorld().Id}/resources/{region.ResourceId}_big";
            resourceText.Text = resource.Name;
        }
    }
}
