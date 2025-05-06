using Investo.DataAccess.EF;
using Investo.DataAccess.Entities;
using Investo.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Investo.DataAccess.Repositories;

public abstract class AbstractRepository<TEntity, T> : ICrudRepository<TEntity, T>
    where TEntity : AbstractEntity<T>
{
    protected DbSet<TEntity> dbSet;
    protected readonly ApplicationDbContext context;

    protected AbstractRepository(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.context = context;
        this.dbSet = context.Set<TEntity>();
    }

    public virtual async Task<T> CreateAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var entry = await this.dbSet.AddAsync(entity);
        await this.context.SaveChangesAsync();
        return entry.Entity.Id;
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        this.dbSet.Remove(entity);
        await this.context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await this.dbSet.FindAsync(id);
        if (entity is not null)
        {
            this.dbSet.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await this.dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(int page, int pageSize)
    {
        var entities = await this.dbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return entities;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = await this.dbSet.Where(predicate).ToListAsync();
        return entities;
    }

    public virtual async Task<TEntity?> GetByIdAsync(T id)
    {
        var entity = await this.dbSet.FindAsync(id);
        return entity;
    }

    public async Task UpdateAsync(TEntity entity)
    {
         ArgumentNullException.ThrowIfNull(entity);

        this.dbSet.Update(entity);
        await this.context.SaveChangesAsync();
    }
}