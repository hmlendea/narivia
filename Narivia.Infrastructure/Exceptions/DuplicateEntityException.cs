using System;

namespace Narivia.Infrastructure.Exceptions
{
    /// <summary>
    /// Duplicate entity exception.
    /// </summary>
    public class DuplicateEntityException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Infrastructure.Exceptions.DuplicateEntityException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        public DuplicateEntityException(string entityId, string entityType)
            : base($"The {entityId} {entityType} entity is duplicated.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Infrastructure.Exceptions.DuplicateEntityException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="innerException">Inner exception.</param>
        public DuplicateEntityException(string entityId, string entityType, Exception innerException)
            : base($"The {entityId} {entityType} entity is duplicated.", innerException)
        {
        }
    }
}
