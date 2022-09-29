using System.Text.Json.Serialization;

namespace ApiClient;

public class ApiValidationError
{
    [JsonPropertyName("property")]
    public string? Property { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("value")]
    public object? Value { get; set; }
}