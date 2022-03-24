using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciExtensions;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.Controls;
using NuciXNA.Input;
using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

using Narivia.GameLogic.GameManagers;
using Narivia.Gui.SpriteEffects;
using Narivia.Models;
using Narivia.Models.Enumerations;
using Narivia.Settings;

namespace Narivia.Gui.Controls
{
    /// <summary>
    /// World map GUI element.
    /// </summary>
    public class GuiWorldmap : GuiControl, IGuiControl
    {
        /// <summary>
        /// Gets the selected province identifier.
        /// </summary>
        /// <value>The selected province identifier.</value>
        public string SelectedProvinceId { get; private set; }

        IGameManager gameManager;
        IWorldManager worldManager;

        Camera camera;
        World world;

        TextureSprite riverSprite;
        TextureSprite provinceHighlight;
        TextureSprite factionBorder;
        TextureSprite provinceBorder;

        Dictionary<string, Terrain> terrains;
        Dictionary<string, TextureSprite> terrainSprites;

        Point2D mouseCoords;

        public GuiWorldmap(
            IGameManager gameManager,
            IWorldManager worldManager)
        {
            this.gameManager = gameManager;
            this.worldManager = worldManager;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            camera = new Camera { Size = Size };
            world = gameManager.GetWorld();

            terrains = worldManager.GetTerrains().ToDictionary(x => x.Id, x => x);
            terrainSprites = new Dictionary<string, TextureSprite>();

            riverSprite = new TextureSprite
            {
                ContentFile = $"World/Terrain/{GameDefines.MapTileSize}/water_river",
                SourceRectangle = new Rectangle2D(Point2D.Empty, GameDefines.MapTileSize, GameDefines.MapTileSize),
                SpriteSheetEffect = new RiverTilingEffect(worldManager),
                IsActive = true
            };

            foreach (Terrain terrain in terrains.Values)
            {
                TextureSprite terrainSprite = new TextureSprite
                {
                    ContentFile = $"World/Terrain/{GameDefines.MapTileSize}/{terrain.Spritesheet}",
                    SourceRectangle = new Rectangle2D(
                        GameDefines.MapTileSize, GameDefines.MapTileSize * 3,
                        GameDefines.MapTileSize, GameDefines.MapTileSize),
                    SpriteSheetEffect = new TerrainSpriteSheetEffect(worldManager),
                    IsActive = true
                };

                terrainSprites.AddOrUpdate(terrain.Spritesheet, terrainSprite);
            }

            provinceHighlight = new TextureSprite
            {
                ContentFile = $"Interface/Worldmap/{GameDefines.MapTileSize}/highlight",
                SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize),
                Tint = Colour.White
            };

            provinceBorder = new TextureSprite
            {
                ContentFile = $"Interface/Worldmap/{GameDefines.MapTileSize}/province-border",
                SourceRectangle = new Rectangle2D(Point2D.Empty, GameDefines.MapTileSize, GameDefines.MapTileSize),
                Tint = Colour.Black,
                Opacity = 0.25f,
                SpriteSheetEffect = new ProvinceBorderEffect(worldManager),
                IsActive = true
            };
            factionBorder = new TextureSprite
            {
                ContentFile = $"Interface/Worldmap/{GameDefines.MapTileSize}/faction-border",
                SourceRectangle = new Rectangle2D(Point2D.Empty, GameDefines.MapTileSize, GameDefines.MapTileSize),
                SpriteSheetEffect = new FactionBorderEffect(worldManager),
                IsActive = true
            };

            camera.LoadContent();

            riverSprite.LoadContent();
            riverSprite.SpriteSheetEffect.Activate();

            foreach (TextureSprite terrainSprite in terrainSprites.Values)
            {
                terrainSprite.LoadContent();
                terrainSprite.SpriteSheetEffect.Activate();
            }

            provinceHighlight.LoadContent();

            factionBorder.LoadContent();
            factionBorder.SpriteSheetEffect.Activate();

            provinceBorder.LoadContent();
            provinceBorder.SpriteSheetEffect.Activate();

            RegisterEvents();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            camera.UnloadContent();

            riverSprite.UnloadContent();
            
            foreach (TextureSprite terrainSprite in terrainSprites.Values)
            {
                terrainSprite.UnloadContent();
            }

            provinceHighlight.UnloadContent();
            factionBorder.UnloadContent();
            provinceBorder.UnloadContent();

            terrainSprites.Clear();

            UnregisterEvents();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            camera.Size = Size;

