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
        /// Gets or sets the excluded substrings.
        /// </summary>
        /// <value>The excluded substrings.</value>
        List<string> ExcludedSubstrings { get; set; }

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
