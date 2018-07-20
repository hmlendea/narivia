using Microsoft.Xna.Framework;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers;
using Narivia.Models;

namespace Narivia.Gui.SpriteEffects
{
    public class RiverTilingEffect : SpriteSheetEffect
    {
        readonly IWorldManager worldManager;

        readonly World world;

        public Point2D TileLocation { get; set; }

        public RiverTilingEffect(IWorldManager worldManager)
        {
            FrameAmount = new Size2D(3, 6);

            this.worldManager = worldManager;
            this.world = worldManager.GetWorld();
        }

        public override void UpdateFrame(GameTime gameTime)
        {
            CurrentFrame = new Point2D(0, 5);

            bool isRiver = world.Tiles[TileLocation.X, TileLocation.Y].HasRiver;
            bool isWater = world.Tiles[TileLocation.X, TileLocation.Y].HasWater;

            bool riverN = world.Tiles[TileLocation.X, TileLocation.Y - 1].HasRiver;
            bool riverW = world.Tiles[TileLocation.X - 1, TileLocation.Y].HasRiver;
            bool riverS = world.Tiles[TileLocation.X, TileLocation.Y + 1].HasRiver;
            bool riverE = world.Tiles[TileLocation.X + 1, TileLocation.Y].HasRiver;
            bool riverNW = world.Tiles[TileLocation.X - 1, TileLocation.Y - 1].HasRiver;
            bool riverNE = world.Tiles[TileLocation.X + 1, TileLocation.Y - 1].HasRiver;
            bool riverSW = world.Tiles[TileLocation.X - 1, TileLocation.Y + 1].HasRiver;
            bool riverSE = world.Tiles[TileLocation.X + 1, TileLocation.Y + 1].HasRiver;

            bool waterN = world.Tiles[TileLocation.X, TileLocation.Y - 1].HasWater;
            bool waterW = world.Tiles[TileLocation.X - 1, TileLocation.Y].HasWater;
            bool waterS = world.Tiles[TileLocation.X, TileLocation.Y + 1].HasWater;
            bool waterE = world.Tiles[TileLocation.X + 1, TileLocation.Y].HasWater;
            bool waterNW = world.Tiles[TileLocation.X - 1, TileLocation.Y - 1].HasWater;
            bool waterNE = world.Tiles[TileLocation.X + 1, TileLocation.Y - 1].HasWater;
            bool waterSW = world.Tiles[TileLocation.X - 1, TileLocation.Y + 1].HasWater;
            bool waterSE = world.Tiles[TileLocation.X + 1, TileLocation.Y + 1].HasWater;

            if (!isRiver && isWater)
            {
                if (riverN && waterW && waterE)
                {
                    CurrentFrame = new Point2D(3, 0);
                }
                else if (riverN && !waterW && waterE && !(riverNW && waterNE))
                {
                    CurrentFrame = new Point2D(3, 2);
                }
                else if (riverN && waterW && !waterE && !(riverNE && waterNW))
                {
                    CurrentFrame = new Point2D(4, 2);
                }
                else if (riverW && waterN && waterS)
                {
                    CurrentFrame = new Point2D(4, 0);
                }
                else if (riverS && waterW && waterE && !(riverSE && waterNE))
                {
                    CurrentFrame = new Point2D(3, 1);
                }
                else if (riverS && !waterW && waterE && !(riverSW && waterSE))
                {
                    CurrentFrame = new Point2D(3, 3);
                }
                else if (riverS && waterW && !waterE)
                {
                    CurrentFrame = new Point2D(4, 3);
                }
                else if (riverE && waterN && waterS && !riverNE)
                {
                    CurrentFrame = new Point2D(4, 1);
                }
                else if (riverW && waterS && waterE && !(riverNW && waterSW))
                {
                    CurrentFrame = new Point2D(3, 4);
                }
                else if (riverE && waterW && waterS && !(riverNE && waterSE || waterN))
                {
                    CurrentFrame = new Point2D(4, 4);
                }
                else if (riverW && waterN && waterE)
                {
                    CurrentFrame = new Point2D(3, 5);
                }
                else if (riverE && waterN && waterW && !(riverSE && waterNE || waterS))
                {
                    CurrentFrame = new Point2D(4, 5);
                }
            }
            else if (riverN && riverW && riverS && riverE)
            {
                CurrentFrame = new Point2D(0, 2); // +
            }
            else if (riverN && riverW && !riverS && riverE)
            {
                CurrentFrame = new Point2D(0, 3); // _|_
            }
            else if (riverN && riverW && riverS && !riverE)
            {
                CurrentFrame = new Point2D(1, 3); // -|
            }
            else if (!riverN && riverW && riverS && riverE)
            {
                CurrentFrame = new Point2D(0, 4); // T
            }
            else if (riverN && !riverW && riverS && riverE)
            {
                CurrentFrame = new Point2D(1, 4); // |-
            }
            else if (riverN && !riverW && riverS && !riverE ||
                     ((waterN || waterS) && !riverW && !riverE)) // ||
            {
                CurrentFrame = new Point2D(2, 4);
            }
            else if (!riverN && riverW && !riverS && riverE ||
                     ((waterW || waterE) && !riverN && !riverS)) // ||
            {
                CurrentFrame = new Point2D(2, 3);
            }
            else if ((!riverN && !riverW && riverS && riverE) || // /
                     ((riverE && waterS) || (riverS && waterE)))
            {
                CurrentFrame = new Point2D(1, 0);
            }
            else if ((!riverN && riverW && riverS && !riverE) || // 7
                     ((riverW && waterS) || (riverS && waterW)))
            {
                CurrentFrame = new Point2D(2, 0);
            }
            else if ((riverN && !riverW && !riverS && riverE) || // |_
                     ((riverE && waterN) || (riverN && waterE)))
            {
                CurrentFrame = new Point2D(1, 1);
            }
            else if ((riverN && riverW && !riverS && !riverE) || // _|
                     ((riverW && waterN) || (riverN && waterW)))
            {
                CurrentFrame = new Point2D(2, 1);
            }
            else if (!riverW && !riverE && !waterW && !waterE)
            {
                if (!riverN && riverS && !waterN) // ^
                {
                    CurrentFrame = new Point2D(0, 0);
                }
                else if (riverN && !riverS && !waterS) // V
                {
                    CurrentFrame = new Point2D(0, 1);
                }
            }
            else if (!riverN && !riverS && !waterN && !waterS)
            { 
                if (!riverW && riverE && !waterW) // <
                {
                    CurrentFrame = new Point2D(1, 2);
                }
                else if (riverW && !riverE && !waterE) // >
                {
                    CurrentFrame = new Point2D(2, 2);
                }
            }
        }
    }
}
