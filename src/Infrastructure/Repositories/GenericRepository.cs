using Microsoft.EntityFrameworkCore;

namespace UnicornValley.Infrastructure.Repositories;

public abstract class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(AppDbContext db)
    {
        _dbSet = db.Set<TEntity>();
    }

    protected abstract string[] IncludeProperties { get; }

    public async Task<Result<TEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(a => a.Id == id);

        foreach (var includeProperty in IncludeProperties)
        {
            query = query.Include(includeProperty);
        }

        var result = await query.FirstOrDefaultAsync(cancellationToken);

        if(result is null)
        {
            return Result.Fail(DomainErrors.Common.NotFoundById);
        }

        return result;
    }

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }
}