using System.Collections.Generic;
using System.Drawing;

using Narivia.Models;

using Region = Narivia.Models.Region;

namespace Narivia.Repositories
{
    public class RegionRepository : RepositoryXml<Region>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositorys.RegionRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public RegionRepository(string fileName)
            : base(fileName)
        {

        }
        
        public List<Region> GetAllByFaction(string factionId)
        {
            return Entities.FindAll(R => R.FactionId == factionId);
        }

        /// <summary>
        /// Adds the region.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <param name="color">Color.</param>
        public void Create(string id, string name, string description, System.Drawing.Color color)
        {
            Region region = new Region();

            region.Id = id;
            region.Name = name;
            region.Description = description;
            region.Colour = color;

            Add(region);
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
            Region region = Get(id);
            region.Name = name;
            region.Description = description;
            region.Colour = colour;
        }
    }
}
