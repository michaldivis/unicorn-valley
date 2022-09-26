namespace UnicornValley.Application.Meetings.Queries;

public record GetAllMeetingsQuery : IRequest<List<Meeting>>
{
}