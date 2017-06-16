using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Narivia.Input.Enumerations;

namespace Narivia.Input
{
    public class InputManager
    {
        /// <summary>
        /// Gets the mouse position.
        /// </summary>
        /// <value>The mouse position.</value>
        public Vector2 MousePosition => new Vector2(currentMouseState.Position.X, currentMouseState.Position.Y);

        KeyboardState currentKeyState, previousKeyState;
        MouseState currentMouseState, previousMouseState;

        static volatile InputManager instance;
        static object syncRoot = new object();

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
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new InputManager();
                        }
                    }
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
            previousMouseState = currentMouseState;

            currentKeyState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Checks if any of the keys is pressed.
        /// </summary>
        /// <returns><c>true</c>, if any of the keys is pressed, <c>false</c> otherwise.</returns>
        /// <param name="keys">Keys.</param>
        public bool IsKeyPressed(params Keys[] keys)
        {
            return keys.Any(key => currentKeyState.IsKeyDown(key) &&
                                   previousKeyState.IsKeyUp(key));
        }

        /// <summary>
        /// Checks if any of the keys is released.
        /// </summary>
        /// <returns><c>true</c>, if any of they keys is released, <c>false</c> otherwise.</returns>
        /// <param name="keys">Keys.</param>
        public bool IsKeyReleased(params Keys[] keys)
        {
            return keys.Any(key => currentKeyState.IsKeyUp(key) &&
                                   previousKeyState.IsKeyDown(key));
        }

        /// <summary>
        /// Checks if any of the keys is down.
        /// </summary>
        /// <returns><c>true</c>, if any of the keys is down, <c>false</c> otherwise.</returns>
        /// <param name="keys">Keys.</param>
        public bool IsKeyDown(params Keys[] keys)
        {
            return keys.Any(currentKeyState.IsKeyDown);
        }

        /// <summary>
        /// Checks if any key is down.
        /// </summary>
        /// <returns><c>true</c>, if any key is down, <c>false</c> otherwise.</returns>
        public bool IsAnyKeyDown()
        {
            List<Keys> keys = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();

            return keys.Any(currentKeyState.IsKeyDown);
        }

        /// <summary>
        /// Gets the state of the specified mouse button.
        /// </summary>
        /// <param name="button">Mouse button.</param>
        /// <returns>The state of the specified mouse button.</returns>
        public MouseButtonState GetMouseButtonState(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LeftButton:
                    if (currentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        return MouseButtonState.Pressed;
                    }
                    break;

                case MouseButton.RightButton:
                    if (currentMouseState.RightButton == ButtonState.Pressed)
                    {
                        return MouseButtonState.Pressed;
                    }
                    break;

                case MouseButton.MiddleButton:
                    if (currentMouseState.MiddleButton == ButtonState.Pressed)
                    {
                        return MouseButtonState.Pressed;
                    }
                    break;
            }

            return MouseButtonState.Released;
        }

        /// <summary>
        /// Checks if any of the mouse buttons is pressed.
        /// </summary>
        /// <returns><c>true</c>, if any of the mouse buttons is pressed, <c>false</c> otherwise.</returns>
        /// <param name="buttons">Mouse buttons.</param>
        public bool IsMouseButtonPressed(params MouseButton[] buttons)
        {
            foreach (MouseButton button in buttons)
            {
                switch (button)
                {
                    case MouseButton.LeftButton:
                        return currentMouseState.LeftButton == ButtonState.Pressed &&
                               previousMouseState.LeftButton == ButtonState.Released;

                    case MouseButton.RightButton:
                        return currentMouseState.RightButton == ButtonState.Pressed &&
                               previousMouseState.RightButton == ButtonState.Released;

                    case MouseButton.MiddleButton:
                        return currentMouseState.MiddleButton == ButtonState.Pressed &&
                               previousMouseState.MiddleButton == ButtonState.Released;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if any of the mouse buttons is released.
        /// </summary>
        /// <returns><c>true</c>, if any of the mouse buttons is released, <c>false</c> otherwise.</returns>
        /// <param name="buttons">Mouse buttons.</param>
        public bool IsMouseButtonReleased(params MouseButton[] buttons)
        {
            foreach (MouseButton button in buttons)
            {
                switch (button)
                {
                    case MouseButton.LeftButton:
                        return currentMouseState.LeftButton == ButtonState.Released &&
                               previousMouseState.LeftButton == ButtonState.Pressed;

                    case MouseButton.RightButton:
                        return currentMouseState.RightButton == ButtonState.Released &&
                               previousMouseState.RightButton == ButtonState.Pressed;

                    case MouseButton.MiddleButton:
                        return currentMouseState.MiddleButton == ButtonState.Released &&
                               previousMouseState.MiddleButton == ButtonState.Pressed;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if any of the mouse buttons is down.
        /// </summary>
        /// <returns><c>true</c>, if any of the mouse buttons is down, <c>false</c> otherwise.</returns>
        /// <param name="buttons">Mouse buttons.</param>
        public bool IsMouseButtonDown(params MouseButton[] buttons)
        {
            foreach (MouseButton button in buttons)
            {
                switch (button)
                {
                    case MouseButton.LeftButton:
                        return currentMouseState.LeftButton == ButtonState.Pressed;

                    case MouseButton.RightButton:
                        return currentMouseState.RightButton == ButtonState.Pressed;

                    case MouseButton.MiddleButton:
                        return currentMouseState.MiddleButton == ButtonState.Pressed;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the cursor is entering a specified area.
        /// </summary>
        /// <returns><c>true</c>, if the cursor entering the specified area, <c>false</c> otherwise.</returns>
        /// <param name="area">Area.</param>
        public bool IsCursorEnteringArea(Rectangle area)
        {
            Vector2 currentMousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
            Vector2 previousMousePosition = new Vector2(previousMouseState.X, previousMouseState.Y);

            return area.Contains(currentMousePosition) && !area.Contains(previousMousePosition);
        }

        /// <summary>
        /// Checks if the cursor is leaving a specified area.
        /// </summary>
        /// <returns><c>true</c>, if the cursor leaving the specified area, <c>false</c> otherwise.</returns>
        /// <param name="area">Area.</param>
        public bool IsCursorLeavingArea(Rectangle area)
        {
            Vector2 currentMousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
            Vector2 previousMousePosition = new Vector2(previousMouseState.X, previousMouseState.Y);

            return !area.Contains(currentMousePosition) && area.Contains(previousMousePosition);
        }

        /// <summary>
        /// Checks if the cursor is inside a specified area.
        /// </summary>
        /// <returns><c>true</c>, if the cursor is inside the specified area, <c>false</c> otherwise.</returns>
        /// <param name="area">Area.</param>
        public bool IsCursorInArea(Rectangle area)
        {
            return area.Contains(MousePosition);
        }

        /// <summary>
        /// Resets the input states.
        /// </summary>
        public void ResetInputStates()
        {
            previousKeyState = currentKeyState;
            previousMouseState = currentMouseState;

            currentKeyState = new KeyboardState();
            currentMouseState = new MouseState();
        }
    }
}
