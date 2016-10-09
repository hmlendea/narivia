using Microsoft.Xna.Framework;

namespace Narivia.UI.Graphics.ImageEffects
{
    public class FadeEffect : Effect
    {
        public float Speed { get; set; }

        public bool Increase { get; set; }

        public FadeEffect()
        {
            Speed = 1;
            Increase = false;
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

            if (Image.Active)
            {
                if (Increase == false)
                    Image.Opacity -= Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                else
                    Image.Opacity += Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);

                if (Image.Opacity < 0.0f)
                {
                    Increase = true;
                    Image.Opacity = 0.0f;
                }
                else if (Image.Opacity > 1.0f)
                {
                    Increase = false;
                    Image.Opacity = 1.0f;
                }
            }
            else
                Image.Opacity = 1.0f;
        }
    }
}
