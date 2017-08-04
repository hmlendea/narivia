using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class GuiElement : IComponent, IDisposable
    {
        public string Id { get; set; }
        
        /// <summary>
        /// Gets the position of this <see cref="GuiElement"/>.
        /// </summary>
        /// <value>The position.</value>
        public Point Position { get; set; }

        /// <summary>
        /// Gets the size of this <see cref="GuiElement"/>.
        /// </summary>
        /// <value>The size.</value>
        public virtual Point Size { get; set; }

        /// <summary>
        /// Gets the screen area covered by this <see cref="GuiElement"/>.
        /// </summary>
        /// <value>The screen area.</value>
        public Rectangle ScreenArea => new Rectangle(Position.X, Position.Y, Size.X, Size.Y);

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        public float Opacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the children GUI elements.
        /// </summary>
        /// <value>The children.</value>
        public List<GuiElement> Children { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/> is destroyed.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool IsDisposed { get; private set; }

        [XmlIgnore]
        public ISite Site { get; set; }

        [XmlIgnore]
        protected virtual bool CanRaiseEvents => true;

        [XmlIgnore]
        public IContainer Container
        {
            get
            {
                if (Site == null)
                {
                    return null;
                }

                return Site.Container;
            }
        }

        [XmlIgnore]
        protected bool DesignMode
        {
            get
            {
                if (Site == null)
                {
                    return false;
                }

                return Site.DesignMode;
            }
        }

        /// <summary>
        /// Occurs when clicked.
        /// </summary>
        public event MouseButtonEventHandler Clicked;

        /// <summary>
        /// Occurs when the mouse moved.
        /// </summary>
        public event MouseEventHandler MouseMoved;

        /// <summary>
        /// Occurs when the mouse entered this <see cref="GuiElement"/>.
        /// </summary>
        public event MouseEventHandler MouseEntered;

        /// <summary>
        /// Occurs when the mouse left this <see cref="GuiElement"/>.
        /// </summary>
        public event MouseEventHandler MouseLeft;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> was disposed.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiElement"/> class.
        /// </summary>
        public GuiElement()
        {
            Enabled = true;
            Visible = true;
            Opacity = 1.0f;

            Id = Guid.NewGuid().ToString();

            Children = new List<GuiElement>();
        }

        ~GuiElement()
        {
            Dispose();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public virtual void LoadContent()
        {
            SetChildrenProperties();

            Children.ForEach(x => x.LoadContent());

            IsDisposed = false;

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
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            Children.RemoveAll(w => w.IsDisposed);

            SetChildrenProperties();

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
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
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
        /// Disposes of this <see cref="GuiElement"/>.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

            Children.ForEach(c => c.Dispose());
        }

        /// <summary>
        /// Disposes of this <see cref="GuiElement"/>.
        /// </summary>
        protected void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            IsDisposed = true;

            lock (this)
            {
                if (Site != null && Site.Container != null)
                {
                    Site.Container.Remove(this);
                }

                UnloadContent();

                OnDisposed(this, EventArgs.Empty);
            }
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

        protected virtual void SetChildrenProperties()
        {
        }

        protected virtual object GetService(Type service)
        {
            if (Site == null)
            {
                return null;
            }

            return Site.GetService(service);
        }

        public override string ToString()
        {
            if (Site == null)
            {
                return GetType().FullName;
            }

            return $"{Site.Name} [{GetType().FullName}]";
        }

        /// <summary>
        /// Fired by the Disposed event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnDisposed(object sender, EventArgs e)
        {
            if (Disposed != null)
            {
                Disposed(sender, e);
            }
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
                Clicked(sender, e);
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

        void NormaliseProperties()
        {
            if (Opacity > 1.0f)
            {
                Opacity = 1.0f;
            }
            else if (Opacity < 0.0f)
            {
                Opacity = 0.0f;
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
