using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    public class ArmyRepository : Repository<Army>, IArmyRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.ArmyRepository"/> class.
        /// </summary>
        public ArmyRepository()
            : base()
        {
        }

        /// <summary>
        /// Gets an entity.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        public Army Get(string factionId, string unitId)
        {
            return DataStore[$"{factionId}:{unitId}"];
        }

        /// <summary>
        /// Gets all borders of a specified region.
        /// </summary>
        /// <returns>The all by region.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public List<Army> GetAllByFaction(string factionId)
        {
            return DataStore.Values.ToList().FindAll(A => A.FactionId == factionId);
        }

        /// <summary>
        /// Check if the specified border exists.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        public bool Contains(string factionId, string unitId)
        {
            return base.Contains($"{factionId}:{unitId}");
        }

        public int GetFactionTroops(string factionId)
        {
            int troops = 0;

            foreach (Army army in DataStore.Values)
                if (army.FactionId == factionId)
                    troops += army.Size;

            return troops;
        }
    }
}