            camera.Update(gameTime);

            provinceHighlight.Update(gameTime);
            factionBorder.Update(gameTime);
            provinceBorder.Update(gameTime);

            Point2D mouseGameMapCoords = ScreenToMapCoordinates(mouseCoords);

            int x = mouseGameMapCoords.X;
            int y = mouseGameMapCoords.Y;

            if (x > 0 && x < world.Width &&
                y > 0 && y < world.Height)
            {
                // TODO: Handle the Id retrieval properly
                SelectedProvinceId = world.Tiles[x, y].ProvinceId;

                // TODO: Also handle this properly
                if (worldManager.GetFaction(x, y).Type == FactionType.Gaia)
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
        protected override void DoDraw(SpriteBatch spriteBatch)
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
            DrawBorders(spriteBatch);
        }

        /// <summary>
        /// Centres the camera on the specified location.
        /// </summary>
        public void CentreCameraOnLocation(Point2D worldLocation)
        {
            camera.CentreOnLocation(worldLocation * GameDefines.MapTileSize);
        }

        void RegisterEvents()
        {
            MouseMoved += OnMouseMoved;
        }

        void UnregisterEvents()
        {
            MouseMoved -= OnMouseMoved;
        }

        void DrawTile(SpriteBatch spriteBatch, int x, int y)
        {
            WorldTile tile = world.Tiles[x, y];
            Point2D location = new Point2D(x, y);

            // TODO: Don't do all this, and definetely don't do it here
            foreach (string terrainId in tile.TerrainIds)
            {
                Terrain terrain = terrains[terrainId]; // TODO: Optimise this. Don't call this every time

                TerrainSpriteSheetEffect terrainTilingEffect = (TerrainSpriteSheetEffect)terrainSprites[terrain.Spritesheet].SpriteSheetEffect;
                terrainTilingEffect.TileLocation = location;
                terrainTilingEffect.TerrainId = terrain.Id;
                terrainTilingEffect.TilesWith = new List<string> { terrain.Id };
                terrainTilingEffect.Update(null);

                DrawTerrainSprite(spriteBatch, x, y, terrain.Spritesheet, 1, 3);
            }

            if (tile.HasRiver || tile.HasWater)
            {
                RiverTilingEffect riverTilingEffect = (RiverTilingEffect)riverSprite.SpriteSheetEffect;
                riverTilingEffect.TileLocation = location;
                riverTilingEffect.Update(null);

                riverSprite.Location = new Point2D(
                    x * GameDefines.MapTileSize - camera.Location.X,
                    y * GameDefines.MapTileSize - camera.Location.Y);

                riverSprite.Draw(spriteBatch);
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

                    if (x < 0 || x > world.Width ||
                        y < 0 || y > world.Height)
                    {
                        continue;
                    }

                    string provinceId = world.Tiles[x, y].ProvinceId;
                    Faction faction = worldManager.GetFaction(x, y);

                    if (faction.Type == FactionType.Gaia)
                    {
                        continue;
                    }

                    if (SelectedProvinceId == provinceId)
                    {
                        provinceHighlight.Location = screenCoords;
                        provinceHighlight.Tint = faction.Colour;
                        provinceHighlight.Draw(spriteBatch);
                    }
                }
            }
        }

        void DrawBorders(SpriteBatch spriteBatch)
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

                    if (x < 0 || x > world.Width ||
                        y < 0 || y > world.Height)
                    {
                        continue;
                    }

                    Faction faction = worldManager.GetFaction(x, y);

                    if (faction.Type == FactionType.Gaia)
                    {
                        continue;
                    }

                    Colour tintColour = faction.Colour;

                    provinceBorder.Location = screenCoords;
                    factionBorder.Location = screenCoords;
                    factionBorder.Tint = tintColour;

                    ProvinceBorderEffect provinceBorderEffect = (ProvinceBorderEffect)provinceBorder.SpriteSheetEffect;
                    provinceBorderEffect.TileLocation = gameCoords;
                    provinceBorderEffect.Update(null);

                    FactionBorderEffect factionBorderEffect = (FactionBorderEffect)factionBorder.SpriteSheetEffect;
                    factionBorderEffect.TileLocation = gameCoords;
                    factionBorderEffect.Update(null);

                    provinceBorder.Draw(spriteBatch);
                    factionBorder.Draw(spriteBatch);
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

        void OnMouseMoved(object sender, MouseEventArgs e)
        {
            mouseCoords = e.Location;
        }
    }
}
