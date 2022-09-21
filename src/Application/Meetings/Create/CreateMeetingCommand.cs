namespace UnicornValley.Application.Meetings.Create;
public record CreateMeetingCommand : IRequest<Result<Meeting>>
{
    public Guid CreatorId { get; init; }
    public MeetingType Type { get; init; }
    public DateTime ScheduledAtUtc { get; init; }
    public string Name { get; init; } = null!;
    public string Location { get; init; } = null!;
    public int? MaximumNumberOfAttendees { get; init; }
    public int? InvitationValidBeforeInHours { get; init; }
}
