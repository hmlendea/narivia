using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Graphics.ImageEffects;
using Narivia.Input;
using Narivia.Input.Enumerations;
using Narivia.Input.Events;

namespace Narivia.Gui
{
    public class Cursor
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public Vector2 Position
        {
            get { return idleImage.Position; }
            private set
            {
                idleImage.Position = value;
                clickImage.Position = value;
            }
        }

        public MouseButtonState State { get; private set; }

        Image idleImage;
        Image clickImage;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            idleImage = new Image
            {
                Effects = "AnimationEffect",
                ImagePath = "Cursors/idle",
                Active = true
            };
            clickImage = new Image
            {
                Effects = "AnimationEffect",
                ImagePath = "Cursors/click",
                Active = true
            };

            idleImage.AnimationEffect = new AnimationEffect
            {
                FrameAmount = new Vector2(8, 1),
                SwitchFrame = 150
            };
            clickImage.AnimationEffect = new AnimationEffect
            {
                FrameAmount = new Vector2(8, 1),
                SwitchFrame = 150
            };

            idleImage.LoadContent();
            clickImage.LoadContent();

            InputManager.Instance.MouseButtonPressed += InputManager_OnMouseButtonPressed;
            InputManager.Instance.MouseButtonReleased += InputManager_OnMouseButtonReleased;
            InputManager.Instance.MouseMoved += InputManager_OnMouseMoved;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            idleImage.UnloadContent();
            clickImage.UnloadContent();

            InputManager.Instance.MouseButtonPressed -= InputManager_OnMouseButtonPressed;
            InputManager.Instance.MouseButtonReleased -= InputManager_OnMouseButtonReleased;
            InputManager.Instance.MouseMoved -= InputManager_OnMouseMoved;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            idleImage.Update(gameTime);
            clickImage.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (State == MouseButtonState.Pressed)
            {
                clickImage.Draw(spriteBatch);
            }
            else
            {
                idleImage.Draw(spriteBatch);
            }
        }

        void InputManager_OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.LeftButton)
            {
                State = MouseButtonState.Pressed;
            }
        }

        void InputManager_OnMouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.LeftButton)
            {
                State = MouseButtonState.Released;
            }
        }

        void InputManager_OnMouseMoved(object sender, MouseEventArgs e)
        {
            Position = e.CurrentMousePosition;
        }
    }
}
