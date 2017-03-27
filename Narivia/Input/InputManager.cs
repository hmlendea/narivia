using System.Linq;

using Microsoft.Xna.Framework.Input;

using Narivia.Screens;

namespace Narivia.Input
{
    public class InputManager
    {
        KeyboardState currentKeyState, previousKeyState;

        static InputManager instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }

                return instance;
            }
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        public void Update()
        {
            previousKeyState = currentKeyState;

            if (!ScreenManager.Instance.Transitioning)
            {
                currentKeyState = Keyboard.GetState();
            }
        }

        /// <summary>
        /// Checks if any of the keys is pressed.
        /// </summary>
        /// <returns><c>true</c>, if any of the keys is pressed, <c>false</c> otherwise.</returns>
        /// <param name="keys">Keys.</param>
        public bool KeyPressed(params Keys[] keys)
        {
            return keys.Any(key => currentKeyState.IsKeyDown(key) &&
                                   previousKeyState.IsKeyUp(key));
        }

        /// <summary>
        /// Checks if any of the keys is released.
        /// </summary>
        /// <returns><c>true</c>, if any of they keys is released, <c>false</c> otherwise.</returns>
        /// <param name="keys">Keys.</param>
        public bool KeyReleased(params Keys[] keys)
        {
            return keys.Any(key => currentKeyState.IsKeyUp(key) &&
                                   previousKeyState.IsKeyDown(key));
        }

        /// <summary>
        /// Checks if any of the keys is down.
        /// </summary>
        /// <returns><c>true</c>, if down was keyed, <c>false</c> otherwise.</returns>
        /// <param name="keys">Keys.</param>
        public bool KeyDown(params Keys[] keys)
        {
            return keys.Any(currentKeyState.IsKeyDown);
        }
    }
}
