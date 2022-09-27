namespace UnicornValley.Application.Meetings.Queries;

public class GetAllMeetingsQueryHandler : IRequestHandler<GetAllMeetingsQuery, List<Meeting>>
{
    private readonly IReadOnlyMeetingRepository _readOnlyMeetingRepository;

    public GetAllMeetingsQueryHandler(IReadOnlyMeetingRepository readOnlyUserRepository)
    {
        _readOnlyMeetingRepository = readOnlyUserRepository;
    }

    public async Task<List<Meeting>> Handle(GetAllMeetingsQuery request, CancellationToken cancellationToken)
    {
        var meetings = await _readOnlyMeetingRepository.GetAllAsync(cancellationToken);
        return meetings;
    }
}
