using Microsoft.Xna.Framework.Audio;

using Narivia.DataAccess.Resources;

namespace Narivia.Audio
{
    /// <summary>
    /// Audio manager.
    /// </summary>
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
        /// Plays the specified sound.
        /// </summary>
        /// <value>Plays the specified sound.</value>
        public void PlaySound(string sound)
        {
            SoundEffect soundEffect = ResourceManager.Instance.LoadSoundEffect("Audio/" + sound);

            soundEffect.CreateInstance().Play();
        }
    }
}
