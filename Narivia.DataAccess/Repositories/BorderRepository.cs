using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Border repository implementation.
    /// </summary>
    public class BorderRepository : IBorderRepository
    {
        /// <summary>
        /// Gets or sets the borders.
        /// </summary>
        /// <value>The borders.</value>
        readonly Dictionary<string, Border> borders;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.BorderRepository"/> class.
        /// </summary>
        public BorderRepository()
        {
            borders = new Dictionary<string, Border>();
        }

        /// <summary>
        /// Adds the specified border.
        /// </summary>
        /// <param name="border">Border.</param>
        public void Add(Border border)
        {
            borders.Add(border.Id, border);
        }

        /// <summary>
        /// Get the border with the specified region identifiers.
        /// </summary>
        /// <returns>The border.</returns>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        public Border Get(string region1Id, string region2Id)
        {
            return borders[$"{region1Id}:{region2Id}"];
        }

        /// <summary>
        /// Gets all the borders.
        /// </summary>
        /// <returns>The borders.</returns>
        public IEnumerable<Border> GetAll()
        {
            return borders.Values;
        }

        /// <summary>
        /// Removes the border with the specified region identifiers.
        /// </summary>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        public void Remove(string region1Id, string region2Id)
        {
            borders.Remove($"{region1Id}:{region2Id}");
        }
    }
}
