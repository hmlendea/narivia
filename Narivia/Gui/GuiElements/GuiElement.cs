using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Input;
using Narivia.Input.Enumerations;
using Narivia.Input.Events;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// GUI Element.
    /// </summary>
    public class GuiElement
    {
        /// <summary>
        /// Gets the position of this GUI element.
        /// </summary>
        /// <value>The position.</value>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets the size of this GUI element.
        /// </summary>
        /// <value>The size.</value>
        public virtual Vector2 Size { get; set; }

        /// <summary>
        /// Gets the screen area covered by this GUI element.
        /// </summary>
        /// <value>The screen area.</value>
        public Rectangle ScreenArea => new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        public float Opacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Narivia.GUI elements.GUI element"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Narivia.GUI elements.GUI element"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the children GUI elements.
        /// </summary>
        /// <value>The children.</value>
        public List<GuiElement> Children { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Narivia.GUI elements.GUI element"/> is destroyed.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool Destroyed { get; private set; }

        /// <summary>
        /// Occurs when clicked.
        /// </summary>
        public event MouseButtonEventHandler Clicked;

        /// <summary>
        /// Occurs when the mouse moved.
        /// </summary>
        public event MouseEventHandler MouseMoved;

        /// <summary>
        /// Occurs when the mouse entered this GUI element.
        /// </summary>
        public event MouseEventHandler MouseEntered;

        /// <summary>
        /// Occurs when the mouse left this GUI element.
        /// </summary>
        public event MouseEventHandler MouseLeft;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiElement"/> class.
        /// </summary>
        public GuiElement()
        {
            Enabled = true;
            Visible = true;
            Opacity = 1.0f;

            Children = new List<GuiElement>();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public virtual void LoadContent()
        {
            Children.ForEach(x => x.LoadContent());

            Destroyed = false;

            //InputManager.Instance.MouseButtonPressed += InputManager_OnMouseButtonPressed;
            InputManager.Instance.MouseMoved += InputManager_OnMouseMoved;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            Children.ForEach(x => x.UnloadContent());

            //InputManager.Instance.MouseButtonPressed -= InputManager_OnMouseButtonPressed;
            InputManager.Instance.MouseMoved -= InputManager_OnMouseMoved;

            Destroyed = true;
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            Children.RemoveAll(w => w.Destroyed);

            foreach (GuiElement guiElement in Children.Where(w => w.Enabled))
            {
                guiElement.Update(gameTime);
            }
        }

        /// <summary>
        /// Handles the input.
        /// </summary>
        public void HandleInput()
        {
            Vector2 mousePos = InputManager.Instance.MousePosition;

            if (Enabled && Visible && ScreenArea.Contains(mousePos) &&
                !InputManager.Instance.MouseButtonInputHandled &&
                InputManager.Instance.IsLeftMouseButtonClicked() &&
                Clicked != null)
            {
                MouseButtonEventArgs e = new MouseButtonEventArgs(MouseButton.LeftButton, MouseButtonState.Pressed, mousePos);

                InputManager_OnMouseButtonPressed(this, e);
                InputManager.Instance.MouseButtonInputHandled = true;
            }

            foreach (GuiElement child in Children)
            {
                if (InputManager.Instance.MouseButtonInputHandled)
                {
                    break;
                }

                child.HandleInput();
            }
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (GuiElement guiElement in Children.Where(w => w.Visible))
            {
                guiElement.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Destroys this GUI element.
        /// </summary>
        public virtual void Destroy()
        {
            UnloadContent();

            Destroyed = true;
        }

        /// <summary>
        /// Shows this GUI element.
        /// </summary>
        public virtual void Show()
        {
            Enabled = true;
            Visible = true;
        }

        /// <summary>
        /// Hide this GUI element.
        /// </summary>
        public virtual void Hide()
        {
            Enabled = false;
            Visible = false;
        }

        /// <summary>
        /// Fired by the Clicked event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnClicked(object sender, MouseButtonEventArgs e)
        {
            if (Clicked != null)
            {
                Clicked(this, e);
            }
        }

        /// <summary>
        /// Fired by the MouseMoved event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseMoved(object sender, MouseEventArgs e)
        {
            if (MouseMoved != null)
            {
                MouseMoved(this, e);
            }
        }

        /// <summary>
        /// Fired by the MouseEntered event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseEntered(object sender, MouseEventArgs e)
        {
            if (MouseEntered != null)
            {
                MouseEntered(this, e);
            }
        }

        /// <summary>
        /// Fired by the MouseLeft event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseLeft(object sender, MouseEventArgs e)
        {
            if (MouseLeft != null)
            {
                MouseLeft(this, e);
            }
        }

        void InputManager_OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (!Enabled || !Visible)
            {
                return;
            }

            if (ScreenArea.Contains(e.MousePosition))
            {
                OnClicked(this, e);
            }
        }

        void InputManager_OnMouseMoved(object sender, MouseEventArgs e)
        {
            if (!Enabled || !Visible)
            {
                return;
            }

            if (ScreenArea.Contains(e.CurrentMousePosition) ||
                ScreenArea.Contains(e.PreviousMousePosition))
            {
                OnMouseMoved(this, e);

                if (!ScreenArea.Contains(e.PreviousMousePosition))
                {
                    OnMouseEntered(this, e);
                }
                else if (!ScreenArea.Contains(e.CurrentMousePosition))
                {
                    OnMouseLeft(this, e);
                }
            }
        }
    }
}