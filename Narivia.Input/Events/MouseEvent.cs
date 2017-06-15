using Microsoft.Xna.Framework;

using Narivia.Input.Enumerations;

namespace Narivia.Input.Events
{
    /// <summary>
    /// Mouse event handler.
    /// </summary>
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);

    /// <summary>
    /// Mouse event arguments.
    /// </summary>
    public class MouseEventArgs
    {
        /// <summary>
        /// Gets the button.
        /// </summary>
        /// <value>The button.</value>
        public MouseButton Button { get; private set; }

        /// <summary>
        /// Gets the state of the button.
        /// </summary>
        /// <value>The state of the button.</value>
        public MouseButtonState ButtonState { get; private set; }

        /// <summary>
        /// Gets position of the mouse.
        /// </summary>
        /// <value>The mouse position.</value>
        public Vector2 MousePosition { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEventArgs"/> class.
        /// </summary>
        /// <param name="button">Button.</param>
        /// <param name="buttonState">Button state.</param>
        /// <param name="mousePosition">Mouse position.</param>
        public MouseEventArgs(MouseButton button, MouseButtonState buttonState, Vector2 mousePosition)
        {
            Button = button;
            ButtonState = buttonState;
            MousePosition = mousePosition;
        }
    }
}
