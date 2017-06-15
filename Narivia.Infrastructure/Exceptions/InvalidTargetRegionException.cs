using System;

namespace Narivia.Infrastructure.Exceptions
{
    /// <summary>
    /// Invalid target region exception.
    /// </summary>
    public class InvalidTargetRegionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTargetRegionException"/> class.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        public InvalidTargetRegionException(string regionId)
            : base($"The targeted region, {regionId}, is invalid.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTargetRegionException"/> class.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="innerException">Inner exception.</param>
        public InvalidTargetRegionException(string regionId, Exception innerException)
            : base($"The targeted region, {regionId}, is invalid.", innerException)
        {
        }
    }
}
