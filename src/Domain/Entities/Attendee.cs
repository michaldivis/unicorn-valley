namespace UnicornValley.Domain.Entities;

public class Attendee : Entity
{
    public override Guid Id { get; protected set; }
    public Guid UserId { get; private set; }
    public Guid MeetingId { get; private set; }
    public DateTime JoinedAtUtc { get; private set; }

    internal Attendee(Invitation invitation)
    {
        Id = Guid.NewGuid();
        UserId = invitation.UserId;
        MeetingId = invitation.MeetingId;
        JoinedAtUtc = DateTime.UtcNow;
    }

    [Obsolete("To be used by EF Core only")]
    internal Attendee()
    {
    }
}