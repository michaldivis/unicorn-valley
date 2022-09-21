namespace UnicornValley.Domain.Repositories;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<TEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(TEntity entity);
}
