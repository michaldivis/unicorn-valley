namespace UnicornValley.Infrastructure.Repositories;

public class ReadOnlyAttendeeRepository : ReadOnlyGenericRepository<Attendee>, IReadOnlyAttendeeRepository
{
    public ReadOnlyAttendeeRepository(AppDbContext db) : base(db)
    {
    }
}