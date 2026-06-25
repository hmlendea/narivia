using System;

using NuciDAL.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Border repository implementation.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BorderRepository"/> class.
    /// </remarks>
    public class BorderRepository(string fileName) : XmlRepository<BorderEntity>(fileName)
    {
        public override void Update(BorderEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
