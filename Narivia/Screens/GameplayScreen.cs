using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.BusinessLogic.GameManagers;
using Narivia.Entities;
using Narivia.Graphics;
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

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            camera = new Camera();
            map = new Map()
            {
                TileDimensions = Vector2.One * 16
            };

            game = new GameDomainService();
            game.NewGame("narivia", "alpalet");

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
            
            base.Draw(spriteBatch);
        }
    }
}

