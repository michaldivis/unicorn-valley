namespace UnicornValley.Domain.Entities;

public class Meeting : EntityBase
{
    public User Creator { get; private set; }
    public MeetingType Type { get; private set; }
    public DateTime ScheduledAtUtc { get; private set; }
    public string Name { get; private set; }

    private readonly List<Invitation> _invitations = new();
    public IReadOnlyCollection<Invitation> Invitations => _invitations.AsReadOnly();

    public Meeting(Guid id, User creator, MeetingType type, DateTime scheduledAtUtc, string name) : base(id)
    {
        Creator = creator;
        Type = type;
        ScheduledAtUtc = scheduledAtUtc;
        Name = name;
    }
}