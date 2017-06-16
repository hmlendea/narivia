using System;
using System.Collections.Generic;

using Narivia.GameLogic.Generators.Interfaces;

namespace Narivia.GameLogic.Generators
{
    /// <summary>
    /// Random name generator that mixes words from different lists
    /// </summary>
    public class RandomMixerNameGenerator : INameGenerator
    {
        /// <summary>
        /// Gets or sets the minimum length of the name.
        /// </summary>
        /// <value>The minimum length of the name.</value>
        public int MinNameLength { get; set; }

        /// <summary>
        /// Gets or sets the excluded substrings.
        /// </summary>
        /// <value>The excluded substrings.</value>
        public List<string> ExcludedSubstrings { get; set; }

        /// <summary>
        /// Gets or sets the first input list.
        /// </summary>
        /// <value>The first input list.</value>
        public List<string> InputList1 { get; set; }

        /// <summary>
        /// Gets or sets the second input list.
        /// </summary>
        /// <value>The second input list.</value>
        public List<string> InputList2 { get; set; }

        /// <summary>
        /// Gets or sets the used words.
        /// </summary>
        /// <value>The used words.</value>
        public List<string> UsedWords { get; private set; }

        readonly Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomMixerNameGenerator"/> class.
        /// </summary>
        public RandomMixerNameGenerator()
        {
            random = new Random();
            MinNameLength = 5;

            ExcludedSubstrings = new List<string>();
            UsedWords = new List<string>();
        }

        /// <summary>
        /// Generates a name.
        /// </summary>
        /// <returns>The name.</returns>
        public string GenerateName()
        {
            string word = string.Empty;

            while (string.IsNullOrWhiteSpace(word) || UsedWords.Contains(word))
            {
                word = InputList1[random.Next(InputList1.Count)] +
                       InputList2[random.Next(InputList2.Count)];
            }

            return word;
        }

        /// <summary>
        /// Reset the list of used names.
        /// </summary>
        public void Reset()
        {
            UsedWords = new List<string>();
        }
    }
}
