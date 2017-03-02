using System;
using System.Runtime.Serialization;

namespace Narivia.Infrastructure.Exceptions
{
    /// <summary>
    /// Duplicate entity exception.
    /// </summary>
    [Serializable]
    public class DuplicateEntityException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.DuplicateEntityException"/> class.
        /// </summary>
        public DuplicateEntityException()
            : base("Entity already exists.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.DuplicateEntityException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        public DuplicateEntityException(string entityId)
            : base($"{entityId} already exists.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.DuplicateEntityException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="innerException">Inner exception.</param>
        public DuplicateEntityException(string entityId, Exception innerException)
            : base($"{entityId} already exists.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.DuplicateEntityException"/> class.
        /// </summary>
        /// <param name="info">Info.</param>
        /// <param name="context">Context.</param>
        protected DuplicateEntityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
