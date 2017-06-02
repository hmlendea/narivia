using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.BusinessLogic.GameManagers;
using Narivia.Graphics;
using Narivia.Input;
using Narivia.Interface;
using Narivia.Interface.Widgets;
using Narivia.WorldMap;

namespace Narivia.Screens
{
    /// <summary>
    /// Gameplay screen.
    /// </summary>
    public class GameplayScreen : Screen
    {
        GameDomainService game;
        Camera camera;
        Map map;

        Image regionHighlightImage;

        string selectedRegionId;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            string worldName = "narivia";

            camera = new Camera();
            map = new Map()
            {
                TileDimensions = Vector2.One * 16
            };

            game = new GameDomainService();
            game.NewGame(worldName, "alpalet");

            regionHighlightImage = new Image
            {
                ImagePath = "World/Effects/border",
                SourceRectangle = new Rectangle(0, 0, (int)map.TileDimensions.X, (int)map.TileDimensions.Y),
                Tint = Color.White,
                Opacity = 0.5f
            };

            regionHighlightImage.LoadContent();

            ShowNotification("Welcome",
                             $"Welcome to the world of {worldName} " +
                             Environment.NewLine +
                             Environment.NewLine +
                             "This is still a very WIP project !!!",
                             NotificationType.Informational,
                             NotificationStyle.Big,
                             new Vector2(256, 128));

            camera.LoadContent();
            map.LoadContent("narivia");
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();

            camera.UnloadContent();
            map.UnloadContent();
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

            Vector2 gameCoordsCursor = ScreenToMapCoordinates(InputManager.Instance.MousePosition);

            int x = (int)gameCoordsCursor.X;
            int y = (int)gameCoordsCursor.Y;

            if (x > 0 && x < game.WorldWidth &&
                y > 0 && y < game.WorldHeight)
            {
                // TODO: Handle the Id retrieval properly
                selectedRegionId = game.WorldTiles[x, y];

                // TODO: Also handle this properly
                if (game.FactionIdAtPosition(x, y) == "gaia")
                {
                    selectedRegionId = null;
                }
            }
            else
            {
                selectedRegionId = null;
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

            base.Draw(spriteBatch);
        }

        void DrawRegionHighlight(SpriteBatch spriteBatch)
        {
            if (string.IsNullOrEmpty(selectedRegionId))
            {
                return;
            }

            int cameraSizeX = (int)(camera.Size.X / map.TileDimensions.X + map.TileDimensions.X);
            int cameraSizeY = (int)(camera.Size.Y / map.TileDimensions.Y + map.TileDimensions.Y);

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

                    if (selectedRegionId == regionId)
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

