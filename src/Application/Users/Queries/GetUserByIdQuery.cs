namespace UnicornValley.Application.Users.Queries;
public record GetUserByIdQuery : IRequest<Result<User>>
{
    public Guid UserId { get; init; }
}