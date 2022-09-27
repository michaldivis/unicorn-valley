using System.Text.Json.Serialization;
using UnicornValley.Domain.Errors;

namespace UnicornValley.Domain.Entities;

public class Meeting : AggregateRoot
{
    private readonly List<Attendee> _attendees = new();
    private readonly List<Invitation> _invitations = new();

    public override Guid Id { get; protected set; }
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

    private Meeting(Guid id, User creator, MeetingType type, DateTime scheduledAtUtc, string name, string location)
    {
        Id = id;
        Creator = creator;
        Type = type;
        ScheduledAtUtc = scheduledAtUtc;
        Name = name;
        Location = location;
    }

    [Obsolete("To be used by EF Core only")]
    public Meeting()
    {
    }

    public static Result<Meeting> Create(Guid id, User creator, MeetingType type, DateTime scheduledAtUtc, string name, string location, int? maximumNumberOfAttendees, int? invitationValidBeforeInHours)
    {
        var meeting = new Meeting(id, creator, type, scheduledAtUtc, name, location);

        var initializationResult = type switch
        {
            MeetingType.WithLimitedNumberOfAttendees => InitializeWithLimitedNumberOfAttendees(meeting, maximumNumberOfAttendees),
            MeetingType.WithExpirationForInvitations => InitializeWithExpirationForInvitations(meeting, scheduledAtUtc, invitationValidBeforeInHours),
            _ => throw ExhaustiveMatch.Failed(type)
        };

        if (initializationResult.IsFailed)
        {
            return initializationResult;
        }

        return meeting;
    }

    private static Result InitializeWithLimitedNumberOfAttendees(Meeting meeting, int? maximumNumberOfAttendees)
    {
        if (maximumNumberOfAttendees is null)
        {
            return Result.Fail(DomainErrors.Meeting.MaximumNumberOfAttendeesMissing());
        }

        meeting.MaximumNumberOfAttendees = maximumNumberOfAttendees;

        return Result.Ok();
    }

    private static Result InitializeWithExpirationForInvitations(Meeting meeting, DateTime scheduledAtUtc, int? invitationValidBeforeInHours)
    {
        if (invitationValidBeforeInHours is null)
        {
            return Result.Fail(DomainErrors.Meeting.InvitationValidBeforeInHoursMissing());
        }

        meeting.InvitationsExpireAtUtc = scheduledAtUtc.AddHours(-invitationValidBeforeInHours.Value);

        return Result.Ok();
    }

    public Result<Invitation> SendInvitation(User user, Invitation? existingInvitation)
    {
        if(user.Id == Creator.Id)
        {
            return Result.Fail(DomainErrors.Meeting.InvitingCreator(Creator.Id));
        }

        if(ScheduledAtUtc < DateTime.UtcNow)
        {
            return Result.Fail(DomainErrors.Meeting.AlreadyPassed(ScheduledAtUtc));
        }

        if (existingInvitation is not null)
        {
            var canInviteAgain = existingInvitation.Status switch
            {
                InvitationStatus.Pending => false,
                InvitationStatus.Expired => true,
                InvitationStatus.Accepted => false,
                _ => throw ExhaustiveMatch.Failed(existingInvitation.Status)
            };

            if (!canInviteAgain)
            {
                return Result.Fail(DomainErrors.Meeting.InvitationAlreadyExists(user.Id, this.Id, existingInvitation.Id));
            }
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
            return Result.Fail(DomainErrors.Meeting.InvitationExpired(InvitationsExpireAtUtc!.Value));
        }

        if(invitation.Status == InvitationStatus.Accepted)
        {
            return Result.Fail(DomainErrors.Meeting.InvitationAlreadyAccepted());
        }

        var attendee = invitation.Accept();

        _attendees.Add(attendee);
        NumberOfAttendees++;

        RaiseDomainEvent(new InvitationAccepted(invitation.Id, Id));

        return attendee;
    }
}