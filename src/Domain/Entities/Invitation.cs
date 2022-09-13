namespace UnicornValley.Domain.Entities;

public class Invitation : Entity
{
    public Guid UserId { get; private set; }
    public Guid MeetingId { get; private set; }
    public InvitationStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? LastModifiedAtUtc { get; private set; }

    internal Invitation(Guid id, User user, Meeting meeting) : base(id)
    {
        UserId = user.Id;
        MeetingId = meeting.Id;
        Status = InvitationStatus.Pending;
        CreatedAtUtc = DateTime.UtcNow;
    }

    internal void Expire()
    {
        Status = InvitationStatus.Expired;
        LastModifiedAtUtc = DateTime.UtcNow;
    }

    internal Attendee Accept()
    {
        Status = InvitationStatus.Accepted;
        LastModifiedAtUtc = DateTime.UtcNow;

        var attendee = new Attendee(this);

        return attendee;
    }
}
