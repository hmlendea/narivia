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
        /// Occurs when this <see cref="GuiElement"/> was disposed.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Occurs when a mouse button was pressed on this <see cref="GuiElement"/>.
        /// </summary>
        public event MouseButtonEventHandler MouseButtonPressed;

        /// <summary>
        /// Occurs when the mouse entered this <see cref="GuiElement"/>.
        /// </summary>
        public event MouseEventHandler MouseEntered;

        /// <summary>
        /// Occurs when the mouse left this <see cref="GuiElement"/>.
        /// </summary>
        public event MouseEventHandler MouseLeft;

        /// <summary>
        /// Occurs when the mouse moved.
        /// </summary>
        public event MouseEventHandler MouseMoved;

        /// <summary>
        /// Occurs when the Position property value changes.
        /// </summary>
        public event EventHandler PositionChanged;

        /// <summary>
        /// Occurs when the Size property value changes.
        /// </summary>
        public event EventHandler SizeChanged;

        Point oldPosition;
        Point oldSize;

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
            InputManager.Instance.MouseButtonPressed += OnMouseButtonPressed;
            InputManager.Instance.MouseMoved += OnMouseEntered;
            InputManager.Instance.MouseMoved += OnMouseMoved;
            InputManager.Instance.MouseMoved += OnMouseLeft;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            Children.ForEach(x => x.UnloadContent());

            //InputManager.Instance.MouseButtonPressed -= InputManager_OnMouseButtonPressed;
            InputManager.Instance.MouseButtonPressed -= OnMouseButtonPressed;
            InputManager.Instance.MouseMoved -= OnMouseEntered;
            InputManager.Instance.MouseMoved -= OnMouseLeft;
            InputManager.Instance.MouseMoved -= OnMouseMoved;
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            Children.RemoveAll(w => w.IsDisposed);

            SetChildrenProperties();

            OnPositionChanged(this, null);
            OnSizeChanged(this, null);
            
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
                InputManager.Instance.IsLeftMouseButtonClicked())
            {
                MouseButtonEventArgs e = new MouseButtonEventArgs(MouseButton.LeftButton, MouseButtonState.Pressed, mousePos);

                OnClicked(this, e);
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
            oldPosition = Position;
            oldSize = Size;
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
        /// Fired by the Clicked event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnClicked(object sender, MouseButtonEventArgs e)
        {
            if (!Enabled || !Visible)
            {
                return;
            }

            if (ScreenArea.Contains(e.MousePosition))
            {
                if (Clicked != null)
                {
                    Clicked?.Invoke(sender, e);
                    InputManager.Instance.MouseButtonInputHandled = true;
                }
            }
        }

        /// <summary>
        /// Fired by the Disposed event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnDisposed(object sender, EventArgs e)
        {
            Disposed?.Invoke(sender, e);
        }

        /// <summary>
        /// Fired by the MouseClick event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (!Enabled || !Visible)
            {
                return;
            }

            if (ScreenArea.Contains(e.MousePosition))
            {
                MouseButtonPressed?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Fired by the MouseEntered event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseEntered(object sender, MouseEventArgs e)
        {
            if (!Enabled || !Visible)
            {
                return;
            }

            if (ScreenArea.Contains(e.CurrentMousePosition) &&
                !ScreenArea.Contains(e.PreviousMousePosition))
            {
                MouseEntered?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Fired by the MouseLeft event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseLeft(object sender, MouseEventArgs e)
        {
            if (!Enabled || !Visible)
            {
                return;
            }

            if (!ScreenArea.Contains(e.CurrentMousePosition) &&
                ScreenArea.Contains(e.PreviousMousePosition))
            {
                MouseLeft?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Fired by the MouseMoved event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseMoved(object sender, MouseEventArgs e)
        {
            if (!Enabled || !Visible)
            {
                return;
            }

            if (ScreenArea.Contains(e.CurrentMousePosition) &&
                ScreenArea.Contains(e.PreviousMousePosition) &&
                e.CurrentMousePosition != e.PreviousMousePosition)
            {
                MouseMoved?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raised by the PositionChanged event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnPositionChanged(object sender, EventArgs e)
        {
            if (!Enabled || !Visible)
            {
                return;
            }
            if (oldPosition != Position)
            {
                PositionChanged?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raised by the SizeChanged event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnSizeChanged(object sender, EventArgs e)
        {
            if (!Enabled || !Visible)
            {
                return;
            }
            if (oldSize != Size)
            {
                SizeChanged?.Invoke(this, e);
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
    }
}
