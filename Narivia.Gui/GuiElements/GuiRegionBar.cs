using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics;
using Narivia.Graphics.Enumerations;
using Narivia.Graphics.Geometry;
using Narivia.Graphics.Mapping;
using Narivia.Models;
using Narivia.Settings;

using Region = Narivia.Models.Region;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Region bar GUI element.
    /// </summary>
    public class GuiRegionBar : GuiElement
    {
        /// <summary>
        /// Gets the region identifier.
        /// </summary>
        /// <value>The region identifier.</value>
        [XmlIgnore]
        public string RegionId { get; private set; }

        IGameManager game;

        GuiImage background;

        GuiImage regionNameBackground;
        GuiImage regionNameBackgroundDecor;
        GuiText regionNameText;
        GuiFactionFlag factionFlag;

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
                ContentFile = "Interface/Backgrounds/wool",
                TextureLayout = TextureLayout.Tile
            };

            regionNameBackground = new GuiImage
            {
                Size = new Size2D(256, 48),
                ContentFile = "Interface/region-panel-label",
                SourceRectangle = new Rectangle2D(0, 0, 256, 48)
            };
            regionNameBackgroundDecor = new GuiImage
            {
                Size = regionNameBackground.Size,
                ContentFile = regionNameBackground.ContentFile,
                SourceRectangle = new Rectangle2D(0, 48, 256, 48)
            };
            regionNameText = new GuiText
            {
                FontName = "SideBarFont", // TODO: Consider providing a dedicated font
                Size = new Size2D(regionNameBackground.SourceRectangle.Width,
                                  regionNameBackground.SourceRectangle.Height),
                TextOutline = true
            };
            factionFlag = new GuiFactionFlag
            {
                Size = new Size2D(regionNameBackground.Size.Height,
                                  regionNameBackground.Size.Height)
            };

            resourceImage = new GuiImage
            {
                SourceRectangle = new Rectangle2D(0, 0, 64, 64)
            };
            resourceText = new GuiText
            {
                FontName = "RegionBarHoldingFont",
                ForegroundColour = Colour.Black,
                HorizontalAlignment = HorizontalAlignment.Top
            };

            RegionId = game.GetFactionRegions(game.PlayerFactionId).First().Id;

            Children.Add(background);

            Children.Add(regionNameBackground);
            Children.Add(regionNameBackgroundDecor);
            Children.Add(regionNameText);
            Children.Add(factionFlag);

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
                    SourceRectangle = new Rectangle2D(64 * ((int)holding.Type - 1), 0, 64, 64),
                    Location = new Point2D(Location.X + GameDefines.GUI_SPACING * (holdingImages.Count + 2) + 64 * (holdingImages.Count + 1),
                                           Location.Y + Size.Height - 64)
                };

                GuiText holdingText = new GuiText
                {
                    Location = new Point2D(holdingImage.Location.X - GameDefines.GUI_SPACING, Location.Y + 2),
                    Text = holding.Name,
                    Size = new Size2D(holdingImage.SourceRectangle.Width + GameDefines.GUI_SPACING * 2,
                                      Size.Height - holdingImage.SourceRectangle.Height + 10),
                    FontName = "RegionBarHoldingFont",
                    ForegroundColour = Colour.Black,
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
            background.Location = Location;
            background.Size = Size;

            regionNameBackground.Location = new Point2D(Location.X + (Size.Width - regionNameText.ClientRectangle.Width) / 2,
                                                        Location.Y - regionNameText.ClientRectangle.Height + (int)(regionNameBackground.Size.Height * 0.1f));
            regionNameBackgroundDecor.Location = regionNameBackground.Location;
            regionNameText.Location = regionNameBackground.Location;
            regionNameText.ForegroundColour = ForegroundColour;

            factionFlag.Location = new Point2D(regionNameBackground.Location.X - factionFlag.ClientRectangle.Width / 2,
                                               regionNameBackground.Location.Y);
            resourceImage.Location = new Point2D(Location.X + GameDefines.GUI_SPACING,
                                                 Location.Y + Size.Height - 64);
            resourceText.Location = new Point2D(Location.X, Location.Y + 2);
            resourceText.Size = new Size2D(64 + GameDefines.GUI_SPACING * 2,
                                           Size.Height - 74);

            if (string.IsNullOrWhiteSpace(RegionId))
            {
                return;
            }

            Region region = game.GetRegion(RegionId);
            Resource resource = game.GetResource(region.ResourceId);
            Faction faction = game.GetFaction(region.FactionId);
            Flag flag = game.GetFlag(faction.FlagId);

            regionNameBackground.TintColour = faction.Colour.ToColour();
            regionNameText.Text = region.Name;

            factionFlag.Layer1 = flag.Layer1;
            factionFlag.Layer2 = flag.Layer2;
            factionFlag.Emblem = flag.Emblem;
            factionFlag.Skin = flag.Skin;
            factionFlag.BackgroundColour = flag.BackgroundColour.ToColour();
            factionFlag.Layer1Colour = flag.Layer1Colour.ToColour();
            factionFlag.Layer2Colour = flag.Layer2Colour.ToColour();
            factionFlag.EmblemColour = flag.EmblemColour.ToColour();

            resourceImage.ContentFile = $"World/Assets/{game.GetWorld().Id}/resources/{region.ResourceId}_big";
            resourceText.Text = resource.Name;
        }
    }
}
