namespace UnicornValley.Application.Users.Queries;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<User>>
{
    private readonly IReadOnlyUserRepository _readOnlyUserRepository;
    private readonly IResultHandler _resultHandler;

    public GetUserByIdQueryHandler(IReadOnlyUserRepository readOnlyUserRepository, IResultHandler resultHandler)
    {
        _readOnlyUserRepository = readOnlyUserRepository;
        _resultHandler = resultHandler;
    }

    public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userResult = await _readOnlyUserRepository.FindByIdAsync(request.UserId, cancellationToken);
        await _resultHandler.HandleAsync(userResult, cancellationToken);

        if (userResult.IsFailed)
        {
            return userResult;
        }

        return userResult;
    }
}