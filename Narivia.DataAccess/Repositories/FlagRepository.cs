
using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Flag repository implementation.
    /// </summary>
    public class FlagRepository : XmlRepository<FlagEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlagRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public FlagRepository(string fileName) : base(fileName)
        {

        }

        /// <summary>
        /// Updates the specified flag.
        /// </summary>
        /// <param name="entity">Flag.</param>
        public override void Update(FlagEntity entity)
        {
            LoadEntitiesIfNeeded();

            FlagEntity flagEntityToUpdate = Get(entity.Id);

            if (flagEntityToUpdate == null)
            {
                throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            }

            flagEntityToUpdate.Layer1 = entity.Layer1;
            flagEntityToUpdate.Layer2 = entity.Layer2;
            flagEntityToUpdate.Emblem = entity.Emblem;
            flagEntityToUpdate.Skin = entity.Skin;
            flagEntityToUpdate.BackgroundColourHexadecimal = entity.BackgroundColourHexadecimal;
            flagEntityToUpdate.Layer1ColourHexadecimal = entity.Layer1ColourHexadecimal;
            flagEntityToUpdate.Layer2ColourHexadecimal = entity.Layer2ColourHexadecimal;
            flagEntityToUpdate.EmblemColourHexadecimal = entity.EmblemColourHexadecimal;

            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
