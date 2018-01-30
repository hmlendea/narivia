using System.Linq;

using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Faction repository implementation.
    /// </summary>
    public class FactionRepository : XmlRepository<FactionEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FactionRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public FactionRepository(string fileName) : base(fileName)
        {

        }
        
        /// <summary>
        /// Updates the specified faction.
        /// </summary>
        /// <param name="factionEntity">Faction.</param>
        public override void Update(FactionEntity factionEntity)
        {
            LoadEntitiesIfNeeded();

            FactionEntity factionEntityToUpdate = Entities.FirstOrDefault(x => x.Id == factionEntity.Id);

            if (factionEntityToUpdate == null)
            {
                throw new EntityNotFoundException(factionEntity.Id, nameof(BorderEntity));
            }

            factionEntityToUpdate.Name = factionEntity.Name;
            factionEntityToUpdate.Description = factionEntity.Description;
            factionEntityToUpdate.ColourHexadecimal = factionEntity.ColourHexadecimal;
            factionEntityToUpdate.FlagId = factionEntity.FlagId;
            factionEntityToUpdate.CultureId = factionEntity.CultureId;

            XmlFile.SaveEntities(Entities);
        }
    }
}
