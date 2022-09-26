namespace UnicornValley.Application.Users.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<User>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IErrorHandler _errorHandler;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IErrorHandler errorHandler)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _errorHandler = errorHandler;
    }

    public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var isUsernameValid = Username.TryFrom(request.Username, out var username);
        if (!isUsernameValid)
        {
            return Result.Fail(DomainErrors.User.InvalidUsername(request.Username));
        }

        var isUsernameUnique = await _userRepository.IsUsernameUniqueAsync(username);

        var userResult = User.Create(Guid.NewGuid(), username, isUsernameUnique);

        if (userResult.IsFailed)
        {
            await _errorHandler.HandleAsync(userResult, cancellationToken);
            return userResult;
        }

        _userRepository.Add(userResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return userResult;
    }
}
