using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Province repository implementation.
    /// </summary>
    public class ProvinceRepository : XmlRepository<ProvinceEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProvinceRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public ProvinceRepository(string fileName) : base(fileName)
        {

        }

        /// <summary>
        /// Updates the specified province.
        /// </summary>
        /// <param name="entity">Province.</param>
        public override void Update(ProvinceEntity entity)
        {
            LoadEntitiesIfNeeded();

            ProvinceEntity provinceEntityToUpdate = Get(entity.Id);

            if (provinceEntityToUpdate == null)
            {
                throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            }

            provinceEntityToUpdate.Name = entity.Name;
            provinceEntityToUpdate.Description = entity.Description;
            provinceEntityToUpdate.ColourHexadecimal = entity.ColourHexadecimal;
            provinceEntityToUpdate.Type = entity.Type;
            provinceEntityToUpdate.FactionId = entity.FactionId;
            provinceEntityToUpdate.SovereignFactionId = entity.SovereignFactionId;

            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
