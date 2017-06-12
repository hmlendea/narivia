using System;

namespace Narivia.Infrastructure.Exceptions
{
    /// <summary>
    /// Repository exception.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEntityException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        public EntityNotFoundException(string entityId, string entityType)
            : base($"The {entityId} {entityType} entity can not be found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="innerException">Inner exception.</param>
        public EntityNotFoundException(string entityId, string entityType, Exception innerException)
            : base($"The {entityId} {entityType} entity can not be found.", innerException)
        {
        }
    }
}
