namespace UnicornValley.Application.Invitations.Queries;

public class GetAllInvitationsQueryHandler : IRequestHandler<GetAllInvitationsQuery, List<Invitation>>
{
    private readonly IReadOnlyInvitationRepository _readOnlyInvitationRepository;

    public GetAllInvitationsQueryHandler(IReadOnlyInvitationRepository readOnlyUserRepository)
    {
        _readOnlyInvitationRepository = readOnlyUserRepository;
    }

    public async Task<List<Invitation>> Handle(GetAllInvitationsQuery request, CancellationToken cancellationToken)
    {
        var meetings = await _readOnlyInvitationRepository.GetAllAsync(cancellationToken);
        return meetings;
    }
}
