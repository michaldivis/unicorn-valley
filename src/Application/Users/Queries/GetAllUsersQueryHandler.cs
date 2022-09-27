namespace UnicornValley.Application.Users.Queries;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<User>>
{
    private readonly IReadOnlyUserRepository _readOnlyUserRepository;

    public GetAllUsersQueryHandler(IReadOnlyUserRepository readOnlyUserRepository)
    {
        _readOnlyUserRepository = readOnlyUserRepository;
    }

    public async Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _readOnlyUserRepository.GetAllAsync(cancellationToken);
        return users;
    }
}
