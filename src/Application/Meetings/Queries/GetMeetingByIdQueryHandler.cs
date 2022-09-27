namespace UnicornValley.Application.Meetings.Queries;

public class GetMeetingByIdQueryHandler : IRequestHandler<GetMeetingByIdQuery, Result<Meeting>>
{
    private readonly IReadOnlyMeetingRepository _readOnlyMeetingRepository;
    private readonly IErrorHandler _errorHandler;

    public GetMeetingByIdQueryHandler(IReadOnlyMeetingRepository readOnlyMeetingRepository, IErrorHandler errorHandler)
    {
        _readOnlyMeetingRepository = readOnlyMeetingRepository;
        _errorHandler = errorHandler;
    }

    public async Task<Result<Meeting>> Handle(GetMeetingByIdQuery request, CancellationToken cancellationToken)
    {
        var userResult = await _readOnlyMeetingRepository.FindByIdAsync(request.MeetingId, cancellationToken);

        if (userResult.IsFailed)
        {
            await _errorHandler.HandleAsync(userResult, cancellationToken);
            return userResult;
        }

        return userResult;
    }
}