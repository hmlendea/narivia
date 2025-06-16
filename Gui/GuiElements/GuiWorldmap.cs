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
        public string SelectedFactionId { get; private set; }

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
                ContentFile = $"World/Terrain/water_river",
                SourceRectangle = new Rectangle2D(Point2D.Empty, GameDefines.SourceMapTileSize, GameDefines.SourceMapTileSize),
                SpriteSheetEffect = new RiverTilingEffect(worldManager),
                IsActive = true
            };

            foreach (Terrain terrain in terrains.Values)
            {
                TextureSprite terrainSprite = new()
                {
                    ContentFile = $"World/Terrain/{terrain.Spritesheet}",
                    SourceRectangle = new Rectangle2D(
                        GameDefines.SourceMapTileSize, GameDefines.SourceMapTileSize * 3,
                        GameDefines.SourceMapTileSize, GameDefines.SourceMapTileSize),
                    SpriteSheetEffect = new TerrainSpriteSheetEffect(worldManager),
                    Scale = new Scale2D(
                        (float)GameDefines.MapTileSize / GameDefines.SourceMapTileSize,
                        (float)GameDefines.MapTileSize / GameDefines.SourceMapTileSize),
                    TextureLayout = TextureLayout.Stretch,
                    IsActive = true
                };

                terrainSprites.AddOrUpdate(terrain.Spritesheet, terrainSprite);
            }

            provinceHighlight = new TextureSprite
            {
                ContentFile = $"Interface/Worldmap/highlight",
                SourceRectangle = new Rectangle2D(0, 0, GameDefines.SourceMapTileSize, GameDefines.SourceMapTileSize),
                Scale = new Scale2D(
                    (float)GameDefines.MapTileSize / GameDefines.SourceMapTileSize,
                    (float)GameDefines.MapTileSize / GameDefines.SourceMapTileSize),
                Tint = Colour.White
            };

            provinceBorder = new TextureSprite
            {
                ContentFile = $"Interface/Worldmap/province-border",
                SourceRectangle = new Rectangle2D(Point2D.Empty, GameDefines.SourceMapTileSize, GameDefines.SourceMapTileSize),
                Scale = new Scale2D(
                    (float)GameDefines.MapTileSize / GameDefines.SourceMapTileSize,
                    (float)GameDefines.MapTileSize / GameDefines.SourceMapTileSize),
                Tint = Colour.Black,
                Opacity = 0.25f,
                SpriteSheetEffect = new ProvinceBorderEffect(worldManager),
                IsActive = true
            };
            factionBorder = new TextureSprite
            {
                ContentFile = $"Interface/Worldmap/faction-border",
                SourceRectangle = new Rectangle2D(Point2D.Empty, GameDefines.SourceMapTileSize, GameDefines.SourceMapTileSize),
                Scale = new Scale2D(
                    (float)GameDefines.MapTileSize / GameDefines.SourceMapTileSize,
                    (float)GameDefines.MapTileSize / GameDefines.SourceMapTileSize),
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
                Faction faction = worldManager.GetFaction(x, y);
                SelectedProvinceId = world.Tiles[x, y].ProvinceId;
                SelectedFactionId = faction.Id;

                // TODO: Also handle this properly
                if (faction.Type == FactionType.Gaia)
                {
                    SelectedProvinceId = null;
                    SelectedFactionId = null;
                }
            }
            else
            {
                SelectedProvinceId = null;
                SelectedFactionId = null;
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

                    DrawTerrain(spriteBatch, gameCoords);
                    DrawProvinceHighlight(spriteBatch, gameCoords);
                    DrawBorder(spriteBatch, gameCoords);
                    DrawWater(spriteBatch, gameCoords);
                    DrawRiver(spriteBatch, gameCoords);
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

        void DrawTerrain(SpriteBatch spriteBatch, Point2D gameCoords)
        {
            WorldTile tile = world.Tiles[gameCoords.X, gameCoords.Y];

            foreach (string terrainId in tile.TerrainIds)
            {
                if (!terrainId.Equals("water"))
                {
                    DrawTile(spriteBatch, terrainId, gameCoords);
                }
            }
        }

        void DrawWater(SpriteBatch spriteBatch, Point2D gameCoords)
        {
            WorldTile tile = world.Tiles[gameCoords.X, gameCoords.Y];

            if (tile.TerrainIds.Contains("water"))
            {
                DrawTile(spriteBatch, "water", gameCoords);
            }
        }

        void DrawTile(SpriteBatch spriteBatch, string terrainId, Point2D gameCoords)
        {
            Terrain terrain = terrains[terrainId]; // TODO: Optimise this. Don't call this every time

            TerrainSpriteSheetEffect terrainTilingEffect = (TerrainSpriteSheetEffect)terrainSprites[terrain.Spritesheet].SpriteSheetEffect;
            terrainTilingEffect.TileLocation = gameCoords;
            terrainTilingEffect.TerrainId = terrain.Id;
            terrainTilingEffect.TilesWith = [terrain.Id];
            terrainTilingEffect.Update(null);

            DrawTerrainSprite(spriteBatch, gameCoords, terrain.Spritesheet, 1, 3);
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
                GameDefines.SourceMapTileSize * spritesheetX, GameDefines.SourceMapTileSize * spritesheetY,
                GameDefines.SourceMapTileSize, GameDefines.SourceMapTileSize);

            terrainSprite.Draw(spriteBatch);
        }

        void DrawProvinceHighlight(SpriteBatch spriteBatch, Point2D gameCoords)
        {
            if (string.IsNullOrEmpty(SelectedProvinceId))
            {
                return;
            }

            if (AreCoordinatesOutsideWorld(gameCoords))
            {
                return;
            }

            Province province = worldManager.GetProvince(gameCoords.X, gameCoords.Y);
            Faction faction = worldManager.GetFaction(province.FactionId);

            if (!province.Id.Equals(SelectedProvinceId))
            {
                if (faction.Type.Equals(FactionType.Gaia))
                {
                    if(!DoesTileNeighbourProvince(gameCoords, SelectedProvinceId))
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            Faction selectedFaction = worldManager.GetFaction(SelectedFactionId);

            provinceHighlight.Location = MapToScreenCoordinates(gameCoords);
            provinceHighlight.Tint = selectedFaction.Colour;
            provinceHighlight.Draw(spriteBatch);
        }

        void DrawBorder(SpriteBatch spriteBatch, Point2D gameCoords)
        {
            if (AreCoordinatesOutsideWorld(gameCoords))
            {
                return;
            }

            Faction faction = worldManager.GetFaction(gameCoords.X, gameCoords.Y);

            if (faction.Type.Equals(FactionType.Gaia))
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

        bool DoesTileNeighbourProvince(Point2D gameCoords, string provinceId)
        {
            for (int relX = -1; relX <= 1; relX++)
            {
                for (int relY = -1; relY <= 1; relY++)
                {
                    Point2D relGameCoords = new(gameCoords.X + relX, gameCoords.Y + relY);

                    if (AreCoordinatesOutsideWorld(relGameCoords))
                    {
                        continue;
                    }

                    Province relProvince = worldManager.GetProvince(relGameCoords.X, relGameCoords.Y);

                    if (relProvince.Id.Equals(provinceId))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        bool AreCoordinatesOutsideWorld(Point2D gameCoords)
        {
            if (gameCoords.X < 0 || gameCoords.X > world.Width ||
                gameCoords.Y < 0 || gameCoords.Y > world.Height)
            {
                return true;
            }

            return false;
        }

        void OnMouseMoved(object sender, MouseEventArgs e)
        {
            mouseCoords = e.Location;
        }
    }
}
