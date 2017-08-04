using System;

namespace Narivia.DataAccess.Exceptions
{
    /// <summary>
    /// Duplicate entity exception.
    /// </summary>
    public class InvalidEntityFieldException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEntityFieldException"/> class.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        public InvalidEntityFieldException(string fieldName, string entityId, string entityType)
            : base($"The {fieldName} field of {entityId} {entityType} entity is invalid.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEntityException"/> class.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="innerException">Inner exception.</param>
        public InvalidEntityFieldException(string fieldName, string entityId, string entityType, Exception innerException)
            : base($"The {fieldName} field of {entityId} {entityType} entity is invalid.", innerException)
        {
        }
    }
}
