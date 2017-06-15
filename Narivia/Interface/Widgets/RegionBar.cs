using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics;
using Narivia.Models;

namespace Narivia.Interface.Widgets
{
    /// <summary>
    /// Region bar widget.
    /// </summary>
    public class RegionBar : Widget
    {
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

        Image background;
        Image regionNameBackground;
        Image regionNameText;
        List<Image> holdingIcons;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            background = new Image
            {
                ImagePath = "Interface/backgrounds",
                SourceRectangle = new Rectangle(32, 0, 32, 32),
                Position = Position,
                TextureFillMode = TextureFillMode.Tile,
                Scale = Size / 32
            };

            regionNameBackground = new Image
            {
                Tint = Color.Black,
                ImagePath = "ScreenManager/FillImage",
                SourceRectangle = new Rectangle(0, 0, 1, 1),
                Scale = new Vector2(240, 32),
            };

            regionNameText = new Image
            {
                SpriteSize = regionNameBackground.Scale,
                Tint = TextColour,
                FontName = "SideBarFont",
                TextVerticalAlignment = VerticalAlignment.Center,
                TextHorizontalAlignment = HorizontalAlignment.Center
            };

            List<Holding> holdings = game.GetRegionHoldings(RegionId).ToList();

            holdingIcons = new List<Image>();

            foreach (Holding holding in holdings)
            {
                Image holdingIcon = new Image
                {
                    ImagePath = $"World/Assets/{game.WorldId}/holdings/generic",
                    SourceRectangle = new Rectangle(64 * ((int)holding.Type - 1), 0, 64, 64),
                    Position = new Vector2(Position.X + 64 * (holdingIcons.Count), Position.Y + (Size.Y - 64) / 2)
                };

                holdingIcons.Add(holdingIcon);
                holdingIcon.LoadContent();
            }

            background.LoadContent();
            regionNameBackground.LoadContent();
            regionNameText.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            background.UnloadContent();
            regionNameBackground.UnloadContent();
            regionNameText.UnloadContent();

            holdingIcons.ForEach(x => x.UnloadContent());
            holdingIcons.Clear();

            base.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }

            Colour factionColour = game.GetFactionColour(game.GetRegionFaction(RegionId));

            regionNameBackground.Tint = new Color(factionColour.Red, factionColour.Green, factionColour.Blue);
            regionNameBackground.Position = new Vector2(Position.X + (Size.X - regionNameBackground.ScreenArea.Width) / 2,
                                              Position.Y - regionNameBackground.ScreenArea.Height / 2);

            regionNameText.Text = game.GetRegionName(RegionId);
            regionNameText.Position = regionNameBackground.Position;

            background.Update(gameTime);
            regionNameBackground.Update(gameTime);
            regionNameText.Update(gameTime);

            holdingIcons.ForEach(x => x.Update(gameTime));

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }

            background.Draw(spriteBatch);
            regionNameBackground.Draw(spriteBatch);
            regionNameText.Draw(spriteBatch);

            holdingIcons.ForEach(x => x.Draw(spriteBatch));

            base.Draw(spriteBatch);
        }

        /// <summary>
        /// Sets the region.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        public void SetRegion(string regionId)
        {
            if (RegionId == regionId)
            {
                return;
            }

            RegionId = regionId;

            if (holdingIcons != null)
            {
                UnloadContent();
            }

            LoadContent();
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
    }
}
