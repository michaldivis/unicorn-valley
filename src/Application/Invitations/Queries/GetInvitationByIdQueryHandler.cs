namespace UnicornValley.Application.Invitations.Queries;

public class GetInvitationByIdQueryHandler : IRequestHandler<GetInvitationByIdQuery, Result<Invitation>>
{
    private readonly IReadOnlyInvitationRepository _readOnlyInvitationRepository;
    private readonly IErrorHandler _errorHandler;

    public GetInvitationByIdQueryHandler(IReadOnlyInvitationRepository readOnlyInvitationRepository, IErrorHandler errorHandler)
    {
        _readOnlyInvitationRepository = readOnlyInvitationRepository;
        _errorHandler = errorHandler;
    }

    public async Task<Result<Invitation>> Handle(GetInvitationByIdQuery request, CancellationToken cancellationToken)
    {
        var userResult = await _readOnlyInvitationRepository.FindByIdAsync(request.InvitationId);

        if (userResult.IsFailed)
        {
            await _errorHandler.HandleAsync(userResult, cancellationToken);
            return userResult;
        }

        return userResult;
    }
}