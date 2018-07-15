using Microsoft.Xna.Framework;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers;
using Narivia.Models;
using Narivia.Models.Enumerations;

namespace Narivia.Gui.SpriteEffects
{
    public class ProvinceBorderEffect : SpriteSheetEffect
    {
        readonly IGameManager game;

        readonly World world;

        public Point2D TileLocation { get; set; }

        public ProvinceBorderEffect(IGameManager game)
        {
            FrameAmount = new Size2D(7, 6);

            this.game = game;
            this.world = game.GetWorld();
        }

        public override void UpdateFrame(GameTime gameTime)
        {
            CurrentFrame = new Point2D(1, 3); // Default

            string id = world.Tiles[TileLocation.X, TileLocation.Y].ProvinceId;
            string idN = world.Tiles[TileLocation.X, TileLocation.Y - 1].ProvinceId;
            string idW = world.Tiles[TileLocation.X - 1, TileLocation.Y].ProvinceId;
            string idS = world.Tiles[TileLocation.X, TileLocation.Y + 1].ProvinceId;
            string idE = world.Tiles[TileLocation.X + 1, TileLocation.Y].ProvinceId;
            string idNW = world.Tiles[TileLocation.X - 1, TileLocation.Y - 1].ProvinceId;
            string idNE = world.Tiles[TileLocation.X + 1, TileLocation.Y - 1].ProvinceId;
            string idSW = world.Tiles[TileLocation.X - 1, TileLocation.Y + 1].ProvinceId;
            string idSE = world.Tiles[TileLocation.X + 1, TileLocation.Y + 1].ProvinceId;

            bool tilesN = id == idN;
            bool tilesW = id == idW;
            bool tilesS = id == idS;
            bool tilesE = id == idE;
            bool tilesNW = id == idNW;
            bool tilesNE = id == idNE;
            bool tilesSW = id == idSW;
            bool tilesSE = id == idSE;

            if (tilesN && tilesW && tilesS && tilesE && tilesNW && tilesNE && tilesSW && tilesSE) // Middle
            {
                CurrentFrame = new Point2D(1, 3);
            }
            else if (!tilesN && !tilesW && !tilesS && !tilesE && !tilesNW && !tilesNE && !tilesSW && !tilesSE) // Single
            {
                CurrentFrame = new Point2D(0, 0);
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesNW && tilesNE && tilesSW && tilesSE) // TopLeftInnerCorner
            {
                CurrentFrame = new Point2D(2, 1);
            }
            else if (tilesN && tilesW && tilesS && tilesE && tilesNW && !tilesNE && tilesSW && tilesSE) // TopRightInnerCorner
            {
                CurrentFrame = new Point2D(1, 1);
            }
            else if (tilesN && tilesW && tilesS && tilesE && tilesNW && tilesNE && !tilesSW && tilesSE) // BottomLeftInnerCorner
            {
                CurrentFrame = new Point2D(2, 0);
            }
            else if (tilesN && tilesW && tilesS && tilesE && tilesNW && tilesNE && tilesSW && !tilesSE) // BottomRightInnerCorner
            {
                CurrentFrame = new Point2D(1, 0);
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesNW && !tilesNE && !tilesSW && !tilesSE) // TopLeftInnerCorner and TopRightInnerCorner and BottomLeftInnerCorner and BottomRightInnerCorner
            {
                CurrentFrame = new Point2D(8, 1);
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesNW && !tilesNE && tilesSW && tilesSE) // TopLeftInnerCorner and TopRightInnerCorner
            {
                CurrentFrame = new Point2D(8, 2);
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesNW && tilesNE && !tilesSW && tilesSE) // TopLeftInnerCorner and BottomLeftInnerCorner
            {
                CurrentFrame = new Point2D(6, 4);
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesNW && tilesNE && tilesSW && !tilesSE) // TopLeftInnerCorner and BottomRightInnerCorner
            {
                CurrentFrame = new Point2D(7, 0);
            }
            else if (tilesN && tilesW && tilesS && tilesE && tilesNW && !tilesNE && !tilesSW && tilesSE) // TopRightInnerCorner and BottomLeftInnerCorner
            {
                CurrentFrame = new Point2D(7, 2);
            }
            else if (tilesN && tilesW && tilesS && tilesE && tilesNW && !tilesNE && tilesSW && !tilesSE) // TopRightInnerCorner and BottomRightInnerCorner
            {
                CurrentFrame = new Point2D(7, 1);
            }
            else if (tilesN && tilesW && tilesS && tilesE && tilesNW && tilesNE && !tilesSW && !tilesSE) // BottomLeftInnerCorner and BottomRightInnerCorner
            {
                CurrentFrame = new Point2D(8, 0);
            }
            else if (tilesN && tilesW && tilesS && tilesE && tilesNW && tilesNE && tilesSW && tilesSE) // Middle
            {
                CurrentFrame = new Point2D(1, 3);
            }
            else if (!tilesN && !tilesW && tilesS && tilesE && !tilesNW && !tilesSE) // TopLeftOuterCorner and BottomRightInnerCorner
            {
                CurrentFrame = new Point2D(3, 2);
            }
            else if (!tilesN && tilesW && tilesS && !tilesE && !tilesNE && !tilesSW) // TopRightOuterCorner and BottomLeftInnerCorner
            {
                CurrentFrame = new Point2D(4, 2);
            }
            else if (tilesN && !tilesW && !tilesS && tilesE && !tilesSW && !tilesNE) // BottomLeftOuterCorner and TopRightInnerCorner
            {
                CurrentFrame = new Point2D(3, 3);
            }
            else if (tilesN && tilesW && !tilesS && !tilesE && !tilesSE && !tilesNW) // BottomRightOuterCorner and TopLeftInnerCorner
            {
                CurrentFrame = new Point2D(4, 3);
            }
            else if (!tilesN && tilesW && tilesS && tilesE && !tilesSW && !tilesSE) // Top with BottomLeftInnerCorner and BottomRightInnerCorner
            {
                CurrentFrame = new Point2D(5, 2);
            }
            else if (tilesN && !tilesW && tilesS && tilesE && !tilesNE && !tilesSE) // Left with TopRightInnerCorner and BottomRightInnerCorner
            {
                CurrentFrame = new Point2D(6, 2);
            }
            else if (tilesN && tilesW && !tilesS && tilesE && !tilesNW && !tilesNE) // Bottom with TopLeftInnerCorner and TopRightInnerCorner
            {
                CurrentFrame = new Point2D(5, 3);
            }
            else if (tilesN && tilesW && tilesS && !tilesE && !tilesNW && !tilesSW) // Right with TopLeftInnerCorner and BottomLeftInnerCorner
            {
                CurrentFrame = new Point2D(6, 3);
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesNE) // TopLeftInnerCorner and BottomLeftInnerCorner and BottomRightInnerCorner
            {
                CurrentFrame = new Point2D(8, 3);
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesNW) // TopRightInnerCorner and BottomLeftInnerCorner and BottomRightInnerCorner
            {
                CurrentFrame = new Point2D(7, 3);
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesSE) // TopLeftInnerCorner and TopRightInnerCorner and BottomLeftInnerCorner
            {
                CurrentFrame = new Point2D(8, 4);
            }
            else if (tilesN && tilesW && tilesS && tilesE && !tilesSW) // TopLeftInnerCorner and TopRightInnerCorner and BottomRightInnerCorner
            {
                CurrentFrame = new Point2D(7, 4);
            }
            else if (!tilesN && tilesW && tilesS && tilesE && !tilesSW) // Top with BottomLeftInnerCorner
            {
                CurrentFrame = new Point2D(4, 0);
            }
            else if (!tilesN && tilesW && tilesS && tilesE && !tilesSE) // Top with BottomRightInnerCorner
            {
                CurrentFrame = new Point2D(3, 0);
            }
            else if (tilesN && !tilesW && tilesS && tilesE && !tilesNE) // Left with TopRightInnerCorner
            {
                CurrentFrame = new Point2D(5, 1);
            }
            else if (tilesN && !tilesW && tilesS && tilesE && !tilesSE) // Left with BottomRightInnerCorner
            {
                CurrentFrame = new Point2D(5, 0);
            }
            else if (tilesN && tilesW && !tilesS && tilesE && !tilesNW) // Bottom with TopLeftInnerCorner
            {
                CurrentFrame = new Point2D(4, 1);
            }
            else if (tilesN && tilesW && !tilesS && tilesE && !tilesNE) // Bottom with TopRightInnerCorner
            {
                CurrentFrame = new Point2D(3, 1);
            }
            else if (tilesN && tilesW && tilesS && !tilesE && !tilesNW) // Right with TopLeftInnerCorner
            {
                CurrentFrame = new Point2D(6, 1);
            }
            else if (tilesN && tilesW && tilesS && !tilesE && !tilesSW) // Right with BottomLeftInnerCorner
            {
                CurrentFrame = new Point2D(6, 0);
            }
            else if (!tilesN && !tilesW && tilesS && tilesE && !tilesNW) // TopLeftOuterCorner
            {
                CurrentFrame = new Point2D(0, 2);
            }
            else if (!tilesN && tilesW && tilesS && !tilesE && !tilesNE) // TopRightOuterCorner
            {
                CurrentFrame = new Point2D(2, 2);
            }
            else if (tilesN && !tilesW && !tilesS && tilesE && !tilesSW) // BottomLeftOuterCorner
            {
                CurrentFrame = new Point2D(0, 4);
            }
            else if (tilesN && tilesW && !tilesS && !tilesE && !tilesSE) // BottomRightOuterCorner
            {
                CurrentFrame = new Point2D(2, 4);
            }
            else if (!tilesN && !tilesW && !tilesS && tilesE) // <
            {
                CurrentFrame = new Point2D(3, 4);
            }
            else if (!tilesN && tilesW && !tilesS && !tilesE) // >
            {
                CurrentFrame = new Point2D(5, 4);
            }
            else if (!tilesN && !tilesW && tilesS && !tilesE) // ^
            {
                CurrentFrame = new Point2D(3, 5);
            }
            else if (tilesN && !tilesW && !tilesS && !tilesE) // v
            {
                CurrentFrame = new Point2D(5, 5);
            }
            else if (!tilesN && tilesW && !tilesS && tilesE) // =
            {
                CurrentFrame = new Point2D(4, 4);
            }
            else if (tilesN && !tilesW && tilesS && !tilesE) // ||
            {
                CurrentFrame = new Point2D(4, 5);
            }
            else if (!tilesN && tilesW && tilesS && tilesE) // Top
            {
                CurrentFrame = new Point2D(1, 2);
            }
            else if (tilesN && !tilesW && tilesS && tilesE) // Left
            {
                CurrentFrame = new Point2D(0, 3);
            }
            else if (tilesN && tilesW && !tilesS && tilesE) // Bottom
            {
                CurrentFrame = new Point2D(1, 4);
            }
            else if (tilesN && tilesW && tilesS && !tilesE) // Right
            {
                CurrentFrame = new Point2D(2, 3);
            }
        }
    }
}
