using System;
using System.Collections.Generic;

using Narivia.GameLogic.Generators.Interfaces;

namespace Narivia.GameLogic.Generators
{
    /// <summary>
    /// Random name generator that mixes words from different lists
    /// </summary>
    public class RandomMixerGenerator : INameGenerator
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

        List<string> used = new List<string>();
        readonly Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomMixerGenerator"/> class.
        /// </summary>
        public RandomMixerGenerator()
        {
            random = new Random();
            ExcludedSubstrings = new List<string>();
            MinNameLength = 4;

            used = new List<string>();
        }

        /// <summary>
        /// Generates a name.
        /// </summary>
        /// <returns>The name.</returns>
        public string GenerateName()
        {
            string word = string.Empty;

            while (string.IsNullOrWhiteSpace(word) || used.Contains(word))
            {
                word = InputList1[random.Next(InputList1.Count)] +
                       InputList2[random.Next(InputList2.Count)];
            }

            return word;
        }

        /// <summary>
        /// Reset the list of used words.
        /// </summary>
        public void Reset()
        {
            used = new List<string>();
        }
    }
}
