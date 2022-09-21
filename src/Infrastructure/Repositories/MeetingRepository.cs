using UnicornValley.Domain.Entities;
using UnicornValley.Domain.Repositories;

namespace UnicornValley.Infrastructure.Repositories;
public class MeetingRepository : GenericRepository<Meeting>, IMeetingRepository
{
    public MeetingRepository(AppDbContext db) : base(db)
    {
    }

    private string[]? _includeProperties;
    protected override string[] IncludeProperties => _includeProperties ??= new[]
        {
            nameof(Meeting.Creator),
            nameof(Meeting.Invitations),
            nameof(Meeting.Attendees)
        };
}