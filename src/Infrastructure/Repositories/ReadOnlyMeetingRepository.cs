namespace UnicornValley.Infrastructure.Repositories;

public class ReadOnlyMeetingRepository : ReadOnlyGenericRepository<Meeting>, IReadOnlyMeetingRepository
{
    public ReadOnlyMeetingRepository(AppDbContext db) : base(db)
    {
    }
}
