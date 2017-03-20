using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.WorldMap.Entities;

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

        public void LoadContent()
        {
            Layers.ForEach(layer => layer.LoadContent(TileDimensions));
        }

        public void UnloadContent()
        {
            Layers.ForEach(layer => layer.UnloadContent());
        }

        public void Update(GameTime gameTime)
        {
            Layers.ForEach(layer => layer.Update(gameTime));
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Layers.ForEach(layer => layer.Draw(spriteBatch, camera));
        }
    }
}
