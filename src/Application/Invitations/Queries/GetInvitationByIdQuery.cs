namespace UnicornValley.Application.Invitations.Queries;
public record GetInvitationByIdQuery : IRequest<Result<Invitation>>
{
    public Guid InvitationId { get; init; }
}