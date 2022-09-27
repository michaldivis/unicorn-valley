using System.Text.Json.Serialization;

namespace UnicornValley.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MeetingType
{
    WithLimitedNumberOfAttendees = 0,
    WithExpirationForInvitations = 1
}
