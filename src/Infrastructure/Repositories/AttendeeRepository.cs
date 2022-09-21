using UnicornValley.Domain.Entities;
using UnicornValley.Domain.Repositories;

namespace UnicornValley.Infrastructure.Repositories;

public class AttendeeRepository : GenericRepository<Attendee>, IAttendeeRepository
{
    public AttendeeRepository(AppDbContext db) : base(db)
    {
    }

    private string[]? _includeProperties;
    protected override string[] IncludeProperties => _includeProperties ??= Array.Empty<string>();
}