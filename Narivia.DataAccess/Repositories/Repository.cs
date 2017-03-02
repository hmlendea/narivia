using System.Collections.Generic;
using System.Linq;

using Narivia.Infrastructure.Exceptions;
using Narivia.Models;
using Narivia.Infrastructure.Extensions;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Memory Repository.
    /// </summary>
    public class Repository<T> where T : EntityBase
    {
        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>The entities.</value>
        protected Dictionary<string, T> DataStore { get; set; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public int Size
        {
            get
            {
                if (DataStore.IsNullOrEmpty())
                    return 0;

                return DataStore.Count;
            }
        }

        /// <summary>
        /// Indicates wether the repository is empty.
        /// </summary>
        /// <value>True if the repository is empty, false otherwise.</value>
        public bool Empty
        {
            get { return DataStore.IsNullOrEmpty(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.Repository`1"/> class.
        /// </summary>
        public Repository()
        {
            DataStore = new Dictionary<string, T>();
        }

        /// <summary>
        /// Add the specified entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public virtual void Add(T entity)
        {
            if (Contains(entity))
                throw new DuplicateEntityException(entity.Id);

            DataStore.Add(entity.Id, entity);
        }

        /// <summary>
        /// Get the entity with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public virtual T Get(string id)
        {
            if (!Contains(id))
                throw new EntityNotFoundException(id);

            return DataStore[id];
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>The all.</returns>
        public List<T> GetAll()
        {
            return DataStore.Values.ToList();
        }

        /// <summary>
        /// Remove the specified entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public virtual void Remove(T entity)
        {
            if (!Contains(entity))
                throw new EntityNotFoundException(entity.Id);

            DataStore.Remove(entity.Id);
        }

        /// <summary>
        /// Remove the entity with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public virtual void Remove(string id)
        {
            if (!Contains(id))
                throw new EntityNotFoundException(id);

            DataStore.Remove(id);
        }

        /// <summary>
        /// Empties the data store.
        /// </summary>
        public void Clear()
        {
            DataStore.Clear();
        }

        /// <summary>
        /// Checks wether the specified entity exists.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public bool Contains(T entity)
        {
            return DataStore.ContainsValue(entity);
        }

        /// <summary>
        /// Checks wether an entity with the specified identifier exists.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public bool Contains(string id)
        {
            return DataStore.ContainsKey(id);
        }
    }
}
