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

        int wordListsCount;

        List<string> wordList1;
        List<string> wordList2;
        List<string> wordList3;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomMixerNameGenerator"/> class.
        /// </summary>
        RandomMixerNameGenerator()
        {
            random = new Random();

            MinNameLength = 5;
            MaxNameLength = 10;

            ExcludedSubstrings = new List<string>();
            UsedWords = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomMixerNameGenerator"/> class.
        /// </summary>
        /// <param name="wordList1">Word list 1.</param>
        /// <param name="wordList2">Word list 2.</param>
        /// <param name="wordList3">Word list 3.</param>
        public RandomMixerNameGenerator(List<string> wordList1, List<string> wordList2, List<string> wordList3)
            : this()
        {
            wordListsCount = 3;

            this.wordList1 = wordList1;
            this.wordList2 = wordList2;
            this.wordList3 = wordList3;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomMixerNameGenerator"/> class.
        /// </summary>
        /// <param name="wordList1">Word list1.</param>
        /// <param name="wordList2">Word list2.</param>
        public RandomMixerNameGenerator(List<string> wordList1, List<string> wordList2)
            : this()
        {
            wordListsCount = 2;

            this.wordList1 = wordList1;
            this.wordList2 = wordList2;
        }

        /// <summary>
        /// Generates a name.
        /// </summary>
        /// <returns>The name.</returns>
        public string GenerateName()
        {
            string name = string.Empty;

            while (string.IsNullOrWhiteSpace(name) ||
                   name.Length < MinNameLength ||
                   name.Length > MaxNameLength ||
                   UsedWords.Contains(name) ||
                   ExcludedSubstrings.Any(name.Contains))
            {
                name = GetRandomName();
            }

            return name;
        }

        /// <summary>
        /// Reset the list of used names.
        /// </summary>
        public void Reset()
        {
            UsedWords = new List<string>();
        }

        string GetRandomName()
        {
            string name = string.Empty;

            switch (wordListsCount)
            {
                case 2:
                    name = wordList1[random.Next(wordList1.Count)] +
                           wordList2[random.Next(wordList2.Count)];
                    break;

                case 3:
                    name = wordList1[random.Next(wordList1.Count)] +
                           wordList2[random.Next(wordList2.Count)] +
                           wordList3[random.Next(wordList3.Count)];
                    break;
            }

            return name;
        }
    }
}
