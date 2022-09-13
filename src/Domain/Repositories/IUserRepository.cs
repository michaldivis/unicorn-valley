namespace UnicornValley.Domain.Repositories;

public interface IUserRepository
{
    Task<Result<User>> FindByIdAsync(Guid id, CancellationToken cancellationToken);
}
