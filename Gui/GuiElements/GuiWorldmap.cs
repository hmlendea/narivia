using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciExtensions;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.Controls;
using NuciXNA.Input;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers;
using Narivia.Gui.SpriteEffects;
using Narivia.Models;
using Narivia.Models.Enumerations;
using Narivia.Settings;

namespace Narivia.Gui.Controls
{
    public class GuiWorldmap : GuiControl, IGuiControl
    {
        public string SelectedProvinceId { get; private set; }

        readonly IGameManager gameManager;
        readonly IWorldManager worldManager;

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

        protected override void DoLoadContent()
        {
            camera = new Camera { Size = Size };
            world = gameManager.GetWorld();

            terrains = worldManager.GetTerrains().ToDictionary(x => x.Id, x => x);
            terrainSprites = [];

            riverSprite = new TextureSprite
            {
                ContentFile = $"World/Terrain/{GameDefines.MapTileSize}/water_river",
                SourceRectangle = new Rectangle2D(Point2D.Empty, GameDefines.MapTileSize, GameDefines.MapTileSize),
                SpriteSheetEffect = new RiverTilingEffect(worldManager),
                IsActive = true
            };

            foreach (Terrain terrain in terrains.Values)
            {
                TextureSprite terrainSprite = new()
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

        protected override void DoDraw(SpriteBatch spriteBatch)
        {
            Point2D camCoordsBegin = camera.Location / GameDefines.MapTileSize;
            Point2D camCoordsEnd = camCoordsBegin + new Point2D(
                camera.Size.Width / GameDefines.MapTileSize + 2,
                camera.Size.Height / GameDefines.MapTileSize + 1);

            for (int y = camCoordsBegin.Y; y < camCoordsEnd.Y; y++)
            {
                for (int x = camCoordsBegin.X; x < camCoordsEnd.X; x++)
                {
                    Point2D gameCoords = new(x, y);

                    DrawTile(spriteBatch, gameCoords);
                    DrawProvinceHighlight(spriteBatch, gameCoords);
                    DrawRiver(spriteBatch, gameCoords);
                    DrawBorder(spriteBatch, gameCoords);
                }
            }
        }

        public void CentreCameraOnLocation(Point2D worldLocation)
        => camera.CentreOnLocation(worldLocation * GameDefines.MapTileSize);

        void RegisterEvents()
        {
            MouseMoved += OnMouseMoved;
        }

        void UnregisterEvents()
        {
            MouseMoved -= OnMouseMoved;
        }

        void DrawTile(SpriteBatch spriteBatch, Point2D gameCoords)
        {
            WorldTile tile = world.Tiles[gameCoords.X, gameCoords.Y];

            // TODO: Don't do all this, and definetely don't do it here
            foreach (string terrainId in tile.TerrainIds)
            {
                Terrain terrain = terrains[terrainId]; // TODO: Optimise this. Don't call this every time

                TerrainSpriteSheetEffect terrainTilingEffect = (TerrainSpriteSheetEffect)terrainSprites[terrain.Spritesheet].SpriteSheetEffect;
                terrainTilingEffect.TileLocation = gameCoords;
                terrainTilingEffect.TerrainId = terrain.Id;
                terrainTilingEffect.TilesWith = [terrain.Id];
                terrainTilingEffect.Update(null);

                DrawTerrainSprite(spriteBatch, gameCoords, terrain.Spritesheet, 1, 3);
            }
        }

        void DrawRiver(SpriteBatch spriteBatch, Point2D gameCoords)
        {
            WorldTile tile = world.Tiles[gameCoords.X, gameCoords.Y];

            if (tile.HasRiver || tile.HasWater)
            {
                RiverTilingEffect riverTilingEffect = (RiverTilingEffect)riverSprite.SpriteSheetEffect;
                riverTilingEffect.TileLocation = gameCoords;
                riverTilingEffect.Update(null);

                riverSprite.Location = MapToScreenCoordinates(gameCoords);
                riverSprite.Draw(spriteBatch);
            }
        }

        void DrawTerrainSprite(SpriteBatch spriteBatch, Point2D gameCoords, string spritesheet, int spritesheetX, int spritesheetY)
        {
            TextureSprite terrainSprite = terrainSprites[spritesheet];

            terrainSprite.Location = MapToScreenCoordinates(gameCoords);
            terrainSprite.SourceRectangle = new Rectangle2D(
                GameDefines.MapTileSize * spritesheetX, GameDefines.MapTileSize * spritesheetY,
                GameDefines.MapTileSize, GameDefines.MapTileSize);

            terrainSprite.Draw(spriteBatch);
        }

        void DrawProvinceHighlight(SpriteBatch spriteBatch, Point2D gameCoords)
        {
            if (string.IsNullOrEmpty(SelectedProvinceId))
            {
                return;
            }

            if (gameCoords.X < 0 || gameCoords.X > world.Width ||
                gameCoords.Y < 0 || gameCoords.Y > world.Height)
            {
                return;
            }

            string provinceId = world.Tiles[gameCoords.X, gameCoords.Y].ProvinceId;
            Faction faction = worldManager.GetFaction(gameCoords.X, gameCoords.Y);

            if (faction.Type == FactionType.Gaia)
            {
                return;
            }

            if (SelectedProvinceId == provinceId)
            {
                provinceHighlight.Location = MapToScreenCoordinates(gameCoords);
                provinceHighlight.Tint = faction.Colour;
                provinceHighlight.Draw(spriteBatch);
            }
        }

        void DrawBorder(SpriteBatch spriteBatch, Point2D gameCoords)
        {
            if (gameCoords.X < 0 || gameCoords.X > world.Width ||
                gameCoords.Y < 0 || gameCoords.Y > world.Height)
            {
                return;
            }

            Faction faction = worldManager.GetFaction(gameCoords.X, gameCoords.Y);

            if (faction.Type == FactionType.Gaia)
            {
                return;
            }

            Point2D screenCoords = MapToScreenCoordinates(gameCoords);

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

        Point2D ScreenToMapCoordinates(Point2D screenCoords)
        => (camera.Location + screenCoords) / GameDefines.MapTileSize;

        Point2D MapToScreenCoordinates(Point2D mapCoords)
        => mapCoords * GameDefines.MapTileSize - camera.Location;

        void OnMouseMoved(object sender, MouseEventArgs e)
        {
            mouseCoords = e.Location;
        }
    }
}
