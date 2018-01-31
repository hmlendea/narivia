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
        /// <param name="provinceEntity">Province.</param>
        public override void Update(ProvinceEntity provinceEntity)
        {
            LoadEntitiesIfNeeded();

            ProvinceEntity provinceEntityToUpdate = Get(provinceEntity.Id);

            if (provinceEntityToUpdate == null)
            {
                throw new EntityNotFoundException(provinceEntity.Id, nameof(BorderEntity));
            }

            provinceEntityToUpdate.Name = provinceEntity.Name;
            provinceEntityToUpdate.Description = provinceEntity.Description;
            provinceEntityToUpdate.ColourHexadecimal = provinceEntity.ColourHexadecimal;
            provinceEntityToUpdate.Type = provinceEntity.Type;
            provinceEntityToUpdate.FactionId = provinceEntity.FactionId;
            provinceEntityToUpdate.SovereignFactionId = provinceEntity.SovereignFactionId;
            
            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
