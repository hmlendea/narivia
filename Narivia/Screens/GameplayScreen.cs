using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.BusinessLogic.GameManagers;
using Narivia.Graphics;
using Narivia.Models;
using Narivia.WorldMap;
using Narivia.WorldMap.Entities;

namespace Narivia.Screens
{
    public class GameplayScreen : Screen
    {
        GameDomainService game;
        Camera camera;
        Map map;

        public override void LoadContent()
        {
            base.LoadContent();

            camera = new Camera();

            game = new GameDomainService();
            game.NewGame("narivia", "alpalet");

            LoadMap();
            camera.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            camera.UnloadContent();
            map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            camera.Update(gameTime);
            map.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            camera.Draw(spriteBatch);
            map.Draw(spriteBatch, camera);
        }

        void LoadMap()
        {
            int mapWidth = game.WorldTiles.GetLength(0);
            int mapHeight = game.WorldTiles.GetLength(1);

            map = new Map
            {
                TileDimensions = new Vector2(NariviaGame.TILE_DIMENSIONS, NariviaGame.TILE_DIMENSIONS),
                Layers = new List<Layer>()
            };

            Dictionary<string, Layer> biomeLayers = new Dictionary<string, Layer>();
            List<Biome> biomes = game.GetAllBiomes();

            foreach (Biome biome in biomes)
            {
                Layer layer = new Layer
                {
                    Image = new Image { ImagePath = "World/Terrain/" + biome.Id },
                    TileMap = new string[mapWidth, mapHeight]
                };

                biomeLayers.Add(biome.Id, layer);
                map.Layers.Add(layer);
            }

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    string biomeId = game.BiomeMap[x, y];

                    biomeLayers[biomeId].TileMap[x, y] = "1:3";
                }
            }

            map.LoadContent();
        }
    }
}

