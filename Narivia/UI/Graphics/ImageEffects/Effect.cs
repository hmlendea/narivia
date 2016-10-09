using Microsoft.Xna.Framework;

using Narivia.UI.Graphics;

namespace Narivia.UI.Graphics.ImageEffects
{
    public class Effect
    {
        protected Image Image;

        public bool Active { get; set; }

        /*===== Constructor =====*/

        public Effect()
        {
            Active = false;
        }

        public virtual void LoadContent(ref Image image)
        {
            Image = image;
        }

        public virtual void UnloadContent()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}

