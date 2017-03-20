using System.Linq;

using Microsoft.Xna.Framework.Input;

using Narivia.Screens;

namespace Narivia.Input
{
    public class InputManager
    {
        KeyboardState currentKeyState, previousKeyState;

        static InputManager instance;

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

        public void Update()
        {
            previousKeyState = currentKeyState;

            if (!ScreenManager.Instance.Transitioning)
            {
                currentKeyState = Keyboard.GetState();
            }
        }

        public bool KeyPressed(params Keys[] keys)
        {
            return keys.Any(key => currentKeyState.IsKeyDown(key) &&
                                   previousKeyState.IsKeyUp(key));
        }

        public bool KeyReleased(params Keys[] keys)
        {
            return keys.Any(key => currentKeyState.IsKeyUp(key) &&
                                   previousKeyState.IsKeyDown(key));
        }

        public bool KeyDown(params Keys[] keys)
        {
            return keys.Any(currentKeyState.IsKeyDown);
        }
    }
}
