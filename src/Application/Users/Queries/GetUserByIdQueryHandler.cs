namespace UnicornValley.Application.Users.Queries;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<User>>
{
    private readonly IReadOnlyUserRepository _readOnlyUserRepository;
    private readonly IErrorHandler _errorHandler;

    public GetUserByIdQueryHandler(IReadOnlyUserRepository readOnlyUserRepository, IErrorHandler errorHandler)
    {
        _readOnlyUserRepository = readOnlyUserRepository;
        _errorHandler = errorHandler;
    }

    public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userResult = await _readOnlyUserRepository.FindByIdAsync(request.UserId, cancellationToken);

        if (userResult.IsFailed)
        {
            await _errorHandler.HandleAsync(userResult, cancellationToken);
            return userResult;
        }

        return userResult;
    }
}