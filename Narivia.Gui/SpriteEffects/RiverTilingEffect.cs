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

            bool tilesN = world.Tiles[TileLocation.X, TileLocation.Y - 1].HasRiver;
            bool tilesW = world.Tiles[TileLocation.X - 1, TileLocation.Y].HasRiver;
            bool tilesS = world.Tiles[TileLocation.X, TileLocation.Y + 1].HasRiver;
            bool tilesE = world.Tiles[TileLocation.X + 1, TileLocation.Y].HasRiver;

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
                if (tilesN)
                {
                    CurrentFrame = new Point2D(3, 0);
                }
                else if (tilesW)
                {
                    CurrentFrame = new Point2D(4, 0);
                }
                else if (tilesS)
                {
                    CurrentFrame = new Point2D(3, 1);
                }
                else if (tilesE)
                {
                    CurrentFrame = new Point2D(4, 1);
                }
            }
            else if (tilesN && tilesW && tilesS && tilesE) // +
            {
                CurrentFrame = new Point2D(0, 2);
            }
            else if (tilesN && tilesW && !tilesS && tilesE) // _|_
            {
                CurrentFrame = new Point2D(0, 3);
            }
            else if (tilesN && !tilesW && tilesS && tilesE) // |-
            {
                CurrentFrame = new Point2D(1, 3);
            }
            else if (tilesN && tilesW && tilesS && !tilesE) // -|
            {
                CurrentFrame = new Point2D(0, 4);
            }
            else if (!tilesN && tilesW && tilesS && tilesE) // T
            {
                CurrentFrame = new Point2D(1, 4);
            }
            else if (!tilesN && !tilesW && tilesS && tilesE) // /
            {
                CurrentFrame = new Point2D(1, 0);
            }
            else if (!tilesN && tilesW && tilesS && !tilesE) // 7
            {
                CurrentFrame = new Point2D(2, 0);
            }
            else if (tilesN && !tilesW && !tilesS && tilesE) // |_
            {
                CurrentFrame = new Point2D(1, 1);
            }
            else if (tilesN && tilesW && !tilesS && !tilesE) // _|
            {
                CurrentFrame = new Point2D(2, 1);
            }
            else if (!tilesW && !tilesE)
            {
                if (!tilesN && tilesS && !waterN) // ^
                {
                    CurrentFrame = new Point2D(0, 0);
                }
                else if ((tilesN || waterN) && (tilesS || waterS)) // ||
                {
                    CurrentFrame = new Point2D(2, 4);
                }
                else if (tilesN && !tilesS && !waterS) // V
                {
                    CurrentFrame = new Point2D(0, 1);
                }
            }
            else if (!tilesN && !tilesS)
            { 
                if (!tilesW && tilesE && !waterW) // <
                {
                    CurrentFrame = new Point2D(1, 2);
                }
                else if ((tilesW || waterW) && (tilesE || waterE)) // =
                {
                    CurrentFrame = new Point2D(2, 3);
                }
                else if (tilesW && !tilesE && !waterE) // >
                {
                    CurrentFrame = new Point2D(2, 2);
                }
            }
        }
    }
}
