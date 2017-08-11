using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;

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
        /// <param name="borderEntity">Border.</param>
        void Add(BorderEntity borderEntity);

        /// <summary>
        /// Get the border with the specified province identifiers.
        /// </summary>
        /// <returns>The border.</returns>
        /// <param name="province1Id">First province identifier.</param>
        /// <param name="province2Id">Second province identifier.</param>
        BorderEntity Get(string province1Id, string province2Id);

        /// <summary>
        /// Gets all the borders.
        /// </summary>
        /// <returns>The borders.</returns>
        IEnumerable<BorderEntity> GetAll();

        /// <summary>
        /// Removes the border with the specified province identifiers.
        /// </summary>
        /// <param name="province1Id">First province identifier.</param>
        /// <param name="province2Id">Second province identifier.</param>
        void Remove(string province1Id, string province2Id);
    }
}
