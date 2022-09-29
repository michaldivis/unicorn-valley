namespace UnicornValley.Application.Meetings.Queries;
public record GetMeetingByIdQuery : IRequest<Result<Meeting>>
{
    public Guid MeetingId { get; init; }
}
