using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Narivia.WorldMap
{
    public class Map
    {
        public List<Layer> Layers { get; set; }

        public Vector2 TileDimensions { get; set; }
        
        public Map()
        {
            Layers = new List<Layer>();
            TileDimensions = Vector2.Zero;
        }

        public void LoadContent(Dictionary<string, Vector2> tileEntities)
        {
            foreach (Layer layer in Layers)
                layer.LoadContent(TileDimensions, tileEntities);
        }

        public void UnloadContent()
        {
            foreach (Layer layer in Layers)
                layer.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Layer layer in Layers)
                layer.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Layer layer in Layers)
                layer.Draw(spriteBatch);
        }
    }
}
