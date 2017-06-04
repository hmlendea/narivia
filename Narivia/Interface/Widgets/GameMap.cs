using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.BusinessLogic.GameManagers;
using Narivia.Graphics;
using Narivia.Input;
using Narivia.WorldMap;

namespace Narivia.Interface.Widgets
{
    public class GameMap : Widget
    {
        public new Vector2 Size { get; set; }

        [XmlIgnore]
        public string SelectedRegionId { get; private set; }

        GameDomainService game;
        Camera camera;
        Map map;

        Image regionHighlightImage;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            camera = new Camera { Size = Size };
            map = new Map { TileDimensions = Vector2.One * 16 };
            regionHighlightImage = new Image
            {
                ImagePath = "World/Effects/border",
                SourceRectangle = new Rectangle(0, 0, (int)map.TileDimensions.X, (int)map.TileDimensions.Y),
                Tint = Color.White,
                Opacity = 0.5f
            };

            camera.LoadContent();
            map.LoadContent(game.WorldId);
            regionHighlightImage.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            camera.UnloadContent();
            map.UnloadContent();
            regionHighlightImage.UnloadContent();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            camera.Update(gameTime);
            map.Update(gameTime);
            regionHighlightImage.Update(gameTime);

            Vector2 gameCoordsCursor = ScreenToMapCoordinates(InputManager.Instance.MousePosition);

            int x = (int)gameCoordsCursor.X;
            int y = (int)gameCoordsCursor.Y;

            if (x > 0 && x < game.WorldWidth &&
                y > 0 && y < game.WorldHeight)
            {
                // TODO: Handle the Id retrieval properly
                SelectedRegionId = game.WorldTiles[x, y];

                // TODO: Also handle this properly
                if (game.FactionIdAtPosition(x, y) == "gaia")
                {
                    SelectedRegionId = null;
                }
            }
            else
            {
                SelectedRegionId = null;
            }
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <returns>The draw.</returns>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            camera.Draw(spriteBatch);
            map.Draw(spriteBatch, camera);

            DrawRegionHighlight(spriteBatch);
        }

        // TODO: Handle this better
        public void AssociateGameDomainService(ref GameDomainService game)
        {
            this.game = game;
        }

        void DrawRegionHighlight(SpriteBatch spriteBatch)
        {
            if (string.IsNullOrEmpty(SelectedRegionId))
            {
                return;
            }

            int cameraSizeX = (int)(camera.Size.X / map.TileDimensions.X + 2);
            int cameraSizeY = (int)(camera.Size.Y / map.TileDimensions.Y + 2);

            for (int j = 0; j < cameraSizeY; j++)
            {
                for (int i = 0; i < cameraSizeX; i++)
                {
                    Vector2 screenCoords = new Vector2((int)(i * map.TileDimensions.X) - (int)(camera.Position.X % map.TileDimensions.X),
                                                       (int)(j * map.TileDimensions.Y) - (int)(camera.Position.Y % map.TileDimensions.Y));
                    Vector2 gameCoords = ScreenToMapCoordinates(screenCoords);

                    int x = (int)gameCoords.X;
                    int y = (int)gameCoords.Y;

                    if (x < 0 || x > game.WorldWidth ||
                        y < 0 || y > game.WorldHeight)
                    {
                        continue;
                    }

                    string regionId = game.WorldTiles[x, y];

                    if (SelectedRegionId == regionId)
                    {
                        regionHighlightImage.Position = screenCoords;
                        regionHighlightImage.Draw(spriteBatch);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the map cpprdomates based on the specified screen coordinates.
        /// </summary>
        /// <returns>The map coordinates.</returns>
        /// <param name="screenCoords">Screen coordinates.</param>
        Vector2 ScreenToMapCoordinates(Vector2 screenCoords)
        {
            return new Vector2((int)((camera.Position.X + screenCoords.X) / map.TileDimensions.X),
                               (int)((camera.Position.Y + screenCoords.Y) / map.TileDimensions.Y));
        }
    }
}
