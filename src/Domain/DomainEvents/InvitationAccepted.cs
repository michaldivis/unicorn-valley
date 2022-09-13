namespace UnicornValley.Domain.DomainEvents;
public record InvitationAccepted(Guid InvitationId, Guid MeetingId) : IDomainEvent;
