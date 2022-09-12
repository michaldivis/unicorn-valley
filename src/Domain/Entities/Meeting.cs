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

    public Result<Invitation> SendInvitation(User user)
    {
        if(user.Id == Creator.Id)
        {
            return Result.Fail(DomainErrors.Meeting.InvitingCreator);
        }

        if(ScheduledAtUtc < DateTime.UtcNow)
        {
            return Result.Fail(DomainErrors.Meeting.AlreadyPassed);
        }

        var invitation = new Invitation(Guid.NewGuid(), user, this);

        _invitations.Add(invitation);

        return invitation;
    }
}