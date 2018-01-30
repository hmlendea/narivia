using System;

using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Border repository implementation.
    /// </summary>
    public class BorderRepository : XmlRepository<BorderEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BorderRepository"/> class.
        /// </summary>
        public BorderRepository(string fileName) : base(fileName)
        {

        }

        public override void Update(BorderEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
