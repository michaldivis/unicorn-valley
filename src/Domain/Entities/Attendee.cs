namespace UnicornValley.Domain.Entities;

public class Attendee : EntityBase
{
    public Guid UserId { get; private set; }
    public Guid MeetingId { get; private set; }
    public DateTime JoinedAtUtc { get; private set; }

    public Attendee(Guid id, Guid userId, Guid meetingId, DateTime joinedAtUtc) : base(id)
    {
        UserId = userId;
        MeetingId = meetingId;
        JoinedAtUtc = joinedAtUtc;
    }
}