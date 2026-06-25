using NuciDAL.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Province repository implementation.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ProvinceRepository"/> class.
    /// </remarks>
    /// <param name="fileName">File name.</param>
    public class ProvinceRepository(string fileName) : XmlRepository<ProvinceEntity>(fileName)
    {

        /// <summary>
        /// Updates the specified province.
        /// </summary>
        /// <param name="entity">Province.</param>
        public override void Update(ProvinceEntity entity)
        {
            LoadEntities();

            ProvinceEntity provinceEntityToUpdate = Get(entity.Id) ?? throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            provinceEntityToUpdate.Name = entity.Name;
            provinceEntityToUpdate.Description = entity.Description;
            provinceEntityToUpdate.ColourHexadecimal = entity.ColourHexadecimal;
            provinceEntityToUpdate.Type = entity.Type;
            provinceEntityToUpdate.FactionId = entity.FactionId;
            provinceEntityToUpdate.SovereignFactionId = entity.SovereignFactionId;

            SaveChanges();
        }
    }
}
