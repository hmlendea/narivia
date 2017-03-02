using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    public class HoldingRepository : RepositoryXml<Holding>, IHoldingRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.HoldingRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public HoldingRepository(string fileName)
            : base(fileName)
        {

        }

        /// <summary>
        /// Adds the holding.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        public void Create(string id, string name, string description)
        {
            Holding holding = new Holding();

            holding.Id = id;
            holding.Name = name;
            holding.Description = description;

            Add(holding);
        }

        /// <summary>
        /// Modifies the holding.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="descrption">Descrption.</param>
        public void Modify(string id, string name, string descrption)
        {
            Holding holding = Get(id);
            holding.Name = name;
            holding.Description = descrption;
        }
    }
}
