using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Army repository implementation.
    /// </summary>
    public class ArmyRepository : IArmyRepository
    {
        /// <summary>
        /// Gets or sets the armies.
        /// </summary>
        /// <value>The armies.</value>
        readonly Dictionary<string, Army> armies;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.ArmyRepository"/> class.
        /// </summary>
        public ArmyRepository()
        {
            armies = new Dictionary<string, Army>();
        }

        /// <summary>
        /// Adds the specified army.
        /// </summary>
        /// <param name="army">Army.</param>
        public void Add(Army army)
        {
            armies.Add(army.Id, army);
        }

        /// <summary>
        /// Gets the army with the specified faction and unit identifiers.
        /// </summary>
        /// <returns>The army.</returns>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        public Army Get(string factionId, string unitId)
        {
            return armies[$"{factionId}:{unitId}"];
        }

        /// <summary>
        /// Gets all the armies.
        /// </summary>
        /// <returns>The armies.</returns>
        public IEnumerable<Army> GetAll()
        {
            return armies.Values;
        }

        /// <summary>
        /// Removes the army with the specified faction and unit identifiers.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        public void Remove(string factionId, string unitId)
        {
            armies.Remove($"{factionId}:{unitId}");
        }
    }
}
