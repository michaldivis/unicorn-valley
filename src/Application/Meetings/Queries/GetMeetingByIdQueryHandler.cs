namespace UnicornValley.Application.Meetings.Queries;

public class GetMeetingByIdQueryHandler : IRequestHandler<GetMeetingByIdQuery, Result<Meeting>>
{
    private readonly IReadOnlyMeetingRepository _readOnlyMeetingRepository;
    private readonly IResultHandler _resultHandler;

    public GetMeetingByIdQueryHandler(IReadOnlyMeetingRepository readOnlyMeetingRepository, IResultHandler resultHandler)
    {
        _readOnlyMeetingRepository = readOnlyMeetingRepository;
        _resultHandler = resultHandler;
    }

    public async Task<Result<Meeting>> Handle(GetMeetingByIdQuery request, CancellationToken cancellationToken)
    {
        var userResult = await _readOnlyMeetingRepository.FindByIdAsync(request.MeetingId, cancellationToken);
        await _resultHandler.HandleAsync(userResult, cancellationToken);

        if (userResult.IsFailed)
        {
            return userResult;
        }

        return userResult;
    }
}