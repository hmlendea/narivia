using System.Xml.Serialization;

using Microsoft.Xna.Framework;

namespace Narivia.Graphics.SpriteEffects
{
    public class AnimationEffect : CustomSpriteEffect
    {
        public int FrameCounter { get; set; }

        public int SwitchFrame { get; set; }

        public Vector2 CurrentFrame { get; set; }

        public Vector2 FrameAmount { get; set; }

        [XmlIgnore]
        public int FrameWidth
        {
            get
            {
                if (Sprite.TextureSize == null)
                {
                    return 0;
                }

                return (int)(Sprite.TextureSize.X / FrameAmount.X);
            }
        }

        [XmlIgnore]
        public int FrameHeight
        {
            get
            {
                if (Sprite.TextureSize == null)
                {
                    return 0;
                }

                return (int)(Sprite.TextureSize.Y / FrameAmount.Y);
            }
        }
        
        public AnimationEffect()
        {
            FrameAmount = Vector2.Zero;
            CurrentFrame = Vector2.Zero;
            SwitchFrame = 100;
            FrameCounter = 0;
        }
        
        public override void LoadContent(ref Sprite image)
        {
            base.LoadContent(ref image);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Vector2 newFrame = CurrentFrame;

            if (Sprite.Active)
            {
                FrameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (FrameCounter >= SwitchFrame)
                {
                    FrameCounter = 0;
                    newFrame.X += 1;

                    if (CurrentFrame.X >= Sprite.TextureSize.X / FrameWidth - 1)
                    {
                        newFrame.X = 0;
                    }
                }
            }
            else
            {
                newFrame.X = 1;
            }

            CurrentFrame = newFrame;

            Sprite.SourceRectangle = new Rectangle(
                (int)CurrentFrame.X * FrameWidth,
                (int)CurrentFrame.Y * FrameHeight,
                FrameWidth,
                FrameHeight);
        }
    }
}