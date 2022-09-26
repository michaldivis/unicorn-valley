using System.Runtime.CompilerServices;

namespace UnicornValley.Domain.Common;

public static class DomainErrors
{
    private static DomainError Create(string category, string code, string title, string message)
    {
        return new DomainError($"{category}.{code}", title, message);
    }

    public static class Common
    {
        public static DomainError NotFoundById<TEntity>(Guid id) where TEntity : Entity => 
            Create("Entity not found", $"{typeof(TEntity).Name} with ID: {id} not found")
            .WithMetadata("EntityType", typeof(TEntity).Name)
            .WithMetadata("UserId", id);

        private static DomainError Create(string title, string? message = null, [CallerMemberName] string code = "")
        {
            return DomainErrors.Create(nameof(Common), code, title, message ?? title);
        }
    }

	public static class Meeting
	{
		public static DomainError InvitingCreator(Guid creatorId) => 
            Create( "Can't send invitation to the meeting creator")
            .WithMetadata("CreatorId", creatorId);

        public static DomainError AlreadyPassed(DateTime scheduledAtUtc) => 
            Create("Can't send invitation for a meeting in the past", $"Can't send invitation for a meeting that has lready taken place at {scheduledAtUtc}")
            .WithMetadata("ScheduledAtUtc", scheduledAtUtc);

        public static DomainError MaximumNumberOfAttendeesMissing() => 
            Create("Maximum number of attendees is missing");

        public static DomainError InvitationValidBeforeInHoursMissing() => 
            Create("Invitation valid before in hours is missing");

        public static DomainError InvitationExpired(DateTime expiredAtUtc) => 
            Create("Invitation is expired", $"Invitation has already expired at {expiredAtUtc}")
            .WithMetadata("ExpiredAtUtc", expiredAtUtc);

        public static DomainError InvitationAlreadyAccepted() => 
            Create("Invitation is already accepted");

        public static DomainError InvitationAlreadyExists(Guid userId, Guid meetingId, Guid invitationId) =>
            Create("Invitation already exists")
            .WithMetadata("UserId", userId)
            .WithMetadata("MeetingId", meetingId)
            .WithMetadata("InvitationId", invitationId);

        private static DomainError Create(string title, string? message = null, [CallerMemberName] string code = "")
        {
            return DomainErrors.Create(nameof(Meeting), code, title, message ?? title);
        }
    }

    public static class User
    {
        public static DomainError UsernameAlreadyExists(string username) =>
            Create("A user with this username already exists", $"A user with the username \"{username}\" already exists")
            .WithMetadata("Username", username);

        public static DomainError InvalidUsername(string username) =>
            Create("Username not in a valid format", $"Username \"{username}\" is in a valid format")
            .WithMetadata("Username", username);

        private static DomainError Create(string title, string? message = null, [CallerMemberName] string code = "")
        {
            return DomainErrors.Create(nameof(User), code, title, message ?? title);
        }
    }
}
