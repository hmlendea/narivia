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
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; set; }

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
        /// Gets or sets the used words.
        /// </summary>
        /// <value>The used words.</value>
        public List<string> UsedWords { get; private set; }

        readonly Random random = new Random();
        readonly Dictionary<string, List<char>> chains = new Dictionary<string, List<char>>();

        List<string> InputWords = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkovNameGenerator"/> class.
        /// </summary>
        /// <param name="input">Input names.</param>
        /// <param name="order">Order.</param>
        /// <param name="minNameLength">Minimum name length.</param>
        public MarkovNameGenerator(IEnumerable<string> input, int order, int minNameLength)
        {
            if (order < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(order));
            }

            if (minNameLength < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(minNameLength));
            }

            Order = order;
            MinNameLength = minNameLength;
            ExcludedSubstrings = new List<string>();

            InputWords = input.ToList();

            foreach (string word in InputWords)
            {
                for (int i = 0; i < word.Length - order; i++)
                {
                    string token = word.Substring(i, order);

                    if (!chains.ContainsKey(token))
                    {
                        chains[token] = new List<char>();
                    }

                    chains[token].Add(word[i + order]);
                }
            }
        }

        /// <summary>
        /// Gets a name.
        /// </summary>
        /// <returns>The name.</returns>
        public string GenerateName()
        {
            string name = string.Empty;

            do
            {
                string word = string.Empty;

                while (string.IsNullOrWhiteSpace(word))
                {
                    word = InputWords[random.Next(InputWords.Count)];
                }

                name = word.Substring(random.Next(0, word.Length - Order), Order);

                while (name.Length < word.Length)
                {
                    string token = name.Substring(name.Length - Order, Order);
                    char letter = GetLetter(token);

                    if (letter == '?')
                    {
                        break;
                    }

                    name += letter;
                }

                name = name.Substring(0, 1) + name.Substring(1).ToLower();
            }
            while (name.Length < MinNameLength);

            name = name.ToTitleCase();

            if (UsedWords.Contains(name) || ExcludedSubstrings.Any(name.Contains))
            {
                return GenerateName();
            }

            UsedWords.Add(name);

            return name;
        }

        /// <summary>
        /// Reset this instance.
        /// </summary>
        public void Reset()
        {
            UsedWords.Clear();
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
    }
}