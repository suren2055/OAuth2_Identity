using System.Linq.Expressions;

namespace OAuth2_Identity.Core.Repositories;

public interface IRepositoryBase<TEntity> where TEntity : class
{
        /// <summary>  
        /// Gets the specified identifier.  
        /// </summary>  
        /// <param name="id">The identifier.</param>  
        /// <returns></returns>  
        TEntity Get(int id);

        /// <summary>  
        /// Gets the specified identifier async.  
        /// </summary>  
        /// <param name="id">The identifier.</param>  
        /// <returns></returns>
        Task<TEntity> GetAsync(int id);

        /// <summary>  
        /// Gets all.  
        /// </summary>  
        /// <returns></returns>  
        IEnumerable<TEntity> GetAll();

        /// <summary>  
        /// Finds the specified predicate.  
        /// </summary>  
        /// <param name="predicate">The predicate.</param>  
        /// <returns></returns>  
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>  
        /// Singles the or default.  
        /// </summary>  
        /// <param name="predicate">The predicate.</param>  
        /// <returns></returns>  
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>  
        /// First the or default.  
        /// </summary>  
        /// <returns></returns>  
        TEntity FirstOrDefault();

        /// <summary>  
        /// Adds the specified entity.  
        /// </summary>  
        /// <param name="entity">The entity.</param>  
        void Add(TEntity entity);

        /// <summary>  
        /// Adds the range.  
        /// </summary>  
        /// <param name="entities">The entities.</param>  
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>  
        /// Removes the specified entity.  
        /// </summary>  
        /// <param name="entity">The entity.</param>  
        void Remove(TEntity entity);

        /// <summary>  
        /// Removes the range.  
        /// </summary>  
        /// <param name="entities">The entities.</param>  
        void RemoveRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates the specified entity.  
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// Updates the range.  
        /// </summary>
        /// <param name="entities"></param>
        void UpdateRange(TEntity entities);
}