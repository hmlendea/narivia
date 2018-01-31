using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Army repository implementation.
    /// </summary>
    public class ArmyRepository : Repository<ArmyEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArmyRepository"/> class.
        /// </summary>
        public ArmyRepository() : base("placeholder")
        {

        }

        public override void Update(ArmyEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
