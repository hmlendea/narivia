using System;
using System.Xml.Serialization;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Narivia.Graphics.SpriteEffects
{
    /// <summary>
    /// Effect.
    /// </summary>
    public class CustomSpriteEffect
    {
        protected Sprite Sprite;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CustomSpriteEffect"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        [XmlIgnore]
        public Type Type { get; private set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        [XmlIgnore]
        public string Key { get { return Type.ToString().Split('.').Last(); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSpriteEffect"/> class.
        /// </summary>
        public CustomSpriteEffect()
        {
            Active = false;
            Type = GetType();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="sprite">Sprite.</param>
        public virtual void LoadContent(ref Sprite sprite)
        {
            Sprite = sprite;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {

        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {

        }
    }
}

