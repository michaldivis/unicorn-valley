using System.Net.Mail;
using System.Text.Json.Serialization;
using ValueOf;

namespace UnicornValley.Domain.ValueObjects;

[JsonConverter(typeof(UsernameJsonConverter))]
public class Username : ValueOf<string, Username>
{
    protected override void Validate()
    {
        var isValid = MailAddress.TryCreate(Value, out var _);

        if (!isValid)
        {
            throw new ArgumentException("Value is not a valid e-mail address", nameof(Value));
        }
    }

    protected override bool TryValidate()
    {
        return MailAddress.TryCreate(Value, out var _);
    }
}