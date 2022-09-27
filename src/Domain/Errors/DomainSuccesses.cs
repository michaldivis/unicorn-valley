namespace UnicornValley.Domain.Errors;

public static class DomainSuccesses
{
    public static class Common
    {
        public static DomainSuccess FoundById(Type entityType, Guid id) => new DomainSuccessBuilder()
            .WithCode("common/found-by-id")
            .WithTitle("Entity found by ID")
            .WithMessage("{0} with ID: {1} found", entityType.Name, id)
            .WithMetadata("EntityType", entityType.Name)
            .WithMetadata("Id", id)
            .Create();
    }

    public static class Meeting
    {
        public static DomainSuccess Created(Entities.Meeting meeting) => new DomainSuccessBuilder()
            .WithCode("meeting/created")
            .WithTitle("Meeting created")
            .WithMessage("Created meeting with Id: {0}", meeting.Id)
            .WithMetadata("Id", meeting.Id)
            .Create();

        public static DomainSuccess InvitationSent(Entities.Invitation invitation) => new DomainSuccessBuilder()
            .WithCode("meeting/invitation-sent")
            .WithTitle("Invitation sent")
            .WithMessage("Invitation sent, Id: {0}", invitation.Id)
            .WithMetadata("Id", invitation.Id)
            .Create();

        public static DomainSuccess InvitationAccepted(Entities.Attendee attendee) => new DomainSuccessBuilder()
            .WithCode("meeting/invitation-accepted")
            .WithTitle("Invitation accepted")
            .WithMessage("Invitation accepted, attendee Id: {0}", attendee.Id)
            .WithMetadata("AttendeeId", attendee.Id)
            .Create();
    }

    public static class User
    {
        public static DomainSuccess Created(Entities.User user) => new DomainSuccessBuilder()
            .WithCode("user/created")
            .WithTitle("User created")
            .WithMessage("Created user with Id: {0}", user.Id)
            .WithMetadata("Id", user.Id)
            .Create();
    }
}