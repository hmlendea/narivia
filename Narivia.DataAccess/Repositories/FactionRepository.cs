using System.Drawing;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    public class FactionRepository : RepositoryXml<Faction>, IFactionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositorys.FactionRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public FactionRepository(string fileName)
            : base(fileName)
        {

        }

        /// <summary>
        /// Adds the faction.
        /// </summary>
        /// <returns>The faction.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <param name="colour">Colour.</param>
        public void Create(string id, string name, string description, Color colour)
        {
            Faction faction = new Faction();

            faction.Id = id;
            faction.Name = name;
            faction.Description = description;
            faction.Colour = colour;

            Add(faction);
        }

        /// <summary>
        /// Modifies the name, description, color, abilityId, cultureId and religionId.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <param name="colour">Colour.</param>
        public void Modify(string id, string name, string description, Color colour)
        {
            Faction faction = Get(id);
            faction.Name = name;
            faction.Description = description;
            faction.Colour = colour;
        }
    }
}
