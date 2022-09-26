namespace UnicornValley.Application.Users.Commands;
public record CreateUserCommand : IRequest<Result<User>>
{
    public string Username { get; init; } = null!;
}