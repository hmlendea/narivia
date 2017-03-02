using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    public class ArmyRepository : IArmyRepository
    {
        /// <summary>
        /// Gets or sets the armies.
        /// </summary>
        /// <value>The armies.</value>
        readonly Dictionary<string, Army> armies;

        public ArmyRepository()
        {
            armies = new Dictionary<string, Army>();
        }

        public void Add(Army army)
        {
            armies.Add(army.Id, army);
        }

        public Army Get(string factionId, string unitId)
        {
            return armies[$"{factionId}:{unitId}"];
        }

        public IEnumerable<Army> GetAll()
        {
            return armies.Values;
        }

        public void Remove(string factionId, string unitId)
        {
            armies.Remove($"{factionId}:{unitId}");
        }
    }
}
