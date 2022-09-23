namespace UnicornValley.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<bool> IsUsernameUniqueAsync(Username username);
}
