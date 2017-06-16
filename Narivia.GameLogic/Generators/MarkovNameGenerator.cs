using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.GameLogic.Generators.Interfaces;
using Narivia.Infrastructure.Extensions;

namespace Narivia.GameLogic.Generators
{
    /// <summary>
    /// Name generator using Markov Chains.
    /// </summary>
    public class MarkovNameGenerator : INameGenerator
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
        /// Gets or sets the excluded strings.
        /// </summary>
        /// <value>The excluded strings.</value>
        public List<string> ExcludedStrings { get; set; }

        /// <summary>
        /// Gets or sets the used words.
        /// </summary>
        /// <value>The used words.</value>
        public List<string> UsedWords { get; private set; }

        const int ORDER = 3;

        readonly Random random;
        readonly Dictionary<string, List<char>> chains;

        List<string> inputWords;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkovNameGenerator"/> class.
        /// </summary>
        /// <param name="inputWords">Input words.</param>
        public MarkovNameGenerator(List<string> inputWords)
        {
            this.inputWords = inputWords;

            MinNameLength = 5;
            MaxNameLength = 10;

            ExcludedStrings = new List<string>();
            UsedWords = new List<string>();

            random = new Random();
            chains = new Dictionary<string, List<char>>();

            PopulateChains();
        }

        /// <summary>
        /// Gets a name.
        /// </summary>
        /// <returns>The name.</returns>
        public string GenerateName()
        {
            string name = string.Empty;

            while (!IsNameValid(name))
            {
                string word = string.Empty;

                while (string.IsNullOrWhiteSpace(word))
                {
                    word = inputWords[random.Next(inputWords.Count)];
                }

                name = word.Substring(random.Next(0, word.Length - ORDER), ORDER);

                while (name.Length < word.Length)
                {
                    string token = name.Substring(name.Length - ORDER, ORDER);
                    char letter = GetLetter(token);

                    if (letter == '?')
                    {
                        break;
                    }

                    name += letter;
                }

                name = name.Substring(0, 1) + name.Substring(1).ToLower();
                name = name.ToTitleCase();
            }

            UsedWords.Add(name);

            return name;
        }

        /// <summary>
        /// Reset the list of used names.
        /// </summary>
        public void Reset()
        {
            UsedWords.Clear();
        }

        void PopulateChains()
        {
            foreach (string word in inputWords)
            {
                for (int i = 0; i < word.Length - ORDER; i++)
                {
                    string token = word.Substring(i, ORDER);

                    if (!chains.ContainsKey(token))
                    {
                        chains[token] = new List<char>();
                    }

                    chains[token].Add(word[i + ORDER]);
                }
            }
        }

        char GetLetter(string token)
        {
            if (!chains.ContainsKey(token))
            {
                return '?';
            }

            List<char> letters = chains[token];
            int index = random.Next(letters.Count);

            return letters[index];
        }

        bool IsNameValid(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            if (name.Length < MinNameLength || name.Length > MaxNameLength)
            {
                return false;
            }

            if (UsedWords.Contains(name))
            {
                return false;
            }

            return !ExcludedStrings.Any(name.Contains);
        }
    }
}