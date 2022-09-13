namespace UnicornValley.Application.Meetings.Create;
public record CreateMeetingCommand(Guid CreatorId, MeetingType Type, DateTime ScheduledAtUtc, string Name, string Location, int? MaximumNumberOfAttendees, int? InvitationValidBeforeInHours) : IRequest;
