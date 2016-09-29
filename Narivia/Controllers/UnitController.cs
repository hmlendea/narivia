using System.Collections.Generic;

using Narivia.Models;
using Narivia.Repositories;

namespace Narivia.Controllers
{
    public class UnitController
    {
        readonly RepositoryXml<Unit> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Controllers.UnitController"/> class.
        /// </summary>
        /// <param name="repository">Unit repository.</param>
        public UnitController(RepositoryXml<Unit> repository)
        {
            this.repository = repository;
        }


        /// <summary>
        /// Adds the unit.
        /// </summary>
        /// <returns>The unit.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <param name="type">Type.</param>
        /// <param name="price">Price.</param>
        /// <param name="maintenance">Maintenance.</param>
        /// <param name="power">Attack.</param>
        /// <param name="health">Health.</param>
        public void Create(string id, string name, string description, UnitType type,
                           int price, int maintenance, int power, int health)
        {
            Unit unit = new Unit();

            unit.Id = id;
            unit.Name = name;
            unit.Description = description;
            unit.Type = type;
            unit.Price = price;
            unit.Maintenance = maintenance;
            unit.Power = power;
            unit.Health = health;

            repository.Add(unit);
        }

        /// <summary>
        /// Gets the unit by identifier.
        /// </summary>
        /// <returns>The unit by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public Unit Get(string id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Gets all units.
        /// </summary>
        /// <returns>The units.</returns>
        public List<Unit> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Modifies the name, description, price, maintenance, power and health.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <param name="type">Type.</param>
        /// <param name="price">Price.</param>
        /// <param name="maintenance">Maintenance.</param>
        /// <param name="power">Power.</param>
        /// <param name="health">Health.</param>
        public void Modify(string id, string name, string description, UnitType type,
                           int price, int maintenance, int power, int health)
        {
            Unit unit = Get(id);
            unit.Name = name;
            unit.Description = description;
            unit.Price = price;
            unit.Maintenance = maintenance;
            unit.Power = power;
            unit.Health = health;
        }

        /// <summary>
        /// Removes the unit.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Delete(string id)
        {
            repository.Remove(id);
        }
    }
}
