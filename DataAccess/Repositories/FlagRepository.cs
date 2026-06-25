
using NuciDAL.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Flag repository implementation.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="FlagRepository"/> class.
    /// </remarks>
    /// <param name="fileName">File name.</param>
    public class FlagRepository(string fileName) : XmlRepository<FlagEntity>(fileName)
    {

        /// <summary>
        /// Updates the specified flag.
        /// </summary>
        /// <param name="entity">Flag.</param>
        public override void Update(FlagEntity entity)
        {
            LoadEntities();

            FlagEntity flagEntityToUpdate = Get(entity.Id) ?? throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            flagEntityToUpdate.Layer1 = entity.Layer1;
            flagEntityToUpdate.Layer2 = entity.Layer2;
            flagEntityToUpdate.Emblem = entity.Emblem;
            flagEntityToUpdate.Skin = entity.Skin;
            flagEntityToUpdate.BackgroundColourHexadecimal = entity.BackgroundColourHexadecimal;
            flagEntityToUpdate.Layer1ColourHexadecimal = entity.Layer1ColourHexadecimal;
            flagEntityToUpdate.Layer2ColourHexadecimal = entity.Layer2ColourHexadecimal;
            flagEntityToUpdate.EmblemColourHexadecimal = entity.EmblemColourHexadecimal;

            SaveChanges();
        }
    }
}
