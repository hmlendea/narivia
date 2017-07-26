using System;
using System.Collections.Generic;

using Narivia.Infrastructure.Extensions;

namespace Narivia.GameLogic.Generators
{
    /// <summary>
    /// Name generator using Markov Chains.
    /// </summary>
    public class MarkovNameGenerator : NameGenerator
    {
        const int ORDER = 3;

        readonly Dictionary<string, List<char>> chains;

        List<string> inputWords;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkovNameGenerator"/> class.
        /// </summary>
        /// <param name="inputWords">Input words.</param>
        public MarkovNameGenerator(List<string> inputWords)
        {
            this.inputWords = inputWords;

            chains = new Dictionary<string, List<char>>();

            PopulateChains();
        }

        /// <summary>
        /// Generates a name.
        /// </summary>
        /// <returns>The name.</returns>
        public override string GenerateName()
        {
            string name = string.Empty;

            DateTime startTime = DateTime.Now;

            while (DateTime.Now < startTime.AddMilliseconds(MaxProcessingTime) &&
                   !IsNameValid(name))
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
    }
}