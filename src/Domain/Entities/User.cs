namespace UnicornValley.Domain.Entities;

public class User : Entity
{
    public Username Username { get; private set; }

    public User(Guid id, Username username) : base(id)
    {
        Username = username;
    }
}
