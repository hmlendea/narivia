using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.BusinessLogic.GameManagers;
using Narivia.Graphics;
using Narivia.Helpers;
using Narivia.WorldMap;
using Narivia.WorldMap.Entities;

namespace Narivia.Screens
{
    public class GameplayScreen : Screen
    {
        GameDomainService game;
        Camera camera;
        Map map;
        Image fpsCounter;

        public override void LoadContent()
        {
            base.LoadContent();

            camera = new Camera();
            map = new Map()
            {
                TileDimensions = Vector2.One * 16
            };
            fpsCounter = new Image() { Text = "FPS" };
            fpsCounter.LoadContent();

            game = new GameDomainService();
            game.NewGame("narivia", "alpalet");

            camera.LoadContent();
            map.LoadContent("narivia");
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

            var fpsString = string.Format("FPS: {0}", Math.Round(FramerateCounter.AverageFramesPerSecond));
            spriteBatch.DrawString(content.Load<SpriteFont>("Fonts/FrameCounterFont"), fpsString, new Vector2(1, 1), Color.Lime);
        }
    }
}

