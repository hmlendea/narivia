using System;

namespace Narivia.GameLogic.Generators.Interfaces
{
    public interface INameGenerator
    {
        /// <summary>
        /// Generates a name.
        /// </summary>
        /// <returns>The name.</returns>
        string GenerateName();

        /// <summary>
        /// Reset the list of used names.
        /// </summary>
        void Reset();
    }
}
