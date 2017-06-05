using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;

namespace Narivia.Interface.Widgets
{
    public class InfoBar : Widget
    {
        Image background;

        Image realmSizeIcon, realmSizeText;
        Image wealthIcon, wealthText;
        Image troopsIcon, troopsText;

        [XmlIgnore]
        public int RealmSize { get; set; }

        [XmlIgnore]
        public int Wealth { get; set; }

        [XmlIgnore]
        public int Troops { get; set; }

        public Color BackgroundColour { get; set; }

        public Color TextColour { get; set; }

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

            realmSizeIcon = new Image
            {
                ImagePath = "Interface/game_icons",
                SourceRectangle = new Rectangle(0, 0, 16, 16)
            };
            wealthIcon = new Image
            {
                ImagePath = "Interface/game_icons",
                SourceRectangle = new Rectangle(16, 0, 16, 16)
            };
            troopsIcon = new Image
            {
                ImagePath = "Interface/game_icons",
                SourceRectangle = new Rectangle(32, 0, 16, 16)
            };

            realmSizeText = new Image
            {
                Text = RealmSize.ToString(),
                FontName = "InfoBarFont",
                Tint = TextColour
            };
            wealthText = new Image
            {
                Text = Wealth.ToString(),
                FontName = "InfoBarFont",
                Tint = TextColour
            };
            troopsText = new Image
            {
                Text = Troops.ToString(),
                FontName = "InfoBarFont",
                Tint = TextColour
            };

            background.LoadContent();

            realmSizeIcon.LoadContent();
            realmSizeText.LoadContent();
            wealthIcon.LoadContent();
            wealthText.LoadContent();
            troopsIcon.LoadContent();
            troopsText.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            background.UnloadContent();

            realmSizeIcon.UnloadContent();
            realmSizeText.UnloadContent();
            wealthIcon.UnloadContent();
            wealthText.UnloadContent();
            troopsIcon.UnloadContent();
            troopsText.UnloadContent();

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

            realmSizeText.Text = RealmSize.ToString();
            wealthText.Text = Wealth.ToString();
            troopsText.Text = Troops.ToString();

            realmSizeIcon.Position = new Vector2(Position.X + 2, Position.Y + 2);
            realmSizeText.Position = new Vector2(realmSizeIcon.ScreenArea.Right + 2, Position.Y + 2);
            wealthIcon.Position = new Vector2(realmSizeText.ScreenArea.Right + 2, Position.Y + 2);
            wealthText.Position = new Vector2(wealthIcon.ScreenArea.Right + 2, Position.Y + 2);
            troopsIcon.Position = new Vector2(wealthText.ScreenArea.Right + 2, Position.Y + 2);
            troopsText.Position = new Vector2(troopsIcon.ScreenArea.Right + 2, Position.Y + 2);

            background.Update(gameTime);

            realmSizeIcon.Update(gameTime);
            realmSizeText.Update(gameTime);
            wealthIcon.Update(gameTime);
            wealthText.Update(gameTime);
            troopsIcon.Update(gameTime);
            troopsText.Update(gameTime);

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

            realmSizeIcon.Draw(spriteBatch);
            realmSizeText.Draw(spriteBatch);
            wealthIcon.Draw(spriteBatch);
            wealthText.Draw(spriteBatch);
            troopsIcon.Draw(spriteBatch);
            troopsText.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
