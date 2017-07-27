using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Audio;
using Narivia.Infrastructure.Helpers;
using Narivia.Infrastructure.Logging;
using Narivia.Infrastructure.Logging.Enumerations;
using Narivia.Input;
using Narivia.Gui;
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

        readonly FpsIndicator fpsIndicator;
        readonly Cursor cursor;

        /// <summary>
        /// The tile dimensions.
        /// </summary>
        public const int TILE_DIMENSIONS = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindow"/> class.
        /// </summary>
        public GameWindow()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            fpsIndicator = new FpsIndicator();
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

            LogManager.Instance.LoadContent();
            
            LogManager.Instance.Info(LogBuilder.BuildKvpMessage(Operation.GameStart, OperationStatus.Started));

            SettingsManager.Instance.LoadContent();

            ScreenManager.Instance.GraphicsDevice = GraphicsDevice;
            ScreenManager.Instance.SpriteBatch = spriteBatch;
            ScreenManager.Instance.LoadContent(Content);

            AudioManager.Instance.LoadContent(Content);

            fpsIndicator.LoadContent(Content);
            cursor.LoadContent();

            LogManager.Instance.Info(LogBuilder.BuildKvpMessage(Operation.GameStart, OperationStatus.Success));
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void UnloadContent()
        {
            LogManager.Instance.Info(LogBuilder.BuildKvpMessage(Operation.GameStop, OperationStatus.Started));

            ScreenManager.Instance.UnloadContent();

            fpsIndicator.UnloadContent();
            cursor.UnloadContent();

            LogManager.Instance.Info(LogBuilder.BuildKvpMessage(Operation.GameStop, OperationStatus.Success));
            LogManager.Instance.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            LogManager.Instance.Update(gameTime);
            SettingsManager.Instance.Update(ref graphics);
            ScreenManager.Instance.Update(gameTime);

            if (IsActive)
            {
                InputManager.Instance.Update();
            }
            else // TODO: It shouldn't reset them every single tick when the window's not active
            {
                InputManager.Instance.ResetInputStates();
            }

            fpsIndicator.Update(gameTime);
            cursor.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            ScreenManager.Instance.Draw(spriteBatch);

            fpsIndicator.Draw(spriteBatch);
            cursor.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
