using System.Collections.Generic;
using System.Linq;

namespace Narivia.Helpers
{
    public static class FramerateCounter
    {
        /// <summary>
        /// Gets the total number of frames.
        /// </summary>
        /// <value>The total number of frames.</value>
        public static long TotalFrames { get; private set; }

        /// <summary>
        /// Gets the total seconds.
        /// </summary>
        /// <value>The total seconds.</value>
        public static float TotalSeconds { get; private set; }

        /// <summary>
        /// Gets the average frames per second.
        /// </summary>
        /// <value>The average frames per second.</value>
        public static float AverageFramesPerSecond { get; private set; }

        /// <summary>
        /// Gets the current frames per second.
        /// </summary>
        /// <value>The current frames per second.</value>
        public static float CurrentFramesPerSecond { get; private set; }

        /// <summary>
        /// The maximum samples.
        /// </summary>
        public const int MAXIMUM_SAMPLES = 100;

        static Queue<float> sampleBuffer = new Queue<float>();

        /// <summary>
        /// Updates the framerate.
        /// </summary>
        /// <param name="deltaTime">Delta time.</param>
        public static void Update(float deltaTime)
        {
            CurrentFramesPerSecond = 1.0f / deltaTime;

            sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (sampleBuffer.Count > MAXIMUM_SAMPLES)
            {
                sampleBuffer.Dequeue();
                AverageFramesPerSecond = sampleBuffer.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames += 1;
            TotalSeconds += deltaTime;
        }
    }
}
