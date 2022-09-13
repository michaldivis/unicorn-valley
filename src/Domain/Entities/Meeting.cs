namespace UnicornValley.Domain.Entities;

public class Meeting : AggregateRoot
{
    private readonly List<Attendee> _attendees = new();
    private readonly List<Invitation> _invitations = new();

    public User Creator { get; private set; }
    public MeetingType Type { get; private set; }
    public DateTime ScheduledAtUtc { get; private set; }
    public string Name { get; private set; }
    public string Location { get; private set; }

    public int? MaximumNumberOfAttendees { get; private set; }
    public DateTime? InvitationsExpireAtUtc { get; private set; }
    public int NumberOfAttendees { get; private set; }    

    public IReadOnlyCollection<Attendee> Attendees => _attendees.AsReadOnly();
    public IReadOnlyCollection<Invitation> Invitations => _invitations.AsReadOnly();

    private Meeting(Guid id, User creator, MeetingType type, DateTime scheduledAtUtc, string name, string location) : base(id)
    {
        Creator = creator;
        Type = type;
        ScheduledAtUtc = scheduledAtUtc;
        Name = name;
        Location = location;
    }    

    public static Result<Meeting> Create(Guid id, User creator, MeetingType type, DateTime scheduledAtUtc, string name, string location, int? maximumNumberOfAttendees, int? invitationValidBeforeInHours)
    {
        var meeting = new Meeting(id, creator, type, scheduledAtUtc, name, location);

        switch (type)
        {
            case MeetingType.WithLimitedNumberOfAttendees:
                if(maximumNumberOfAttendees is null)
                {
                    return Result.Fail(DomainErrors.Meeting.MaximumNumberOfAttendeesMissing);
                }
                meeting.MaximumNumberOfAttendees = maximumNumberOfAttendees;
                break;
            case MeetingType.WithExpirationForInvitations:
                if (invitationValidBeforeInHours is null)
                {
                    return Result.Fail(DomainErrors.Meeting.InvitationValidBeforeInHoursMissing);
                }
                meeting.InvitationsExpireAtUtc = scheduledAtUtc.AddHours(-invitationValidBeforeInHours.Value);
                break;
            default:
                throw ExhaustiveMatch.Failed(type);
        }

        return meeting;
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

    public Result<Attendee> AcceptInvitation(Invitation invitation)
    {
        var isExpired = Type switch
        {
            MeetingType.WithLimitedNumberOfAttendees => NumberOfAttendees >= MaximumNumberOfAttendees,
            MeetingType.WithExpirationForInvitations => InvitationsExpireAtUtc < DateTime.UtcNow,
            _ => throw ExhaustiveMatch.Failed(Type)
        };

        if (isExpired)
        {
            invitation.Expire();
            return Result.Fail(DomainErrors.Meeting.InvitationExpired);
        }

        var attendee = invitation.Accept();

        _attendees.Add(attendee);
        NumberOfAttendees++;

        RaiseDomainEvent(new InvitationAccepted(invitation.Id, Id));

        return attendee;
    }
}