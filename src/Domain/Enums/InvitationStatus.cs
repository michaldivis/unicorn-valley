using System.Text.Json.Serialization;

namespace UnicornValley.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InvitationStatus
{
    Pending = 0,
    Expired = 1,
    Accepted = 2
}
