using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.BusinessLogic.GameManagers;
using Narivia.Graphics;
using Narivia.Input;
using Narivia.Models;
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

        Image regionHighlight;
        Image selectedRegionHighlight;
        Image factionBorder;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            camera = new Camera { Size = Size };
            map = new Map { TileDimensions = Vector2.One * GameWindow.TILE_DIMENSIONS };

            regionHighlight = new Image
            {
                ImagePath = "World/Effects/border",
                SourceRectangle = new Rectangle(GameWindow.TILE_DIMENSIONS, GameWindow.TILE_DIMENSIONS * 3, GameWindow.TILE_DIMENSIONS, GameWindow.TILE_DIMENSIONS),
                Tint = Color.White,
                Opacity = 1.0f
            };

            selectedRegionHighlight = new Image
            {
                ImagePath = "World/Effects/border",
                SourceRectangle = new Rectangle(0, GameWindow.TILE_DIMENSIONS * 3, GameWindow.TILE_DIMENSIONS, GameWindow.TILE_DIMENSIONS),
                Tint = Color.White,
                Opacity = 1.0f
            };

            factionBorder = new Image
            {
                ImagePath = "World/Effects/border",
                SourceRectangle = new Rectangle(0, GameWindow.TILE_DIMENSIONS, GameWindow.TILE_DIMENSIONS, GameWindow.TILE_DIMENSIONS),
                Tint = Color.Blue,
                Opacity = 1.0f
            };

            camera.LoadContent();
            map.LoadContent(game.WorldId);

            regionHighlight.LoadContent();
            selectedRegionHighlight.LoadContent();
            factionBorder.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            camera.UnloadContent();
            map.UnloadContent();

            regionHighlight.UnloadContent();
            selectedRegionHighlight.UnloadContent();
            factionBorder.UnloadContent();

            base.UnloadContent();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            camera.Size = Size;

            camera.Update(gameTime);
            map.Update(gameTime);

            regionHighlight.Update(gameTime);
            selectedRegionHighlight.Update(gameTime);
            factionBorder.Update(gameTime);

            base.Update(gameTime);

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
            DrawFactionBorders(spriteBatch);

            base.Draw(spriteBatch);
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
                    string factionId = game.FactionIdAtPosition(x, y);
                    Colour factionColour = game.FactionColourAtPosition(x, y);

                    if (factionId == "gaia")
                    {
                        continue;
                    }

                    if (SelectedRegionId == regionId)
                    {
                        selectedRegionHighlight.Position = screenCoords;
                        selectedRegionHighlight.Tint = new Color(factionColour.Red, factionColour.Green, factionColour.Blue);
                        selectedRegionHighlight.Draw(spriteBatch);
                    }
                    else
                    {
                        regionHighlight.Position = screenCoords;
                        regionHighlight.Tint = new Color(factionColour.Red, factionColour.Green, factionColour.Blue);
                        regionHighlight.Draw(spriteBatch);
                    }
                }
            }
        }

        void DrawFactionBorders(SpriteBatch spriteBatch)
        {
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

                    string factionId = game.FactionIdAtPosition(x, y);
                    Colour factionColour = game.FactionColourAtPosition(x, y);

                    if (factionId == "gaia")
                    {
                        continue;
                    }

                    string factionIdN = game.FactionIdAtPosition(x, y - 1);
                    string factionIdW = game.FactionIdAtPosition(x - 1, y);
                    string factionIdS = game.FactionIdAtPosition(x, y + 1);
                    string factionIdE = game.FactionIdAtPosition(x + 1, y);

                    factionBorder.Position = screenCoords;
                    factionBorder.Tint = new Color(factionColour.Red, factionColour.Green, factionColour.Blue);

                    if (factionIdN != factionId &&
                        factionIdN != "gaia")
                    {
                        factionBorder.SourceRectangle = new Rectangle(GameWindow.TILE_DIMENSIONS, 0,
                                                                           GameWindow.TILE_DIMENSIONS, GameWindow.TILE_DIMENSIONS);
                        factionBorder.Draw(spriteBatch);
                    }
                    if (factionIdW != factionId &&
                        factionIdW != "gaia")
                    {
                        factionBorder.SourceRectangle = new Rectangle(0, GameWindow.TILE_DIMENSIONS,
                                                                           GameWindow.TILE_DIMENSIONS, GameWindow.TILE_DIMENSIONS);
                        factionBorder.Draw(spriteBatch);
                    }
                    if (factionIdS != factionId &&
                        factionIdS != "gaia")
                    {
                        factionBorder.SourceRectangle = new Rectangle(GameWindow.TILE_DIMENSIONS, GameWindow.TILE_DIMENSIONS * 2,
                                                                           GameWindow.TILE_DIMENSIONS, GameWindow.TILE_DIMENSIONS);
                        factionBorder.Draw(spriteBatch);
                    }
                    if (factionIdE != factionId &&
                        factionIdE != "gaia")
                    {
                        factionBorder.SourceRectangle = new Rectangle(GameWindow.TILE_DIMENSIONS * 2, GameWindow.TILE_DIMENSIONS,
                                                                           GameWindow.TILE_DIMENSIONS, GameWindow.TILE_DIMENSIONS);
                        factionBorder.Draw(spriteBatch);
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
