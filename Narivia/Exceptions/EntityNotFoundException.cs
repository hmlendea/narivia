using System;
using System.Runtime.Serialization;

namespace Narivia.Exceptions
{
    /// <summary>
    /// Repository exception.
    /// </summary>
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.DuplicateEntityException"/> class.
        /// </summary>
        public EntityNotFoundException()
            : base("Entity could not be found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        public EntityNotFoundException(string entityId)
            : base($"{entityId} could not be found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="innerException">Inner exception.</param>
        public EntityNotFoundException(string entityId, Exception innerException)
            : base($"{entityId} could not be found.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="info">Info.</param>
        /// <param name="context">Context.</param>
        protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
