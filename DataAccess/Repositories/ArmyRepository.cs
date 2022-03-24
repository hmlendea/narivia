using System;

using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Army repository implementation.
    /// </summary>
    public class ArmyRepository : Repository<ArmyEntity>
    {
        public override void Update(ArmyEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
