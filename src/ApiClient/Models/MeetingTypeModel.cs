using System.Text.Json.Serialization;

namespace ApiClient.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MeetingTypeModel
{
    WithLimitedNumberOfAttendees = 0,
    WithExpirationForInvitations = 1
}
