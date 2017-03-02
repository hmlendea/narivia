using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    public class BorderRepository : IBorderRepository
    {
        /// <summary>
        /// Gets or sets the borders.
        /// </summary>
        /// <value>The borders.</value>
        readonly Dictionary<string, Border> borders;

        public BorderRepository()
        {
            borders = new Dictionary<string, Border>();
        }

        public void Add(Border border)
        {
            borders.Add(border.Id, border);
        }

        public Border Get(string region1Id, string region2Id)
        {
            return borders[$"{region1Id}:{region2Id}"];
        }

        public IEnumerable<Border> GetAll()
        {
            return borders.Values;
        }

        public void Remove(string region1Id, string region2Id)
        {
            borders.Remove($"{region1Id}:{region2Id}");
        }
    }
}
