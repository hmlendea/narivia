using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using NuciLog;
using NuciLog.Enumerations;

namespace Narivia.DataAccess.Resources
{
    /// <summary>
    /// Resource Manager that can load content either from the Content Pipeline or from disk.
    /// </summary>
    public class ResourceManager
    {
        static volatile ResourceManager instance;
        static object syncRoot = new object();

        ContentManager content;
        GraphicsDevice graphicsDevice { get; set; }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ResourceManager();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="content">Content manager.</param>
        /// <param name="graphicsDevice">Graphics device.</param>
        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.content = content;
            this.graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// Loads a sound effect either from the Content Pipeline or from disk (WAVs only).
        /// </summary>
        /// <returns>The sound effect.</returns>
        /// <param name="filePath">The path to the file (without extension).</param>
        public SoundEffect LoadSoundEffect(string filePath)
        {
            SoundEffect soundEffect;

            try
            {
                soundEffect = content.Load<SoundEffect>(filePath);
            }
            catch
            {
                soundEffect = SoundEffect.FromStream(File.OpenRead(Path.Combine(content.RootDirectory, $"{filePath}.wav")));
            }

            return soundEffect;
        }

        /// <summary>
        /// Loads a sprite font from the Content Pipeline.
        /// </summary>
        /// <returns>The sprite font.</returns>
        /// <param name="filePath">The path to the file (without extension).</param>
        public SpriteFont LoadSpriteFont(string filePath)
        => content.Load<SpriteFont>(filePath);

        /// <summary>
        /// Loads a 2D texture either from the Content Pipeline or from disk (PNGs only).
        /// </summary>
        /// <returns>The 2D texture.</returns>
        /// <param name="filePath">The path to the file (without extension).</param>
        public Texture2D LoadTexture2D(string filePath)
        {
            Texture2D texture2D = null;

            try
            {
                texture2D = content.Load<Texture2D>(filePath);
            }
            catch
            {
                string diskFilePath = Path.Combine(content.RootDirectory, $"{filePath}.png");

                if (File.Exists(diskFilePath))
                {
                    texture2D = Texture2D.FromStream(graphicsDevice, File.OpenRead(diskFilePath));
                }
            }

            if (texture2D == null)
            {
                texture2D = content.Load<Texture2D>("ScreenManager/missing-texture");

                LogManager.Instance.Warn(LogBuilder.BuildKvpMessage(Operation.ContentFileLoad,
                                                                    OperationStatus.Failure,
                                                                    new Dictionary<LogInfoKey, string> { { LogInfoKey.FileName, filePath } }));
            }

            return texture2D;
        }
    }
}
