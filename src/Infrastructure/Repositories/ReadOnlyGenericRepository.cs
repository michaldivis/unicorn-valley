using Microsoft.EntityFrameworkCore;
using UnicornValley.Domain.Errors;

namespace UnicornValley.Infrastructure.Repositories;

public abstract class ReadOnlyGenericRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : Entity
{
    protected readonly DbSet<TEntity> _dbSet;

    public ReadOnlyGenericRepository(AppDbContext db)
    {
        _dbSet = db.Set<TEntity>();
    }

    public async Task<Result<TEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _dbSet
            .AsNoTracking()
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (result is null)
        {
            return Result.Fail(DomainErrors.Common.NotFoundById(typeof(TEntity), id));
        }

        return result;
    }

    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _dbSet
            .AsNoTracking()
            .OrderBy(a => a.Id)
            .ToListAsync();
    }
}