namespace UnicornValley.Domain.Repositories;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<Result<TEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(TEntity entity);
}
