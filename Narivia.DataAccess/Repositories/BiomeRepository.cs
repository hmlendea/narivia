using System.Drawing;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    public class BiomeRepository : RepositoryXml<Biome>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.BiomeRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public BiomeRepository(string fileName)
            : base(fileName)
        {

        }

        /// <summary>
        /// Adds the biome.
        /// </summary>
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

            Add(biome);
        }

        /// <summary>
        /// Modifies the biome.
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
    }
}
