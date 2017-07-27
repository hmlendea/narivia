using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Graphics.CustomSpriteEffects;
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
            get { return idleSprite.Position; }
            private set
            {
                idleSprite.Position = value;
                clickSprite.Position = value;
            }
        }

        public MouseButtonState State { get; private set; }

        Sprite idleSprite;
        Sprite clickSprite;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            idleSprite = new Sprite
            {
                Effects = nameof(AnimationEffect),
                ContentFile = "Cursors/idle",
                Active = true
            };
            clickSprite = new Sprite
            {
                Effects = nameof(AnimationEffect),
                ContentFile = "Cursors/click",
                Active = true
            };

            idleSprite.AnimationEffect = new AnimationEffect
            {
                FrameAmount = new Vector2(8, 1),
                SwitchFrame = 150
            };
            clickSprite.AnimationEffect = new AnimationEffect
            {
                FrameAmount = new Vector2(8, 1),
                SwitchFrame = 150
            };

            idleSprite.LoadContent();
            clickSprite.LoadContent();

            InputManager.Instance.MouseButtonPressed += InputManager_OnMouseButtonPressed;
            InputManager.Instance.MouseButtonReleased += InputManager_OnMouseButtonReleased;
            InputManager.Instance.MouseMoved += InputManager_OnMouseMoved;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            idleSprite.UnloadContent();
            clickSprite.UnloadContent();

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
            idleSprite.Update(gameTime);
            clickSprite.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (State == MouseButtonState.Pressed)
            {
                clickSprite.Draw(spriteBatch);
            }
            else
            {
                idleSprite.Draw(spriteBatch);
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
