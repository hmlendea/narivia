using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    public class UnitRepository : RepositoryXml<Unit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositorys.UnitRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public UnitRepository(string fileName)
            : base(fileName)
        {

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

            base.Add(unit);
        }

        /// <summary>
        /// Modifies the unit.
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
    }
}
