namespace UnicornValley.Application.Users.Create;
public record CreateUserCommand : IRequest<Result<User>>
{
    public string Username { get; init; } = null!;
}