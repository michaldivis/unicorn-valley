namespace UnicornValley.Infrastructure.Repositories;

public class ReadOnlyUserRepository : ReadOnlyGenericRepository<User>, IReadOnlyUserRepository
{
    public ReadOnlyUserRepository(AppDbContext db) : base(db)
    {
    }
}
