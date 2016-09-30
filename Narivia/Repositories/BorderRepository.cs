using System.Collections.Generic;
using System.Linq;

using Narivia.Models;

namespace Narivia.Repositories
{
    public class BorderRepository : Repository<Border>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.BorderRepository"/> class.
        /// </summary>
        public BorderRepository()
            : base()
        {
        }

        /// <summary>
        /// Gets an entity.
        /// </summary>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        public Border Get(string region1Id, string region2Id)
        {
            return Entities.Find(B =>
                (B.Region1Id == region1Id && B.Region2Id == region2Id) ||
                (B.Region1Id == region2Id && B.Region2Id == region1Id));
        }
 
        /// <summary>
        /// Gets all borders of a specified region.
        /// </summary>
        /// <returns>The all by region.</returns>
        /// <param name="regionId">Region identifier.</param>
        public List<Border> GetAllByRegion(string regionId)
        {
            return Entities.FindAll(B => B.Region1Id == regionId || B.Region2Id == regionId);
        }

        /// <summary>
        /// Check if the specified border exists.
        /// </summary>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        public bool Contains(string region1Id, string region2Id)
        {
            return Entities.Any(B =>
                (B.Region1Id == region1Id && B.Region2Id == region2Id) ||
                (B.Region1Id == region2Id && B.Region2Id == region1Id));
        }
    }
}
