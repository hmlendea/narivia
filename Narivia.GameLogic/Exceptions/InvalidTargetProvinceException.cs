using System;

using Narivia.Models;

namespace Narivia.GameLogic.Exceptions
{
    /// <summary>
    /// Invalid target province exception.
    /// </summary>
    public class InvalidTargetProvinceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTargetProvinceException"/> class.
        /// </summary>
        /// <param name="province">The province.</param>
        public InvalidTargetProvinceException(Province province)
            : this(province.Id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTargetProvinceException"/> class.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        public InvalidTargetProvinceException(string provinceId)
            : this(provinceId, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTargetProvinceException"/> class.
        /// </summary>
        /// <param name="province">The province.</param>
        /// /// <param name="innerException">Inner exception.</param>
        public InvalidTargetProvinceException(Province province, Exception innerException)
            : this(province.Id, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTargetProvinceException"/> class.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        /// <param name="innerException">Inner exception.</param>
        public InvalidTargetProvinceException(string provinceId, Exception innerException)
            : base($"The targeted province, {provinceId}, is invalid.", innerException)
        {
        }
    }
}
