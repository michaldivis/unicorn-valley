namespace UnicornValley.Application.Invitations.Accept;
public record AcceptInvitationCommand : IRequest<Result<Attendee>>
{
    public Guid InvitationId { get; init; }
}
