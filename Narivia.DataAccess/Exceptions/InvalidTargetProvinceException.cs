using System;

namespace Narivia.DataAccess.Exceptions
{
    /// <summary>
    /// Invalid target province exception.
    /// </summary>
    public class InvalidTargetProvinceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTargetProvinceException"/> class.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        public InvalidTargetProvinceException(string provinceId)
            : base($"The targeted province, {provinceId}, is invalid.")
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
