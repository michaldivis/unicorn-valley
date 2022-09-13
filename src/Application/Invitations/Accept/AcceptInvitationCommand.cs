namespace UnicornValley.Application.Invitations.Accept;
public record AcceptInvitationCommand(Guid InvitationId) : IRequest;
