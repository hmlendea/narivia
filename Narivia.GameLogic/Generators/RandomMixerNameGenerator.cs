using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Gets or sets the maximum length of the name.
        /// </summary>
        /// <value>The maximum length of the name.</value>
        public int MaxNameLength { get; set; }

        /// <summary>
        /// Gets or sets the excluded substrings.
        /// </summary>
        /// <value>The excluded substrings.</value>
        public List<string> ExcludedSubstrings { get; set; }

        /// <summary>
        /// Gets or sets the used words.
        /// </summary>
        /// <value>The used words.</value>
        public List<string> UsedWords { get; private set; }

        readonly Random random;

        List<string> wordList1;
        List<string> wordList2;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomMixerNameGenerator"/> class.
        /// </summary>
        /// <param name="wordList1">Word list1.</param>
        /// <param name="wordList2">Word list2.</param>
        public RandomMixerNameGenerator(List<string> wordList1, List<string> wordList2)
        {
            random = new Random();

            MinNameLength = 5;
            MaxNameLength = 10;

            ExcludedSubstrings = new List<string>();
            UsedWords = new List<string>();

            this.wordList1 = wordList1;
            this.wordList2 = wordList2;
        }

        /// <summary>
        /// Generates a name.
        /// </summary>
        /// <returns>The name.</returns>
        public string GenerateName()
        {
            string word = string.Empty;

            while (string.IsNullOrWhiteSpace(word) ||
                   word.Length < MinNameLength ||
                   word.Length > MaxNameLength ||
                   UsedWords.Contains(word) ||
                   ExcludedSubstrings.Any(word.Contains))
            {
                word = wordList1[random.Next(wordList1.Count)] +
                       wordList2[random.Next(wordList2.Count)];
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
