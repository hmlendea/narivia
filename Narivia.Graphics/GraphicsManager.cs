using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Narivia.Graphics
{
    /// <summary>
    /// Graphics Manager.
    /// </summary>
    public class GraphicsManager
    {
        static volatile GraphicsManager instance;
        static object syncRoot = new object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static GraphicsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new GraphicsManager();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets or sets the graphics device manager.
        /// </summary>
        /// <value>The graphics device manager.</value>
        public GraphicsDeviceManager Graphics { get; set; }

        /// <summary>
        /// Gets or sets the sprite batch.
        /// </summary>
        /// <value>The sprite batch.</value>
        public SpriteBatch SpriteBatch { get; set; }
    }
}
