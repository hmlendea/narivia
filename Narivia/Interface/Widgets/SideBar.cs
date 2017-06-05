using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;

namespace Narivia.Interface.Widgets
{
    public class SideBar : Widget
    {
        Image background;

        Image factionSymbol;
        Image factionName;
        Image turnCounter;

        [XmlIgnore]
        public string FactionId { get; set; }

        [XmlIgnore]
        public string FactionName { get; set; }

        [XmlIgnore]
        public string WorldId { get; set; }

        [XmlIgnore]
        public int Turn { get; set; }

        public Color BackgroundColour { get; set; }

        public Color TextColour { get; set; }

        int margins = 5;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            background = new Image
            {
                ImagePath = "ScreenManager/FillImage",
                SourceRectangle = new Rectangle(0, 0, 1, 1),
                Position = Position,
                Scale = Size,
                Tint = BackgroundColour
            };

            factionSymbol = new Image
            {
                ImagePath = $"World/Assets/{WorldId}/symbols/{FactionId}",
                SourceRectangle = new Rectangle(0, 0, 128, 128)
            };

            factionName = new Image
            {
                Text = FactionName,
                FontName = "SideBarFont",
                Tint = TextColour
            };

            turnCounter = new Image
            {
                Text = $"Turn: {Turn}",
                FontName = "SideBarFont",
                Tint = TextColour
            };

            background.LoadContent();
            factionSymbol.LoadContent();
            factionName.LoadContent();
            turnCounter.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            background.UnloadContent();
            factionSymbol.UnloadContent();
            factionName.UnloadContent();
            turnCounter.UnloadContent();

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

            turnCounter.Text = $"Turn: {Turn}";

            factionName.Position = Position + new Vector2(margins, margins);
            turnCounter.Position = Position + new Vector2(Size.X - turnCounter.ScreenArea.Width - margins, margins);
            factionSymbol.Position = Position + new Vector2((Size.X - factionSymbol.ScreenArea.Width) / 2, factionName.ScreenArea.Bottom + margins);

            background.Update(gameTime);
            factionSymbol.Update(gameTime);
            factionName.Update(gameTime);
            turnCounter.Update(gameTime);

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
            factionSymbol.Draw(spriteBatch);
            factionName.Draw(spriteBatch);
            turnCounter.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
