using System.Text.Json;
using System.Text.Json.Serialization;

namespace UnicornValley.Domain.ValueObjects;

public class UsernameJsonConverter : JsonConverter<Username>
{
    public override Username? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var success = Username.TryFrom(reader.GetString()!, out var username);
        return success ? username : null;
    }

    public override void Write(Utf8JsonWriter writer, Username value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}
