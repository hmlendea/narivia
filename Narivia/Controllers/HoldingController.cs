using System.Collections.Generic;

using Narivia.Models;
using Narivia.Repositories;

namespace Narivia.Controllers
{
    public class HoldingController
    {
        readonly RepositoryXml<Holding> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Controllers.HoldingController"/> class.
        /// </summary>
        /// <param name="repository">Holding repository.</param>
        public HoldingController(RepositoryXml<Holding> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Adds the holding.
        /// </summary>
        /// <returns>The holding.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        public void Create(string id, string name, string description)
        {
            Holding holding = new Holding();

            holding.Id = id;
            holding.Name = name;
            holding.Description = description;

            repository.Add(holding);
        }

        /// <summary>
        /// Gets the holding by identifier.
        /// </summary>
        /// <returns>The holding by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public Holding Get(string id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Gets all holdings.
        /// </summary>
        /// <returns>The holdings.</returns>
        public List<Holding> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Modifies the name, descrption, income, recruits and religiousInfluence.
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

        /// <summary>
        /// Removes the holding.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Delete(string id)
        {
            repository.Remove(id);
        }
    }
}
