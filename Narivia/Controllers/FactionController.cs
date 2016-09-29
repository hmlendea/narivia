using System.Collections.Generic;
using System.Drawing;

using Narivia.Models;
using Narivia.Repositories;

namespace Narivia.Controllers
{
    public class FactionController
    {
        readonly RepositoryXml<Faction> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Controllers.FactionController"/> class.
        /// </summary>
        /// <param name="repository">Faction repository.</param>
        public FactionController(RepositoryXml<Faction> repository)
        {
            this.repository = repository;
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

            repository.Add(faction);
        }

        /// <summary>
        /// Gets the faction by identifier.
        /// </summary>
        /// <returns>The faction by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public Faction Get(string id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Gets all factions.
        /// </summary>
        /// <returns>The factions.</returns>
        public List<Faction> GetAll()
        {
            return repository.GetAll();
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

        /// <summary>
        /// Removes the faction.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Delete(string id)
        {
            repository.Remove(id);
        }
    }
}
