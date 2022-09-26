namespace UnicornValley.Domain.Repositories;

public interface IInvitationRepository : IRepository<Invitation>
{
    Task<Invitation?> GetForUserAndMeeting(Guid userId, Guid meetingId);
}

