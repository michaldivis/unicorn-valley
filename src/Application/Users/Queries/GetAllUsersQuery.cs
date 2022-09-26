namespace UnicornValley.Application.Users.Queries;

public record GetAllUsersQuery : IRequest<List<User>>
{
}