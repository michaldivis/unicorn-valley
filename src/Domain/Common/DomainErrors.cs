namespace UnicornValley.Domain.Common;

public static class DomainErrors
{
    public static class Common
    {
        public static DomainError NotFoundById<TEntity>(Guid id) where TEntity : Entity
        {
            return new DomainError(
                "common-not-found-by-id",
                "Entity not found by ID",
                "{EntityName} with ID: {Id} not found",
                typeof(TEntity).Name, id)
            .WithMetadata("EntityType", typeof(TEntity).Name)
            .WithMetadata("UserId", id);
        }
    }

    public static class Meeting
    {
        public static DomainError InvitingCreator(Guid creatorId)
        {
            return new DomainError(
                "meeting-inviting-creator",
                "Can't send invitation to the meeting creator",
                "Can't send invitation to the meeting creator")
            .WithMetadata("CreatorId", creatorId);
        }

        public static DomainError AlreadyPassed(DateTime scheduledAtUtc)
        {
            return new DomainError(
                "meeting-already-passed",
                "Can't send invitation for a meeting in the past",
                "Can't send invitation for a meeting that has already taken place at {ScheduledAtUtc}",
                scheduledAtUtc)
            .WithMetadata("ScheduledAtUtc", scheduledAtUtc);
        }

        public static DomainError MaximumNumberOfAttendeesMissing()
        {
            return new DomainError(
                "meeting-maximum-number-of-attendees-missing",
                "Maximum number of attendees is missing",
                "Maximum number of attendees is missing");
        }

        public static DomainError InvitationValidBeforeInHoursMissing()
        {
            return new DomainError(
                "meeting-invitations-valid-before-in-hours-missing",
                "Invitation valid before in hours is missing",
                "Invitation valid before in hours is missing");
        }

        public static DomainError InvitationExpired(DateTime expiredAtUtc)
        {
            return new DomainError(
                "meeting-invitation-expired",
                "Invitation is expired",
                "Invitation has already expired at {ExpiredAtUtc}",
                expiredAtUtc)
            .WithMetadata("ExpiredAtUtc", expiredAtUtc);
        }

        public static DomainError InvitationAlreadyAccepted()
        {
            return new DomainError(
                "meeting-invitation-already-accepted",
                "Invitation is already accepted",
                "Invitation is already accepted");
        }

        public static DomainError InvitationAlreadyExists(Guid userId, Guid meetingId, Guid invitationId)
        {
            return new DomainError(
                "meeting-invitation-already-exists",
                "Invitation already exists",
                "Invitation already exists")
            .WithMetadata("UserId", userId)
            .WithMetadata("MeetingId", meetingId)
            .WithMetadata("InvitationId", invitationId);
        }
    }

    public static class User
    {
        public static DomainError UsernameAlreadyExists(string username)
        {
            return new DomainError(
                "user-username-already-exists",
                "A user with this username already exists",
                "A user with the username '{Username}' already exists",
                username)
            .WithMetadata("Username", username);
        }

        public static DomainError InvalidUsername(string username)
        {
            return new DomainError(
                "user-invalid-username",
                "Username not in a valid format",
                "Username '{Username}' is in a valid format",
                username)
            .WithMetadata("Username", username);
        }
    }
}
