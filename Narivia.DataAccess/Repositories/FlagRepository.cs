using System.Linq;

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
        /// <param name="flagEntity">Flag.</param>
        public override void Update(FlagEntity flagEntity)
        {
            LoadEntitiesIfNeeded();

            FlagEntity flagEntityToUpdate = Entities.FirstOrDefault(x => x.Id == flagEntity.Id);

            if (flagEntityToUpdate == null)
            {
                throw new EntityNotFoundException(flagEntity.Id, nameof(BorderEntity));
            }

            flagEntityToUpdate.Layer1 = flagEntity.Layer1;
            flagEntityToUpdate.Layer2 = flagEntity.Layer2;
            flagEntityToUpdate.Emblem = flagEntity.Emblem;
            flagEntityToUpdate.Skin = flagEntity.Skin;
            flagEntityToUpdate.BackgroundColourHexadecimal = flagEntity.BackgroundColourHexadecimal;
            flagEntityToUpdate.Layer1ColourHexadecimal = flagEntity.Layer1ColourHexadecimal;
            flagEntityToUpdate.Layer2ColourHexadecimal = flagEntity.Layer2ColourHexadecimal;
            flagEntityToUpdate.EmblemColourHexadecimal = flagEntity.EmblemColourHexadecimal;

            XmlFile.SaveEntities(Entities);
        }
    }
}
