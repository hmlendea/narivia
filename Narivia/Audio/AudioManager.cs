using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Narivia.Audio
{
    public class AudioManager
    {
        static volatile AudioManager instance;
        static object syncRoot = new object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new AudioManager();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>The content.</value>
        public ContentManager Content { get; private set; }


        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <value>The content.</value>
        public void LoadContent(ContentManager content)
        {
            Content = new ContentManager(content.ServiceProvider, "Content");
        }
        
        /// <summary>
        /// Plays the specified sound.
        /// </summary>
        /// <value>Plays the specified sound.</value>
        public void PlaySound(string sound)
        {
            SoundEffect soundEffect = Content.Load<SoundEffect>("Audio/" + sound);

            soundEffect.CreateInstance().Play();
        }
    }
}
