using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.Infrastructure.Extensions;

namespace Narivia.GameLogic.Generators
{
    /// <summary>
    /// Name generator using Markov Chains.
    /// </summary>
    public class NameGenerator
    {
        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; private set; }

        /// <summary>
        /// Gets the minimum name length.
        /// </summary>
        /// <value>The minimum length of the generated name.</value>
        public int MinNameLength { get; private set; }

        readonly Random random = new Random();
        readonly Dictionary<string, List<char>> chains = new Dictionary<string, List<char>>();

        List<string> inputWords = new List<string>();
        List<string> used = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NameGenerator"/> class.
        /// </summary>
        /// <param name="input">Input names.</param>
        /// <param name="order">Order.</param>
        /// <param name="minNameLength">Minimum name length.</param>
        public NameGenerator(IEnumerable<string> input, int order, int minNameLength)
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

            inputWords = input.ToList();

            foreach (string word in inputWords)
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
        public string GetName()
        {
            string name = string.Empty;

            do
            {
                string word = inputWords[random.Next(inputWords.Count)];

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
            while (used.Contains(name) || name.Length < MinNameLength);

            used.Add(name);

            return name.ToTitleCase();
        }

        /// <summary>
        /// Reset this instance.
        /// </summary>
        public void Reset()
        {
            used.Clear();
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