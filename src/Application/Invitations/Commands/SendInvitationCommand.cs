namespace UnicornValley.Application.Invitations.Commands;
public sealed record SendInvitationCommand : IRequest<Result<Invitation>>
{
    public Guid UserId { get; init; }
    public Guid MeetingId { get; init; }
}