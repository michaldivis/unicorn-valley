namespace UnicornValley.Application.Invitations.Send;
public record SendInvitationCommand(Guid UserId, Guid MeetingId) : IRequest;
