using System.Collections.Generic;
using System.Drawing;

using Narivia.Models;
using Narivia.Repositories;

namespace Narivia.Controllers
{
    public class BiomeController
    {
        readonly RepositoryXml<Biome> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Controllers.BiomeController"/> class.
        /// </summary>
        /// <param name="repository">Repository.</param>
        public BiomeController(RepositoryXml<Biome> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Adds the biome.
        /// </summary>
        /// <returns>The biome.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <param name = "colour">Colour.</param>
        public void Create(string id, string name, string description, Color colour)
        {
            Biome biome = new Biome();

            biome.Id = id;
            biome.Name = name;
            biome.Description = description;
            biome.Colour = colour;

            repository.Add(biome);
        }

        /// <summary>
        /// Gets the biome by identifier.
        /// </summary>
        /// <returns>The biome by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public Biome Get(string id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Gets all biomes.
        /// </summary>
        /// <returns>The biomes.</returns>
        public List<Biome> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Modifies the name and description.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <param name="colour">Colour.</param>
        public void Modify(string id, string name, string description, Color colour)
        {
            Biome biome = Get(id);
            biome.Name = name;
            biome.Description = description;
            biome.Colour = colour;
        }

        /// <summary>
        /// Removes the biome.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Delete(string id)
        {
            repository.Remove(id);
        }
    }
}
