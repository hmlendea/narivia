using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Audio;
using Narivia.Helpers;
using Narivia.Input;
using Narivia.Interface;
using Narivia.Screens;
using Narivia.Settings;

namespace Narivia
{
    /// <summary>
    /// This is the main type for the game.
    /// </summary>
    public class GameWindow : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont fpsFont;
        Cursor cursor;
        Vector2 fpsCounterSize;
        string fpsString;

        /// <summary>
        /// The tile dimensions.
        /// </summary>
        public const int TILE_DIMENSIONS = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.GameWindow"/> class.
        /// </summary>
        public GameWindow()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            cursor = new Cursor();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = (int)ScreenManager.Instance.Size.X;
            graphics.PreferredBackBufferHeight = (int)ScreenManager.Instance.Size.Y;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SettingsManager.Instance.LoadContent();

            ScreenManager.Instance.GraphicsDevice = GraphicsDevice;
            ScreenManager.Instance.SpriteBatch = spriteBatch;
            ScreenManager.Instance.LoadContent(Content);

            AudioManager.Instance.LoadContent(Content);

            cursor.LoadContent();

            fpsFont = Content.Load<SpriteFont>("Fonts/FrameCounterFont");
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void UnloadContent()
        {
            ScreenManager.Instance.UnloadContent();
            cursor.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            SettingsManager.Instance.Update(ref graphics);
            ScreenManager.Instance.Update(gameTime);
            InputManager.Instance.Update();

            cursor.Update(gameTime);

            fpsString = $"FPS: {Math.Round(FramerateCounter.Instance.AverageFramesPerSecond)}";
            fpsCounterSize = fpsFont.MeasureString(fpsString);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            FramerateCounter.Instance.Update(deltaTime);

            graphics.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            ScreenManager.Instance.Draw(spriteBatch);
            cursor.Draw(spriteBatch);

            if (SettingsManager.Instance.DebugMode)
            {
                spriteBatch.DrawString(fpsFont, fpsString, new Vector2(1, 1), Color.Lime);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

