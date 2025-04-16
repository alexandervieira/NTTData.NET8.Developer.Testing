using Microsoft.EntityFrameworkCore;

public abstract class BaseRepository<TEntity> where TEntity : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected BaseRepository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        var query = await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        if (query == null)
        {
            throw new KeyNotFoundException($"Entity of type {typeof(TEntity).Name} with ID {id} not found.");
        }
        return query;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual TEntity Update(TEntity entity)
    {
        DetachIfTracked(entity);

        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;

        // Trata owned types (EF Core 8+)
        foreach (var navigation in _context.Entry(entity).Navigations)
        {
            var targetType = navigation.Metadata.TargetEntityType;

            if (targetType != null && targetType.IsOwned())
            {
                if (navigation.CurrentValue is not null)
                {
                    _context.Entry(navigation.CurrentValue).State = EntityState.Unchanged;
                }
            }
        }

        return entity;
    }

    public virtual void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    protected void DetachIfTracked(TEntity entity)
    {
        var entry = _context.ChangeTracker.Entries<TEntity>()
            .FirstOrDefault(e => e.Entity != null && e.Entity.Equals(entity));

        if (entry != null)
        {
            entry.State = EntityState.Detached;
        }
    }
}
