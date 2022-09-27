namespace ApiClient.Models;

public class MeetingModel
{
    public Guid Id { get; set; }
    public UserModel Creator { get; set; } = null!;
    public MeetingTypeModel Type { get; set; }
    public DateTime ScheduledAtUtc { get; set; }
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;

    public int? MaximumNumberOfAttendees { get; set; }
    public DateTime? InvitationsExpireAtUtc { get; set; }
    public int NumberOfAttendees { get; set; }

    public List<AttendeeModel> Attendees { get; set; } = new();
    public List<InvitationModel> Invitations { get; set; } = new();
}