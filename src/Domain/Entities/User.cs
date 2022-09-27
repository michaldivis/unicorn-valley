using UnicornValley.Domain.Errors;

namespace UnicornValley.Domain.Entities;

public class User : AggregateRoot
{
    public override Guid Id { get; protected set; }
    public Username Username { get; private set; }

    public User(Guid id, Username username)
    {
        Id = id;
        Username = username;
    }

    [Obsolete("To be used by EF Core only")]
    public User()
    {
    }

    public static Result<User> Create(Guid id, Username username, bool isUsernameUnique)
    {
        if (!isUsernameUnique)
        {
            return Result.Fail(DomainErrors.User.UsernameAlreadyExists(username.Value));
        }

        var user = new User(id, username);

        return user;
    }
}
