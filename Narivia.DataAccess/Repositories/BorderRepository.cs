using System.Collections.Generic;
using System.Linq;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories
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
            return DataStore[$"{region1Id}:{region2Id}"];
        }
 
        /// <summary>
        /// Gets all borders of a specified region.
        /// </summary>
        /// <returns>The all by region.</returns>
        /// <param name="regionId">Region identifier.</param>
        public List<Border> GetAllByRegion(string regionId)
        {
            return DataStore.Values.ToList().FindAll(B => B.Region1Id == regionId);
        }

        /// <summary>
        /// Check if the specified border exists.
        /// </summary>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        public bool Contains(string region1Id, string region2Id)
        {
            return base.Contains($"{region1Id}:{region2Id}");
        }
    }
}
