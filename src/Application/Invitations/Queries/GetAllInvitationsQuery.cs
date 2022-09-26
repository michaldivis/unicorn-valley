namespace UnicornValley.Application.Invitations.Queries;

public record GetAllInvitationsQuery : IRequest<List<Invitation>>
{
}