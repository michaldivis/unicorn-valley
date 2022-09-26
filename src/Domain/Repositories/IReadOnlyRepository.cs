namespace UnicornValley.Domain.Repositories;

public interface IReadOnlyRepository<TEntity> where TEntity : Entity
{
    Task<Result<TEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
}
