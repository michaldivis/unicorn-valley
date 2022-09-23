using Microsoft.EntityFrameworkCore;

namespace UnicornValley.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext db) : base(db)
    {
    }

    private string[]? _includeProperties;
    protected override string[] IncludeProperties => _includeProperties ??= Array.Empty<string>();

    public async Task<bool> IsUsernameUniqueAsync(Username username)
    {
        var alreadyExists = await _dbSet.AnyAsync(a => a.Username.Value == username.Value);
        return !alreadyExists;
    }
}
