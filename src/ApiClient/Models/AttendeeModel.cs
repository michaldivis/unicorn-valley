namespace ApiClient.Models;

public class AttendeeModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid MeetingId { get; set; }
    public DateTime JoinedAtUtc { get; set; }
}