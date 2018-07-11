using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Input;
using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

using Narivia.Common.Extensions;
using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Gui.Helpers;
using Narivia.Models;
using Narivia.Settings;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// World map GUI element.
    /// </summary>
    public class GuiWorldmap : GuiElement
    {
        /// <summary>
        /// Gets the selected province identifier.
        /// </summary>
        /// <value>The selected province identifier.</value>
        public string SelectedProvinceId { get; private set; }

        IGameManager game;
        Camera camera;
        World world;

        TextureSprite provinceHighlight;
        TextureSprite selectedProvinceHighlight;
        TextureSprite factionBorder;

        Dictionary<string, TextureSprite> terrainSprites;

        Point2D mouseCoords;

        public GuiWorldmap(IGameManager game)
        {
            this.game = game;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            camera = new Camera { Size = Size };
            world = game.GetWorld();

            terrainSprites = new Dictionary<string, TextureSprite>();

            foreach (Terrain terrain in game.GetTerrains())
            {
                TextureSprite terrainSprite = new TextureSprite
                {
                    ContentFile = $"World/Terrain/{terrain.Spritesheet}",
                    SourceRectangle = new Rectangle2D(
                        GameDefines.MapTileSize, GameDefines.MapTileSize * 3,
                        GameDefines.MapTileSize, GameDefines.MapTileSize)
                };

                terrainSprite.LoadContent();
                terrainSprites.AddOrUpdate(terrain.Spritesheet, terrainSprite);
            }

            provinceHighlight = new TextureSprite
            {
                ContentFile = "World/Effects/border",
                SourceRectangle = new Rectangle2D(
                    GameDefines.MapTileSize, GameDefines.MapTileSize * 3,
                    GameDefines.MapTileSize, GameDefines.MapTileSize),
                Tint = Colour.White
            };

            selectedProvinceHighlight = new TextureSprite
            {
                ContentFile = "World/Effects/border",
                SourceRectangle = new Rectangle2D(
                    0, GameDefines.MapTileSize * 3,
                    GameDefines.MapTileSize, GameDefines.MapTileSize),
                Tint = Colour.White
            };

            factionBorder = new TextureSprite
            {
                ContentFile = "World/Effects/border",
                SourceRectangle = new Rectangle2D(
                    0, GameDefines.MapTileSize,
                    GameDefines.MapTileSize, GameDefines.MapTileSize),
                Tint = Colour.Blue
            };

            camera.LoadContent();

            provinceHighlight.LoadContent();
            selectedProvinceHighlight.LoadContent();
            factionBorder.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            camera.UnloadContent();

            provinceHighlight.UnloadContent();
            selectedProvinceHighlight.UnloadContent();
            factionBorder.UnloadContent();

            terrainSprites.Clear();

            base.UnloadContent();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            camera.Size = Size;

            camera.Update(gameTime);

            provinceHighlight.Update(gameTime);
            selectedProvinceHighlight.Update(gameTime);
            factionBorder.Update(gameTime);

            base.Update(gameTime);

            Point2D mouseGameMapCoords = ScreenToMapCoordinates(mouseCoords);

            int x = mouseGameMapCoords.X;
            int y = mouseGameMapCoords.Y;

            if (x > 0 && x < game.GetWorld().Width &&
                y > 0 && y < game.GetWorld().Height)
            {
                // TODO: Handle the Id retrieval properly
                SelectedProvinceId = game.GetWorld().Tiles[x, y].ProvinceId;

                // TODO: Also handle this properly
                if (game.FactionIdAtLocation(x, y) == GameDefines.GaiaFactionIdentifier)
                {
                    SelectedProvinceId = null;
                }
            }
            else
            {
                SelectedProvinceId = null;
            }
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Point camCoordsBegin = new Point(
                camera.Location.X / GameDefines.MapTileSize,
                camera.Location.Y / GameDefines.MapTileSize);

            Point camCoordsEnd = new Point(
                camCoordsBegin.X + camera.Size.Width / GameDefines.MapTileSize + 2,
                camCoordsBegin.Y + camera.Size.Height / GameDefines.MapTileSize + 1);

            for (int y = camCoordsBegin.Y; y < camCoordsEnd.Y; y++)
            {
                for (int x = camCoordsBegin.X; x < camCoordsEnd.X; x++)
                {
                    DrawTile(spriteBatch, x, y);
                }
            }

            DrawProvinceHighlight(spriteBatch);
            DrawFactionBorders(spriteBatch);

            base.Draw(spriteBatch);
        }

        /// <summary>
        /// Centres the camera on the specified location.
        /// </summary>
        public void CentreCameraOnLocation(int x, int y)
        {
            camera.CentreOnLocation(new Point2D(x * GameDefines.MapTileSize, y * GameDefines.MapTileSize));
        }

        void DrawTile(SpriteBatch spriteBatch, int x, int y)
        {
            WorldTile tile = world.Tiles[x, y];

            Terrain terrain = game.GetTerrain(tile.TerrainId); // TODO: Optimise this. Don't call this every time
            DrawTerrainSprite(spriteBatch, x, y, terrain.Spritesheet, 1, 3);
        }

        void DrawTerrainSprite(SpriteBatch spriteBatch, int tileX, int tileY, string spritesheet, int spritesheetX, int spritesheetY)
        {
            TextureSprite terrainSprite = terrainSprites[spritesheet];

            terrainSprite.Location = new Point2D(
                tileX * GameDefines.MapTileSize - camera.Location.X,
                tileY * GameDefines.MapTileSize - camera.Location.Y);
            terrainSprite.SourceRectangle = new Rectangle2D(
                GameDefines.MapTileSize * spritesheetX, GameDefines.MapTileSize * spritesheetY,
                GameDefines.MapTileSize, GameDefines.MapTileSize);

            terrainSprite.Draw(spriteBatch);
        }

        void DrawProvinceHighlight(SpriteBatch spriteBatch)
        {
            if (string.IsNullOrEmpty(SelectedProvinceId))
            {
                return;
            }

            int cameraSizeX = camera.Size.Width / GameDefines.MapTileSize + 2;
            int cameraSizeY = camera.Size.Height / GameDefines.MapTileSize + 2;

            for (int j = 0; j < cameraSizeY; j++)
            {
                for (int i = 0; i < cameraSizeX; i++)
                {
                    Point2D screenCoords = new Point2D((i * GameDefines.MapTileSize) - camera.Location.X % GameDefines.MapTileSize,
                                                     (j * GameDefines.MapTileSize) - camera.Location.Y % GameDefines.MapTileSize);
                    Point2D gameCoords = ScreenToMapCoordinates(screenCoords);

                    int x = gameCoords.X;
                    int y = gameCoords.Y;

                    if (x < 0 || x > game.GetWorld().Width ||
                        y < 0 || y > game.GetWorld().Height)
                    {
                        continue;
                    }

                    string provinceId = game.GetWorld().Tiles[x, y].ProvinceId;
                    string factionId = game.FactionIdAtLocation(x, y);
                    Colour factionColour = game.GetFaction(factionId).Colour.ToColour();

                    if (factionId == GameDefines.GaiaFactionIdentifier)
                    {
                        continue;
                    }

                    if (SelectedProvinceId == provinceId)
                    {
                        selectedProvinceHighlight.Location = screenCoords;
                        selectedProvinceHighlight.Tint = factionColour;
                        selectedProvinceHighlight.Draw(spriteBatch);
                    }
                    else
                    {
                        provinceHighlight.Location = screenCoords;
                        provinceHighlight.Tint = factionColour;
                        provinceHighlight.Draw(spriteBatch);
                    }
                }
            }
        }

        void DrawFactionBorders(SpriteBatch spriteBatch)
        {
            int cameraSizeX = camera.Size.Width / GameDefines.MapTileSize + 2;
            int cameraSizeY = camera.Size.Height / GameDefines.MapTileSize + 2;

            for (int j = 0; j < cameraSizeY; j++)
            {
                for (int i = 0; i < cameraSizeX; i++)
                {
                    Point2D screenCoords = new Point2D(
                        (i * GameDefines.MapTileSize) - camera.Location.X % GameDefines.MapTileSize,
                        (j * GameDefines.MapTileSize) - camera.Location.Y % GameDefines.MapTileSize);
                    Point2D gameCoords = ScreenToMapCoordinates(screenCoords);

                    int x = gameCoords.X;
                    int y = gameCoords.Y;

                    if (x < 0 || x > game.GetWorld().Width ||
                        y < 0 || y > game.GetWorld().Height)
                    {
                        continue;
                    }

                    string factionId = game.FactionIdAtLocation(x, y);
                    Colour factionColour = game.GetFaction(factionId).Colour.ToColour();

                    if (factionId == GameDefines.GaiaFactionIdentifier)
                    {
                        continue;
                    }

                    string factionIdN = game.FactionIdAtLocation(x, y - 1);
                    string factionIdW = game.FactionIdAtLocation(x - 1, y);
                    string factionIdS = game.FactionIdAtLocation(x, y + 1);
                    string factionIdE = game.FactionIdAtLocation(x + 1, y);

                    factionBorder.Location = screenCoords;
                    factionBorder.Tint = factionColour;

                    // TODO: Optimise the below IFs

                    if (factionIdN != factionId &&
                        factionIdN != GameDefines.GaiaFactionIdentifier)
                    {
                        factionBorder.SourceRectangle = new Rectangle2D(
                            GameDefines.MapTileSize, 0,
                            GameDefines.MapTileSize, GameDefines.MapTileSize);
                        factionBorder.Draw(spriteBatch);
                    }
                    if (factionIdW != factionId &&
                        factionIdW != GameDefines.GaiaFactionIdentifier)
                    {
                        factionBorder.SourceRectangle = new Rectangle2D(
                            0, GameDefines.MapTileSize,
                            GameDefines.MapTileSize, GameDefines.MapTileSize);
                        factionBorder.Draw(spriteBatch);
                    }
                    if (factionIdS != factionId &&
                        factionIdS != GameDefines.GaiaFactionIdentifier)
                    {
                        factionBorder.SourceRectangle = new Rectangle2D(
                            GameDefines.MapTileSize, GameDefines.MapTileSize * 2,
                            GameDefines.MapTileSize, GameDefines.MapTileSize);
                        factionBorder.Draw(spriteBatch);
                    }
                    if (factionIdE != factionId &&
                        factionIdE != GameDefines.GaiaFactionIdentifier)
                    {
                        factionBorder.SourceRectangle = new Rectangle2D(
                            GameDefines.MapTileSize * 2, GameDefines.MapTileSize,
                            GameDefines.MapTileSize, GameDefines.MapTileSize);
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
        Point2D ScreenToMapCoordinates(Point2D screenCoords)
        {
            return new Point2D(
                (camera.Location.X + screenCoords.X) / GameDefines.MapTileSize,
                (camera.Location.Y + screenCoords.Y) / GameDefines.MapTileSize);
        }

        protected override void OnMouseMoved(object sender, MouseEventArgs e)
        {
            base.OnMouseMoved(sender, e);

            mouseCoords = e.Location;
        }
    }
}
