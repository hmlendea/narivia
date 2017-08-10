using System.Drawing;

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
        /// Gets location of the mouse.
        /// </summary>
        /// <value>The mouse location.</value>
        public Point Location { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseButtonEventArgs"/> class.
        /// </summary>
        /// <param name="button">Button.</param>
        /// <param name="buttonState">Button state.</param>
        /// <param name="location">Mouse location.</param>
        public MouseButtonEventArgs(MouseButton button, MouseButtonState buttonState, Point location)
        {
            Button = button;
            ButtonState = buttonState;
            Location = location;
        }
    }
}
