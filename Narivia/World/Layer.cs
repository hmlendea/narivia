using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;

namespace Narivia.WorldMap
{
    public class Layer
    {
        public List<Tile> Tiles { get; private set; }
        
        public Layer()
        {
            Tiles = new List<Tile>();
        }

        public void LoadContent(Vector2 tileDimensions, Dictionary<string, Vector2> tileEntities)
        {
            foreach (KeyValuePair<string, Vector2> tileEntity in tileEntities)
            {
                Image image = new Image();
                Vector2 position = tileEntity.Value;
                string entityId = tileEntity.Key;

                Tile tile = new Tile();
                tile.LoadContent(image, position, entityId);

                Tiles.Add(tile);
            }
        }

        public void UnloadContent()
        {
            foreach(Tile tile in Tiles)
                tile.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Tile tile in Tiles)
                tile.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in Tiles)
                tile.Draw(spriteBatch);
        }
    }
}
