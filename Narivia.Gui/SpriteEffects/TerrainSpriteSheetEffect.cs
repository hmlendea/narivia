using System.Collections.Generic;
using System.Linq;

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
            world = worldManager.GetWorld();
        }

        protected override void DoUpdate(GameTime gameTime)
        {
            List<string> id = world.Tiles[TileLocation.X, TileLocation.Y].TerrainIds;
            List<string> idN = world.Tiles[TileLocation.X, TileLocation.Y - 1].TerrainIds;
            List<string> idW = world.Tiles[TileLocation.X - 1, TileLocation.Y].TerrainIds;
            List<string> idS = world.Tiles[TileLocation.X, TileLocation.Y + 1].TerrainIds;
            List<string> idE = world.Tiles[TileLocation.X + 1, TileLocation.Y].TerrainIds;
            List<string> idNW = world.Tiles[TileLocation.X - 1, TileLocation.Y - 1].TerrainIds;
            List<string> idNE = world.Tiles[TileLocation.X + 1, TileLocation.Y - 1].TerrainIds;
            List<string> idSW = world.Tiles[TileLocation.X - 1, TileLocation.Y + 1].TerrainIds;
            List<string> idSE = world.Tiles[TileLocation.X + 1, TileLocation.Y + 1].TerrainIds;

            bool tilesN = TilesWith.Intersect(idN).Any();
            bool tilesW = TilesWith.Intersect(idW).Any();
            bool tilesS = TilesWith.Intersect(idS).Any();
            bool tilesE = TilesWith.Intersect(idE).Any();
            bool tilesNW = TilesWith.Intersect(idNW).Any();
            bool tilesNE = TilesWith.Intersect(idNE).Any();
            bool tilesSW = TilesWith.Intersect(idSW).Any();
            bool tilesSE = TilesWith.Intersect(idSE).Any();

            if (tilesN && tilesW && tilesS && tilesE && tilesNW && tilesNE && tilesSW && tilesSE)
            {
                CurrentFrame = new Point2D(1, 3); // Middle
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesNW && !tilesNE && !tilesSW && !tilesSE)
            {
                CurrentFrame = new Point2D(7, 3); // +
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesNW && !tilesNE && tilesSW && tilesSE)
            {
                CurrentFrame = new Point2D(3, 0);
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesNW && tilesNE && !tilesSW && tilesSE)
            {
                CurrentFrame = new Point2D(4, 0);
            }
            else if (tilesN && tilesW && tilesS && tilesE && tilesNW && tilesNE && !tilesSW && !tilesSE)
            {
                CurrentFrame = new Point2D(3, 1);
            }
            else if (tilesN && tilesW && tilesS && tilesE && tilesNW && !tilesNE && tilesSW && !tilesSE)
            {
                CurrentFrame = new Point2D(4, 1);
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesNW && !tilesNE && !tilesSW && tilesSE)
            {
                CurrentFrame = new Point2D(5, 0);
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesNW && !tilesNE && !tilesSW && tilesSE)
            {
                CurrentFrame = new Point2D(6, 0);
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesNW && tilesNE && !tilesSW && !tilesSE)
            {
                CurrentFrame = new Point2D(5, 1);
            }
            else if (tilesN && tilesW && tilesS && tilesE && tilesNW && !tilesNE && !tilesSW && !tilesSE)
            {
                CurrentFrame = new Point2D(6, 1);
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesNW && !tilesNE && tilesSW && !tilesSE)
            {
                CurrentFrame = new Point2D(7, 0);
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesNW && tilesNE && !tilesSW && !tilesSE)
            {
                CurrentFrame = new Point2D(8, 0);
            }
            else if (tilesN && tilesW && tilesS && tilesE && tilesNW && !tilesNE && !tilesSW && !tilesSE)
            {
                CurrentFrame = new Point2D(7, 1);
            }
            else if (tilesN && tilesW && tilesS && tilesE && tilesNW && !tilesNE && !tilesSW && !tilesSE)
            {
                CurrentFrame = new Point2D(8, 1);
            }
            else if (tilesN && tilesW && !tilesS && tilesE && !tilesNW && !tilesNE)
            {
                CurrentFrame = new Point2D(8, 2); // _|_
            }
            else if (tilesN && tilesW && tilesS && !tilesE && !tilesNW && !tilesSW)
            {
                CurrentFrame = new Point2D(8, 3); // -|
            }
            else if (!tilesN && tilesW && tilesS && tilesE && !tilesSW && !tilesSE)
            {
                CurrentFrame = new Point2D(8, 4); // T
            }
            else if (tilesN && !tilesW && tilesS && tilesE && !tilesNE && !tilesSE)
            {
                CurrentFrame = new Point2D(8, 5); // |-
            }
            else if (tilesN && !tilesW && tilesS && tilesE && !tilesNE && tilesSE)
            {
                CurrentFrame = new Point2D(3, 2);
            }
            else if (tilesN && tilesW && tilesS && !tilesE && !tilesNW && tilesSW)
            {
                CurrentFrame = new Point2D(4, 2);
            }
            else if (tilesN && !tilesW && tilesS && tilesE && tilesNE && !tilesSE)
            {
                CurrentFrame = new Point2D(3, 3);
            }
            else if (tilesN && tilesW && tilesS && !tilesE && tilesNW && !tilesSW)
            {
                CurrentFrame = new Point2D(4, 3);
            }
            else if (!tilesN && tilesW && tilesS && tilesE && !tilesSW && tilesSE)
            {
                CurrentFrame = new Point2D(3, 4);
            }
            else if (!tilesN && tilesW && tilesS && tilesE && tilesSW && !tilesSE)
            {
                CurrentFrame = new Point2D(4, 4);
            }
            else if (tilesN && tilesW && !tilesS && tilesE && !tilesNW && tilesNE)
            {
                CurrentFrame = new Point2D(3, 5);
            }
            else if (tilesN && tilesW && !tilesS && tilesE && tilesNW && !tilesNE)
            {
                CurrentFrame = new Point2D(4, 5);
            }
            else if (!tilesN && !tilesW && tilesS && tilesE && !tilesNW && !tilesSE)
            {
                CurrentFrame = new Point2D(5, 2);
            }
            else if (!tilesN && tilesW && tilesS && !tilesE && !tilesNE && !tilesSW)
            {
                CurrentFrame = new Point2D(6, 2);
            }
            else if (tilesN && !tilesW && !tilesS && tilesE && !tilesNE && !tilesSW)
            {
                CurrentFrame = new Point2D(5, 3);
            }
            else if (tilesN && tilesW && !tilesS && !tilesE && !tilesNW && !tilesSE)
            {
                CurrentFrame = new Point2D(6, 3);
            }
            else if (!tilesN && !tilesW && !tilesS && !tilesE)
            {
                CurrentFrame = new Point2D(0, 0); // Single
            }
            else if (!tilesN && !tilesW && tilesS && !tilesE)
            {
                CurrentFrame = new Point2D(5, 4); // ^
            }
            else if (tilesN && !tilesW && !tilesS && !tilesE)
            {
                CurrentFrame = new Point2D(5, 5); // v
            }
            else if (!tilesN && !tilesW && !tilesS && tilesE)
            {
                CurrentFrame = new Point2D(6, 4); // <
            }
            else if (!tilesN && tilesW && !tilesS && !tilesE)
            {
                CurrentFrame = new Point2D(6, 5); // >
            }
            else if (tilesN && !tilesW && tilesS && !tilesE)
            {
                CurrentFrame = new Point2D(7, 4); // ||
            }
            else if (!tilesN && tilesW && !tilesS && tilesE)
            {
                CurrentFrame = new Point2D(7, 5); // =
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
            else if (tilesN && tilesW && tilesS && tilesE)
            {
                if (!tilesNW)
                {
                    CurrentFrame = new Point2D(2, 1); // _|
                }
                else if (!tilesNE)
                {
                    CurrentFrame = new Point2D(1, 1); // |_
                }
                else if (!tilesSW)
                {
                    CurrentFrame = new Point2D(2, 0); // /
                }
                else if (!tilesSE)
                {
                    CurrentFrame = new Point2D(1, 0); // 7
                }
            }
        }
    }
}
