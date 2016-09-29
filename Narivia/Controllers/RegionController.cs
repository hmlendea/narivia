using System.Collections.Generic;
using System.Drawing;

using Region = Narivia.Models.Region;
using Narivia.Repositories;

namespace Narivia.Controllers
{
    public class RegionController
    {
        readonly RepositoryXml<Region> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Controllers.RegionController"/> class.
        /// </summary>
        /// <param name="repository">Region repository.</param>
        public RegionController(RepositoryXml<Region> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Adds the region.
        /// </summary>
        /// <returns>The region.</returns>
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

            repository.Add(region);
        }

        /// <summary>
        /// Gets the region by identifier.
        /// </summary>
        /// <returns>The region by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public Region Get(string id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Gets all regions.
        /// </summary>
        /// <returns>The regions.</returns>
        public List<Region> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Modifies the name, description, color, factionId, resourceId and biomeId.
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

        /// <summary>
        /// Removes the region.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Delete(string id)
        {
            repository.Remove(id);
        }
    }
}
