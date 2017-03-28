using System.Xml.Serialization;

using Microsoft.Xna.Framework;

namespace Narivia.Graphics.ImageEffects
{
    public class AnimationEffect : ImageEffect
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
                if (Image.Texture == null)
                {
                    return 0;
                }

                return Image.Texture.Width / (int)FrameAmount.X;
            }
        }

        [XmlIgnore]
        public int FrameHeight
        {
            get
            {
                if (Image.Texture == null)
                {
                    return 0;
                }

                return Image.Texture.Height / (int)FrameAmount.Y;
            }
        }
        
        public AnimationEffect()
        {
            FrameAmount = Vector2.Zero;
            CurrentFrame = Vector2.Zero;
            SwitchFrame = 100;
            FrameCounter = 0;
        }
        
        public override void LoadContent(ref Image image)
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

            if (Image.Active)
            {
                FrameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (FrameCounter >= SwitchFrame)
                {
                    FrameCounter = 0;
                    newFrame.X += 1;

                    if (CurrentFrame.X >= Image.Texture.Width / FrameWidth - 1)
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

            Image.SourceRectangle = new Rectangle(
                (int)CurrentFrame.X * FrameWidth,
                (int)CurrentFrame.Y * FrameHeight,
                FrameWidth,
                FrameHeight);
        }
    }
}