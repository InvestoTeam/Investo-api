namespace Investo.DataAccess.Interfaces;

using System.Linq.Expressions;
using Investo.DataAccess.Entities;

public interface ICrudRepository<TEntity, T>
    where TEntity : AbstractEntity<T>
{
    Task<T> CreateAsync(TEntity entity);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> GetAllAsync(int page, int pageSize);

    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity?> GetByIdAsync(T id);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

    Task DeleteAsync(int id);
}