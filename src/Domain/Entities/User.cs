﻿namespace UnicornValley.Domain.Entities;

public class User : AggregateRoot
{
    public Username Username { get; private set; }

    public User(Guid id, Username username) : base(id)
    {
        Username = username;
    }
}
