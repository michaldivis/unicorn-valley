namespace UnicornValley.Infrastructure.Repositories;

public class ReadOnlyInvitationRepository : ReadOnlyGenericRepository<Invitation>, IReadOnlyInvitationRepository
{
    public ReadOnlyInvitationRepository(AppDbContext db) : base(db)
    {
    }
}
