using System.Collections.Generic;

using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Border repository implementation.
    /// </summary>
    public class RelationRepository : Repository<RelationEntity>
    {
        /// <summary>
        /// Gets or sets the borders.
        /// </summary>
        /// <value>The borders.</value>
        readonly Dictionary<string, RelationEntity> relationEntitiesStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="BorderRepository"/> class.
        /// </summary>
        public RelationRepository() : base("placeholder")
        {

        }

        public override void Update(RelationEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
