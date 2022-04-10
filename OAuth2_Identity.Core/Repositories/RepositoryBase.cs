using System.Linq.Expressions;
using OAuth2_Identity.Core.Concrete;

namespace OAuth2_Identity.Core.Repositories;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{
    #region Variables

    public readonly CoreDBContext _context;

    #endregion

    #region Constructor

    public RepositoryBase(CoreDBContext context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    /// <summary>  
    /// Adds the specified entity.  
    /// </summary>  
    /// <param name="entity">The entity.</param>  
    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    /// <summary>  
    /// Adds the range.  
    /// </summary>  
    /// <param name="entities">The entities.</param>  
    public void AddRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
    }

    /// <summary>  
    /// Finds the specified predicate.  
    /// </summary>  
    /// <param name="predicate">The predicate.</param>  
    /// <returns></returns>  
    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().Where(predicate);
    }

    /// <summary>  
    /// Singles the or default.  
    /// </summary>  
    /// <param name="predicate">The predicate.</param>  
    /// <returns></returns>  
    public TEntity SingleOrDefault(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().Where(predicate).SingleOrDefault();
    }

    /// <summary>  
    /// First the or default.  
    /// </summary>  
    /// <returns></returns>  
    public TEntity FirstOrDefault()
    {
        return _context.Set<TEntity>().SingleOrDefault();
    }

    /// <summary>  
    /// Gets the specified identifier.  
    /// </summary>  
    /// <param name="id">The identifier.</param>  
    /// <returns></returns>  
    public TEntity Get(int id)
    {
        return _context.Set<TEntity>().Find(id);
    }

    /// <summary>  
    /// Gets the specified identifier async.  
    /// </summary>  
    /// <param name="id">The identifier.</param>  
    /// <returns></returns> 
    public async Task<TEntity> GetAsync(int id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    /// <summary>  
    /// Gets all.  
    /// </summary>  
    /// <returns></returns>  
    public IEnumerable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().ToList();
    }

    /// <summary>  
    /// Removes the specified entity.  
    /// </summary>  
    /// <param name="entity">The entity.</param>  
    public void Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    /// <summary>  
    /// Removes the range.  
    /// </summary>  
    /// <param name="entities">The entities.</param>  
    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
    }

    /// <summary>
    /// Updates the specified entity.  
    /// </summary>
    /// <param name="entity"></param>
    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    /// <summary>
    /// Updates the range.  
    /// </summary>
    /// <param name="entities"></param>
    public void UpdateRange(TEntity entities)
    {
        _context.Set<TEntity>().UpdateRange(entities);
    }

    #endregion
}