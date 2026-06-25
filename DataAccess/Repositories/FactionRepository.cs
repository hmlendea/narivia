using NuciDAL.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Faction repository implementation.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="FactionRepository"/> class.
    /// </remarks>
    /// <param name="fileName">File name.</param>
    public class FactionRepository(string fileName) : XmlRepository<FactionEntity>(fileName)
    {

        /// <summary>
        /// Updates the specified faction.
        /// </summary>
        /// <param name="entity">Faction.</param>
        public override void Update(FactionEntity entity)
        {
            LoadEntities();

            FactionEntity factionEntityToUpdate = Get(entity.Id) ?? throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            factionEntityToUpdate.Name = entity.Name;
            factionEntityToUpdate.Description = entity.Description;
            factionEntityToUpdate.ColourHexadecimal = entity.ColourHexadecimal;
            factionEntityToUpdate.FlagId = entity.FlagId;
            factionEntityToUpdate.CultureId = entity.CultureId;

            SaveChanges();
        }
    }
}
