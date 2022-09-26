namespace UnicornValley.Application.Invitations.Commands;
public record AcceptInvitationCommand : IRequest<Result<Attendee>>
{
    public Guid InvitationId { get; init; }
}
