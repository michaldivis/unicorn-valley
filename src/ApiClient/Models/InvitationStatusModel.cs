using System.Text.Json.Serialization;

namespace ApiClient.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InvitationStatusModel
{
    Pending = 0,
    Expired = 1,
    Accepted = 2
}
