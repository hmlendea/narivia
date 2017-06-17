using Microsoft.Xna.Framework;

using Narivia.Input.Enumerations;

namespace Narivia.Input.Events
{
    /// <summary>
    /// Mouse button event handler.
    /// </summary>
    public delegate void MouseButtonEventHandler(object sender, MouseButtonEventArgs e);

    /// <summary>
    /// Mouse button event arguments.
    /// </summary>
    public class MouseButtonEventArgs
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
        /// Initializes a new instance of the <see cref="MouseButtonEventArgs"/> class.
        /// </summary>
        /// <param name="button">Button.</param>
        /// <param name="buttonState">Button state.</param>
        /// <param name="mousePosition">Mouse position.</param>
        public MouseButtonEventArgs(MouseButton button, MouseButtonState buttonState, Vector2 mousePosition)
        {
            Button = button;
            ButtonState = buttonState;
            MousePosition = mousePosition;
        }
    }
}
