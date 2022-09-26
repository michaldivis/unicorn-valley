namespace UnicornValley.Domain.Errors;

public static class DomainErrors
{
    public static class Common
    {
        public static DomainError NotFoundById(Type entityType, Guid id) => new DomainErrorBuilder()
            .WithCode("common/not-found-by-id")
            .WithTitle("Entity not found by ID")
            .WithMessage("{0} with ID: {1} not found", entityType.Name, id)
            .WithMetadata("EntityType", entityType.Name)
            .WithMetadata("Id", id)
            .Create();
    }

    public static class Meeting
    {
        public static DomainError InvitingCreator(Guid creatorId) => new DomainErrorBuilder()
            .WithCode("meeting/inviting-creator")
            .WithTitle("Can't send invitation to the meeting creator")
            .WithMessage("Can't send invitation to the meeting creator")
            .WithMetadata("CreatorId", creatorId)
            .Create();

        public static DomainError AlreadyPassed(DateTime scheduledAtUtc) => new DomainErrorBuilder()
            .WithCode("meeting/already-passed")
            .WithTitle("Can't send invitation for a meeting in the past")
            .WithMessage("Can't send invitation for a meeting that has already taken place at {0}", scheduledAtUtc)
            .WithMetadata("ScheduledAtUtc", scheduledAtUtc)
            .Create();

        public static DomainError MaximumNumberOfAttendeesMissing() => new DomainErrorBuilder()
            .WithCode("meeting/maximum-number-of-attendees-missing")
            .WithTitle("Maximum number of attendees is missing")
            .WithMessage("Maximum number of attendees is missing")
            .Create();

        public static DomainError InvitationValidBeforeInHoursMissing() => new DomainErrorBuilder()
            .WithCode("meeting/invitations-valid-before-in-hours-missing")
            .WithTitle("Invitation valid before in hours is missing")
            .WithMessage("Invitation valid before in hours is missing")
            .Create();

        public static DomainError InvitationExpired(DateTime expiredAtUtc) => new DomainErrorBuilder()
            .WithCode("meeting/invitation-expired")
            .WithTitle("Invitation is expired")
            .WithMessage("Invitation has already expired at {0}", expiredAtUtc)
            .WithMetadata("ExpiredAtUtc", expiredAtUtc)
            .Create();

        public static DomainError InvitationAlreadyAccepted() => new DomainErrorBuilder()
            .WithCode("meeting/invitation-already-accepted")
            .WithTitle("Invitation is already accepted")
            .WithMessage("Invitation is already accepted")
            .Create();

        public static DomainError InvitationAlreadyExists(Guid userId, Guid meetingId, Guid invitationId) => new DomainErrorBuilder()
            .WithCode("meeting/invitation-already-exists")
            .WithTitle("Invitation already exists")
            .WithMessage("Invitation already exists")
            .WithMetadata("UserId", userId)
            .WithMetadata("MeetingId", meetingId)
            .WithMetadata("InvitationId", invitationId)
            .Create();
    }

    public static class User
    {
        public static DomainError UsernameAlreadyExists(string username) => new DomainErrorBuilder()
            .WithCode("user/username-already-exists")
            .WithTitle("A user with this username already exists")
            .WithMessage("A user with the username '{0}' already exists", username)
            .WithMetadata("Username", username)
            .Create();

        public static DomainError InvalidUsername(string username) => new DomainErrorBuilder()
            .WithCode("user/invalid-username")
            .WithTitle("Username not in a valid format")
            .WithMessage("Username '{0}' is in a valid format", username)
            .WithMetadata("Username", username)
            .Create();
    }
}
