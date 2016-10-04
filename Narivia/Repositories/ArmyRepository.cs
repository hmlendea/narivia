using System.Collections.Generic;
using System.Linq;

using Narivia.Models;

namespace Narivia.Repositories
{
    public class ArmyRepository : Repository<Army>
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
            return Entities.Find(A => (A.FactionId == factionId && A.UnitId == unitId));
        }

        /// <summary>
        /// Gets all borders of a specified region.
        /// </summary>
        /// <returns>The all by region.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public List<Army> GetAllByFaction(string factionId)
        {
            return Entities.FindAll(A => A.FactionId == factionId);
        }

        /// <summary>
        /// Check if the specified border exists.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        public bool Contains(string factionId, string unitId)
        {
            return Entities.Any(A => A.FactionId == factionId && A.UnitId == unitId);
        }

        public int GetFactionTroops(string factionId)
        {
            int troops = 0;

            foreach (Army army in Entities)
                if (army.FactionId == factionId)
                    troops += army.Size;

            return troops;
        }
    }
}
