using ValueOf;

namespace UnicornValley.Domain.ValueObjects;

public class Username : ValueOf<string, Username>
{
    protected override void Validate()
    {
        base.Validate();
        //TODO ensure Username is a valid email address
    }
}
