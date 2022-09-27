using System.Text.Json.Serialization;

namespace ApiClient;
public class ApiProblemDetails
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("detail")]
    public string? Detail { get; set; }

    [JsonPropertyName("instance")]
    public string? Instance { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    [JsonPropertyName("problems")]
    public List<ApiProblemDetails>? Problems { get; set; }

    public static ApiProblemDetails Unknown(string instance, int statusCode) => new()
    {
        Type = "Unknown",
        Title = "An unknown error occured",
        Detail = "An unknown error occured",
        Instance = instance,
        Status = statusCode
    };
}
