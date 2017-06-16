using System;
using System.Collections.Generic;

namespace Narivia.GameLogic.Generators
{
    /// <summary>
    /// Random name generator that mixes words from different lists
    /// </summary>
    public class RandomMixerNameGenerator : AbstractNameGenerator
    {
        readonly int wordListsCount;

        readonly List<string> wordList1;
        readonly List<string> wordList2;
        readonly List<string> wordList3;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomMixerNameGenerator"/> class.
        /// </summary>
        /// <param name="wordList1">Word list 1.</param>
        /// <param name="wordList2">Word list 2.</param>
        /// <param name="wordList3">Word list 3.</param>
        public RandomMixerNameGenerator(List<string> wordList1, List<string> wordList2, List<string> wordList3)
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
        {
            wordListsCount = 2;

            this.wordList1 = wordList1;
            this.wordList2 = wordList2;
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
                name = GetRandomName();
            }

            return name;
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
