using UnicornValley.Domain.Errors;

namespace UnicornValley.Application.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<User>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IResultHandler _resultHandler;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IResultHandler resultHandler)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _resultHandler = resultHandler;
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
        await _resultHandler.HandleAsync(userResult, cancellationToken);

        if (userResult.IsFailed)
        {
            return userResult;
        }

        _userRepository.Add(userResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return userResult;
    }
}
