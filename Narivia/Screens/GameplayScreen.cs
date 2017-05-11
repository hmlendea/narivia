using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.BusinessLogic.GameManagers;
using Narivia.Entities;
using Narivia.Widgets;
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

            string worldName = "narivia";

            camera = new Camera();
            map = new Map()
            {
                TileDimensions = Vector2.One * 16
            };

            game = new GameDomainService();
            game.NewGame(worldName, "alpalet");

            Notification welcomeNotif = new Notification
            {
                Style = NotificationStyle.Big,
                Size = new Vector2(256, 128),
                Text = $"Welcome to the world of {worldName} " +
                        Environment.NewLine +
                        Environment.NewLine +
                        "This is still a very WIP project !!!"
            };
            Notifications.Add(welcomeNotif);

            camera.LoadContent();
            map.LoadContent("narivia");
            welcomeNotif.LoadContent();
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

