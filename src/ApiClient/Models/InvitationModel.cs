namespace ApiClient.Models;

public class InvitationModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid MeetingId { get; set; }
    public InvitationStatusModel Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? LastModifiedAtUtc { get; set; }
}
