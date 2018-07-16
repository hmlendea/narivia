using System.Collections.Generic;

using Microsoft.Xna.Framework;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers;
using Narivia.Models;

namespace Narivia.Gui.SpriteEffects
{
    public class TerrainSpriteSheetEffect : SpriteSheetEffect
    {
        readonly IWorldManager worldManager;

        readonly World world;

        public Point2D TileLocation { get; set; }

        public string TerrainId { get; set; }

        public List<string> TilesWith { get; set; }

        public TerrainSpriteSheetEffect(IWorldManager worldManager)
        {
            FrameAmount = new Size2D(3, 6);
            TilesWith = new List<string>();

            this.worldManager = worldManager;
            this.world = worldManager.GetWorld();
        }

        public override void UpdateFrame(GameTime gameTime)
        {
            string id = world.Tiles[TileLocation.X, TileLocation.Y].TerrainId;
            string idN = world.Tiles[TileLocation.X, TileLocation.Y - 1].TerrainId;
            string idW = world.Tiles[TileLocation.X - 1, TileLocation.Y].TerrainId;
            string idS = world.Tiles[TileLocation.X, TileLocation.Y + 1].TerrainId;
            string idE = world.Tiles[TileLocation.X + 1, TileLocation.Y].TerrainId;
            string idNW = world.Tiles[TileLocation.X - 1, TileLocation.Y - 1].TerrainId;
            string idNE = world.Tiles[TileLocation.X + 1, TileLocation.Y - 1].TerrainId;
            string idSW = world.Tiles[TileLocation.X - 1, TileLocation.Y + 1].TerrainId;
            string idSE = world.Tiles[TileLocation.X + 1, TileLocation.Y + 1].TerrainId;

            bool tilesN = TilesWith.Contains(idN);
            bool tilesW = TilesWith.Contains(idW);
            bool tilesS = TilesWith.Contains(idS);
            bool tilesE = TilesWith.Contains(idE);
            bool tilesNW = TilesWith.Contains(idNW);
            bool tilesNE = TilesWith.Contains(idNE);
            bool tilesSW = TilesWith.Contains(idSW);
            bool tilesSE = TilesWith.Contains(idSE);

            if (tilesN && tilesW && tilesS && tilesE) // Middle
            {
                CurrentFrame = new Point2D(1, 3);

                if (!tilesNW)
                {
                    CurrentFrame = new Point2D(2, 1);
                }
                else if (!tilesNE)
                {
                    CurrentFrame = new Point2D(1, 1);
                }
                else if (!tilesSW)
                {
                    CurrentFrame = new Point2D(2, 0);
                }
                else if (!tilesSE)
                {
                    CurrentFrame = new Point2D(1, 0);
                }
            }
            else if (!tilesN && !tilesW && !tilesS && !tilesE) // Single
            {
                CurrentFrame = new Point2D(0, 0);
            }
            else if (!tilesN && !tilesW && tilesS && tilesE) // TopLeftCorner
            {
                CurrentFrame = new Point2D(0, 2);
            }
            else if (!tilesN && tilesW && tilesS && !tilesE) // TopRightCorner
            {
                CurrentFrame = new Point2D(2, 2);
            }
            else if (tilesN && !tilesW && !tilesS && tilesE) // BottomLeftCorner
            {
                CurrentFrame = new Point2D(0, 4);
            }
            else if (tilesN && tilesW && !tilesS && !tilesE) // BottomRightCorner
            {
                CurrentFrame = new Point2D(2, 4);
            }
            else if (!tilesN && tilesW && tilesS && tilesE) // TopCorner
            {
                CurrentFrame = new Point2D(1, 2);
            }
            else if (tilesN && !tilesW && tilesS && tilesE) // LeftCorner
            {
                CurrentFrame = new Point2D(0, 3);
            }
            else if (tilesN && tilesW && !tilesS && tilesE) // BottomCorner
            {
                CurrentFrame = new Point2D(1, 4);
            }
            else if (tilesN && tilesW && tilesS && !tilesE) // RightCorner
            {
                CurrentFrame = new Point2D(2, 3);
            }
        }
    }
}
