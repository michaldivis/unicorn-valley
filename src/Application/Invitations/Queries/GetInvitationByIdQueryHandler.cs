namespace UnicornValley.Application.Invitations.Queries;

public class GetInvitationByIdQueryHandler : IRequestHandler<GetInvitationByIdQuery, Result<Invitation>>
{
    private readonly IReadOnlyInvitationRepository _readOnlyInvitationRepository;
    private readonly IResultHandler _resultHandler;

    public GetInvitationByIdQueryHandler(IReadOnlyInvitationRepository readOnlyInvitationRepository, IResultHandler resultHandler)
    {
        _readOnlyInvitationRepository = readOnlyInvitationRepository;
        _resultHandler = resultHandler;
    }

    public async Task<Result<Invitation>> Handle(GetInvitationByIdQuery request, CancellationToken cancellationToken)
    {
        var userResult = await _readOnlyInvitationRepository.FindByIdAsync(request.InvitationId, cancellationToken);
        await _resultHandler.HandleAsync(userResult, cancellationToken);

        if (userResult.IsFailed)
        {
            return userResult;
        }

        return userResult;
    }
}