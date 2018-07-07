using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Input.Events;
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

            TileShape tileShape = GetTileShape(x, y);

            switch (tileShape)
            {
                case TileShape.InnerCornerNW:
                    DrawTerrainSprite(spriteBatch, x, y, game.GetTerrain(world.Tiles[x - 1, y - 1].TerrainId).Spritesheet, 1, 0); // Terrain NW
                    break;

                case TileShape.InnerCornerNE:
                    DrawTerrainSprite(spriteBatch, x, y, game.GetTerrain(world.Tiles[x + 1, y - 1].TerrainId).Spritesheet, 2, 0); // Terrain NE
                    break;

                case TileShape.InnerCornerSW:
                    DrawTerrainSprite(spriteBatch, x, y, game.GetTerrain(world.Tiles[x - 1, y + 1].TerrainId).Spritesheet, 1, 1); // Terrain SW
                    break;

                case TileShape.InnerCornerSE:
                    DrawTerrainSprite(spriteBatch, x, y, game.GetTerrain(world.Tiles[x + 1, y + 1].TerrainId).Spritesheet, 2, 1); // Terrain SE
                    break;

                case TileShape.OuterCornerNW:
                    DrawTerrainSprite(spriteBatch, x, y, game.GetTerrain(world.Tiles[x + 1, y + 1].TerrainId).Spritesheet, 0, 2); // Terrain SE
                    break;

                case TileShape.OuterCornerNE:
                    DrawTerrainSprite(spriteBatch, x, y, game.GetTerrain(world.Tiles[x - 1, y + 1].TerrainId).Spritesheet, 2, 2); // Terrain SW
                    break;

                case TileShape.OuterCornerSW:
                    DrawTerrainSprite(spriteBatch, x, y, game.GetTerrain(world.Tiles[x + 1, y - 1].TerrainId).Spritesheet, 0, 4); // Terrain NE
                    break;

                case TileShape.OuterCornerSE:
                    DrawTerrainSprite(spriteBatch, x, y, game.GetTerrain(world.Tiles[x - 1, y - 1].TerrainId).Spritesheet, 2, 4); // Terrain NW
                    break;

                case TileShape.EdgeN:
                    DrawTerrainSprite(spriteBatch, x, y, game.GetTerrain(world.Tiles[x, y + 1].TerrainId).Spritesheet, 1, 2); // Terrain S
                    break;

                case TileShape.EdgeW:
                    DrawTerrainSprite(spriteBatch, x, y, game.GetTerrain(world.Tiles[x + 1, y].TerrainId).Spritesheet, 0, 3); // Terrain E
                    break;

                case TileShape.EdgeS:
                    DrawTerrainSprite(spriteBatch, x, y, game.GetTerrain(world.Tiles[x, y - 1].TerrainId).Spritesheet, 1, 4); // Terrain N
                    break;

                case TileShape.EdgeE:
                    DrawTerrainSprite(spriteBatch, x, y, game.GetTerrain(world.Tiles[x - 1, y].TerrainId).Spritesheet, 2, 3); // Terrain W
                    break;
            }
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

        TileShape GetTileShape(int x, int y)
        {
            WorldTile tile = world.Tiles[x, y];
            Terrain terrain = game.GetTerrain(tile.TerrainId); // TODO: Optimise this. Don't call this every time

            WorldTile tileNW = world.Tiles[x - 1, y - 1];
            WorldTile tileNE = world.Tiles[x + 1, y - 1];
            WorldTile tileSW = world.Tiles[x - 1, y + 1];
            WorldTile tileSE = world.Tiles[x + 1, y + 1];
            WorldTile tileN = world.Tiles[x, y - 1];
            WorldTile tileW = world.Tiles[x - 1, y];
            WorldTile tileS = world.Tiles[x, y + 1];
            WorldTile tileE = world.Tiles[x + 1, y];

            if (tile.TerrainId != tileNW.TerrainId &&
                tile.TerrainId != tileN.TerrainId &&
                tile.TerrainId != tileW.TerrainId)
            {
                Terrain terrainNW = game.GetTerrain(world.Tiles[x - 1, y - 1].TerrainId);

                if (terrainNW.ZIndex > terrain.ZIndex)
                {
                    return TileShape.InnerCornerNW;
                }
            }

            if (tile.TerrainId != tileNE.TerrainId &&
                tile.TerrainId != tileN.TerrainId &&
                tile.TerrainId != tileE.TerrainId)
            {
                Terrain terrainNE = game.GetTerrain(world.Tiles[x + 1, y - 1].TerrainId);

                if (terrainNE.ZIndex > terrain.ZIndex)
                {
                    return TileShape.InnerCornerNE;
                }
            }

            if (tile.TerrainId != tileSW.TerrainId &&
                tile.TerrainId != tileS.TerrainId &&
                tile.TerrainId != tileW.TerrainId)
            {
                Terrain terrainSE = game.GetTerrain(world.Tiles[x - 1, y + 1].TerrainId);

                if (terrainSE.ZIndex > terrain.ZIndex)
                {
                    return TileShape.InnerCornerSW;
                }
            }

            if (tile.TerrainId != tileSE.TerrainId &&
                tile.TerrainId != tileS.TerrainId &&
                tile.TerrainId != tileE.TerrainId)
            {
                Terrain terrainSE = game.GetTerrain(world.Tiles[x + 1, y + 1].TerrainId);

                if (terrainSE.ZIndex > terrain.ZIndex)
                {
                    return TileShape.InnerCornerSE;
                }
            }

            if (tileSE.TerrainId != tile.TerrainId &&
                tileSE.TerrainId != tileS.TerrainId &&
                tileSE.TerrainId != tileE.TerrainId)
            {
                Terrain terrainSE = game.GetTerrain(world.Tiles[x + 1, y + 1].TerrainId);

                if (terrainSE.ZIndex > terrain.ZIndex)
                {
                    return TileShape.OuterCornerNW;
                }
            }

            if (tileSW.TerrainId != tile.TerrainId &&
                tileSW.TerrainId != tileW.TerrainId &&
                tileSW.TerrainId != tileS.TerrainId)
            {
                Terrain terrainSW = game.GetTerrain(world.Tiles[x - 1, y + 1].TerrainId);

                if (terrainSW.ZIndex > terrain.ZIndex)
                {
                    return TileShape.OuterCornerNE;
                }
            }

            if (tileNE.TerrainId != tile.TerrainId &&
                tileNE.TerrainId != tileN.TerrainId &&
                tileNE.TerrainId != tileE.TerrainId)
            {
                Terrain terrainNE = game.GetTerrain(world.Tiles[x + 1, y - 1].TerrainId);

                if (terrainNE.ZIndex > terrain.ZIndex)
                {
                    return TileShape.OuterCornerSW;
                }
            }

            if (tileNW.TerrainId != tile.TerrainId &&
                tileNW.TerrainId != tileN.TerrainId &&
                tileNW.TerrainId != tileW.TerrainId)
            {
                Terrain terrainNW = game.GetTerrain(world.Tiles[x - 1, y - 1].TerrainId);

                if (terrainNW.ZIndex > terrain.ZIndex)
                {
                    return TileShape.OuterCornerSE;
                }
            }

            if (tileN.TerrainId != tile.TerrainId)
            {
                Terrain terrainN = game.GetTerrain(world.Tiles[x, y - 1].TerrainId);

                if (terrainN.ZIndex > terrain.ZIndex)
                {
                    return TileShape.EdgeS;
                }
            }

            if (tileW.TerrainId != tile.TerrainId)
            {
                Terrain terrainW = game.GetTerrain(world.Tiles[x - 1, y].TerrainId);

                if (terrainW.ZIndex > terrain.ZIndex)
                {
                    return TileShape.EdgeE;
                }
            }

            if (tileS.TerrainId != tile.TerrainId)
            {
                Terrain terrainS = game.GetTerrain(world.Tiles[x, y + 1].TerrainId);

                if (terrainS.ZIndex > terrain.ZIndex)
                {
                    return TileShape.EdgeN;
                }
            }

            if (tileE.TerrainId != tile.TerrainId)
            {
                Terrain terrainE = game.GetTerrain(world.Tiles[x + 1, y].TerrainId);

                if (terrainE.ZIndex > terrain.ZIndex)
                {
                    return TileShape.EdgeW;
                }
            }

            return TileShape.Middle;
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

            mouseCoords = e.Location.ToPoint2D();
        }
    }
}
