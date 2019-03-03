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
        /// <param name="entity">Faction.</param>
        public override void Update(FactionEntity entity)
        {
            LoadEntitiesIfNeeded();

            FactionEntity factionEntityToUpdate = Get(entity.Id);

            if (factionEntityToUpdate == null)
            {
                throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            }

            factionEntityToUpdate.Name = entity.Name;
            factionEntityToUpdate.Description = entity.Description;
            factionEntityToUpdate.ColourHexadecimal = entity.ColourHexadecimal;
            factionEntityToUpdate.FlagId = entity.FlagId;
            factionEntityToUpdate.CultureId = entity.CultureId;

            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
