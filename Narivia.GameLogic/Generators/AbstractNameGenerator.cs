using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.GameLogic.Generators.Interfaces;
using Narivia.Infrastructure.Extensions;

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

            ExcludedStrings = new List<string>();
            UsedWords = new List<string>();

            random = new Random();
        }

        /// <summary>
        /// Gets a name.
        /// </summary>
        /// <returns>The name.</returns>
        public virtual string GenerateName()
        {
            throw new NotImplementedException();
        }

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

            if (UsedWords.Contains(name))
            {
                return false;
            }

            return !ExcludedStrings.Any(name.Contains);
        }
    }
}
