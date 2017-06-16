using System.Collections.Generic;

namespace Narivia.GameLogic.Generators.Interfaces
{
    public interface INameGenerator
    {
        /// <summary>
        /// Gets or sets the minimum length of the name.
        /// </summary>
        /// <value>The minimum length of the name.</value>
        int MinNameLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum length of the name.
        /// </summary>
        /// <value>The maximum length of the name.</value>
        int MaxNameLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum processing time.
        /// </summary>
        /// <value>The maximum processing time in milliseconds.</value>
        int MaxProcessingTime { get; set; }

        /// <summary>
        /// Gets or sets the excluded strings.
        /// </summary>
        /// <value>The excluded strings.</value>
        List<string> ExcludedStrings { get; set; }

        /// <summary>
        /// Gets the used words.
        /// </summary>
        /// <value>The used words.</value>
        List<string> UsedWords { get; }

        /// <summary>
        /// Generates a name.
        /// </summary>
        /// <returns>The name.</returns>
        string GenerateName();

        /// <summary>
        /// Reset the list of used names.
        /// </summary>
        void Reset();
    }
}
