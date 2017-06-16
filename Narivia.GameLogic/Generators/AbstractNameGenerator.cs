using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.GameLogic.Generators.Interfaces;

namespace Narivia.GameLogic.Generators
{
    public abstract class AbstractNameGenerator : INameGenerator
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
        /// Gets or sets the maximum processing time.
        /// </summary>
        /// <value>The maximum processing time in milliseconds.</value>
        public int MaxProcessingTime { get; set; }

        /// <summary>
        /// Gets or sets the excluded strings.
        /// </summary>
        /// <value>The excluded strings.</value>
        public List<string> ExcludedStrings { get; set; }

        /// <summary>
        /// Gets or sets the included strings.
        /// </summary>
        /// <value>The included strings.</value>
        public List<string> IncludedStrings { get; set; }

        /// <summary>
        /// Gets or sets the string that all generated names must start with.
        /// </summary>
        /// <value>The string start filter.</value>
        public string StartsWithFilter { get; set; }

        /// <summary>
        /// Gets or sets the string that all generated names must end with.
        /// </summary>
        /// <value>The string end filter.</value>
        public string EndsWithFilter { get; set; }

        /// <summary>
        /// Gets or sets the used words.
        /// </summary>
        /// <value>The used words.</value>
        public List<string> UsedWords { get; protected set; }

        protected readonly Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractNameGenerator"/> class.
        /// </summary>
        protected AbstractNameGenerator()
        {
            MinNameLength = 5;
            MaxNameLength = 10;
            MaxProcessingTime = 1000;

            StartsWithFilter = string.Empty;
            EndsWithFilter = string.Empty;

            ExcludedStrings = new List<string>();
            IncludedStrings = new List<string>();
            UsedWords = new List<string>();

            random = new Random();
        }

        /// <summary>
        /// Gets a name.
        /// </summary>
        /// <returns>The name.</returns>
        public abstract string GenerateName();

        /// <summary>
        /// Generates names.
        /// </summary>
        /// <returns>The names.</returns>
        /// <param name="maximumCount">Maximum count.</param>
        public List<string> GenerateNames(int maximumCount)
        {
            List<string> names = new List<string>();
            DateTime startTime = DateTime.Now;

            while (DateTime.Now < startTime.AddMilliseconds(MaxProcessingTime) &&
                   names.Count < maximumCount)
            {
                string name = GenerateName();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    names.Add(name);
                }
            }

            return names;
        }

        /// <summary>
        /// Reset the list of used names.
        /// </summary>
        public void Reset()
        {
            UsedWords.Clear();
        }

        /// <summary>
        /// Checks wether the the name is valid.
        /// </summary>
        /// <returns><c>true</c>, if name is valid, <c>false</c> otherwise.</returns>
        /// <param name="name">Name.</param>
        protected bool IsNameValid(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            if (name.Length < MinNameLength || name.Length > MaxNameLength)
            {
                return false;
            }

            if (!name.StartsWith(StartsWithFilter, StringComparison.InvariantCulture) ||
                !name.EndsWith(EndsWithFilter, StringComparison.InvariantCulture))
            {
                return false;
            }

            if (UsedWords.Contains(name))
            {
                return false;
            }

            if (ExcludedStrings.Any(name.Contains) ||
                !IncludedStrings.All(name.Contains))
            {
                return false;
            }

            return true;
        }
    }
}
