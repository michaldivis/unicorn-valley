using Microsoft.EntityFrameworkCore;

namespace UnicornValley.Infrastructure.Repositories;

public class InvitationRepository : GenericRepository<Invitation>, IInvitationRepository
{
    public InvitationRepository(AppDbContext db) : base(db)
    {
    }

    private string[]? _includeProperties;
    protected override string[] IncludeProperties => _includeProperties ??= Array.Empty<string>();

    public async Task<Invitation?> GetForUserAndMeeting(Guid userId, Guid meetingId)
    {
        return await _dbSet
            .IncludeAll()
            .Where(a => a.UserId == userId && a.MeetingId == meetingId)
            .FirstOrDefaultAsync();
    }
}
