namespace UnicornValley.Domain.Entities;

public class Invitation : EntityBase
{
    public Guid UserId { get; private set; }
    public Guid MeetingId { get; private set; }
    public InvitationStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    internal Invitation(Guid id, User user, Meeting meeting) : base(id)
    {
        UserId = user.Id;
        MeetingId = meeting.Id;
        Status = InvitationStatus.Pending;
        CreatedAtUtc = DateTime.UtcNow;
    }
}
