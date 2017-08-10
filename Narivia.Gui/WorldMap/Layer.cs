using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Settings;

namespace Narivia.Gui.WorldMap
{
    /// <summary>
    /// Map layer.
    /// </summary>
    public class Layer
    {
        /// <summary>
        /// Gets or sets the tile map.
        /// </summary>
        /// <value>The tile map.</value>
        public int[,] TileMap { get; set; }

        /// <summary>
        /// Gets or sets the tileset.
        /// </summary>
        /// <value>The tileset.</value>
        public string Tileset { get; set; }

        /// <summary>
        /// Gets or sets the layer opacity.
        /// </summary>
        /// <value>The layer opacity.</value>
        public float Opacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Layer"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible { get; set; }

        Sprite sprite { get; set; }

        readonly List<Tile> tiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="Layer"/> class.
        /// </summary>
        public Layer()
        {
            tiles = new List<Tile>();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            int mapHeight = TileMap.GetLength(0);
            int mapWidth = TileMap.GetLength(1);

            sprite = new Sprite
            {
                ContentFile = $"World/Terrain/{Tileset}",
                Opacity = Opacity
            };

            ConcurrentBag<Tile> layerTiles = new ConcurrentBag<Tile>();

            sprite.LoadContent();

            int cols = (int)(sprite.TextureSize.X / GameDefines.MAP_TILE_SIZE);

            Parallel.For(0, mapHeight, y => Parallel.For(0, mapWidth, x =>
            {
                if (TileMap[x, y] >= 0)
                {
                    int gid = TileMap[x, y];

                    Rectangle sourceRectangle = new Rectangle(
                        gid % cols * GameDefines.MAP_TILE_SIZE,
                        gid / cols * GameDefines.MAP_TILE_SIZE,
                        GameDefines.MAP_TILE_SIZE,
                        GameDefines.MAP_TILE_SIZE);

                    Tile tile = new Tile();
                    tile.LoadContent(x, y, sourceRectangle);

                    layerTiles.Add(tile);
                }
            }));

            tiles.AddRange(layerTiles);
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            tiles.Clear();
            sprite.UnloadContent();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        /// <param name="camera">Camera.</param>
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Point camCoordsBegin = new Point(camera.Location.X / GameDefines.MAP_TILE_SIZE,
                                             camera.Location.Y / GameDefines.MAP_TILE_SIZE);
            Point camCoordsEnd = new Point(camCoordsBegin.X + camera.Size.Width / GameDefines.MAP_TILE_SIZE,
                                           camCoordsBegin.Y + camera.Size.Height / GameDefines.MAP_TILE_SIZE);

            List<Tile> tilesToDraw = tiles.Where(tile => tile.X >= camCoordsBegin.X - 1 && tile.X <= camCoordsEnd.X + 1 &&
                                                         tile.Y >= camCoordsBegin.Y - 1 && tile.Y <= camCoordsEnd.Y + 1).ToList();

            foreach (Tile tile in tilesToDraw)
            {
                sprite.Location = new Point(tile.X * GameDefines.MAP_TILE_SIZE - camera.Location.X,
                                            tile.Y * GameDefines.MAP_TILE_SIZE - camera.Location.Y);
                sprite.SourceRectangle = tile.SourceRectangle;
                sprite.Draw(spriteBatch);
            }
        }
    }
}
