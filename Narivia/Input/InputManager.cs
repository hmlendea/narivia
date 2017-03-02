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
                    instance = new InputManager();

                return instance;
            }
        }

        public void Update()
        {
            previousKeyState = currentKeyState;

            if (!ScreenManager.Instance.Transitioning)
                currentKeyState = Keyboard.GetState();
        }

        public bool KeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
                if (currentKeyState.IsKeyDown(key) && previousKeyState.IsKeyUp(key))
                    return true;

            return false;
        }

        public bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
                if (currentKeyState.IsKeyUp(key) && previousKeyState.IsKeyDown(key))
                    return true;
            return false;
        }

        public bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
                if (currentKeyState.IsKeyDown(key))
                    return true;

            return false;
        }
    }
}
