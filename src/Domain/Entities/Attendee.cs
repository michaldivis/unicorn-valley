namespace UnicornValley.Domain.Entities;

public class Attendee : Entity
{
    public Guid UserId { get; private set; }
    public Guid MeetingId { get; private set; }
    public DateTime JoinedAtUtc { get; private set; }

    internal Attendee(Invitation invitation) : base(Guid.NewGuid())
    {
        UserId = invitation.UserId;
        MeetingId = invitation.MeetingId;
        JoinedAtUtc = DateTime.UtcNow;
    }
}