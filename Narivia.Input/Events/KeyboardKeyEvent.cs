using Microsoft.Xna.Framework.Input;

using Narivia.Input.Enumerations;

namespace Narivia.Input.Events
{
    /// <summary>
    /// Keyboard key event handler.
    /// </summary>
    public delegate void KeyboardKeyEventHandler(object sender, KeyboardKeyEventArgs e);

    /// <summary>
    /// Keyboard key event arguments.
    /// </summary>
    public class KeyboardKeyEventArgs
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public Keys Key { get; private set; }

        /// <summary>
        /// Gets the state of the key.
        /// </summary>
        /// <value>The state of the key.</value>
        public KeyboardKeyState KeyState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardKeyEventArgs"/> class.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="keyState">Key state.</param>
        public KeyboardKeyEventArgs(Keys key, KeyboardKeyState keyState)
        {
            Key = key;
            KeyState = keyState;
        }
    }
}
