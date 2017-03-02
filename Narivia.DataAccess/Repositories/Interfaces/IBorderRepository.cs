using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Border repository interface.
    /// </summary>
    public interface IBorderRepository
    {
        /// <summary>
        /// Adds the specified border.
        /// </summary>
        /// <param name="border">Border.</param>
        void Add(Border border);

        /// <summary>
        /// Get the border with the specified region identifiers.
        /// </summary>
        /// <returns>The border.</returns>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        Border Get(string region1Id, string region2Id);

        /// <summary>
        /// Gets all the borders.
        /// </summary>
        /// <returns>The borders.</returns>
        IEnumerable<Border> GetAll();

        /// <summary>
        /// Removes the border with the specified region identifiers.
        /// </summary>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        void Remove(string region1Id, string region2Id);
    }
}
