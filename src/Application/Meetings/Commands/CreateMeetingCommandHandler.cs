namespace UnicornValley.Application.Meetings.Commands;
public class CreateMeetingCommandHandler : IRequestHandler<CreateMeetingCommand, Result<Meeting>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IResultHandler _resultHandler;

    public CreateMeetingCommandHandler(IUserRepository userRepository, IMeetingRepository meetingRepository, IUnitOfWork unitOfWork, IResultHandler resultHandler)
    {
        _userRepository = userRepository;
        _meetingRepository = meetingRepository;
        _unitOfWork = unitOfWork;
        _resultHandler = resultHandler;
    }

    public async Task<Result<Meeting>> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.FindByIdAsync(request.CreatorId, cancellationToken);
        await _resultHandler.HandleAsync(userResult, cancellationToken);

        if (userResult.IsFailed)
        {
            return userResult.ToResult<Meeting>();
        }

        var meetingResult = Meeting.Create(Guid.NewGuid(), userResult.Value, request.Type, request.ScheduledAtUtc, request.Name, request.Location, request.MaximumNumberOfAttendees, request.InvitationValidBeforeInHours);
        await _resultHandler.HandleAsync(meetingResult, cancellationToken);

        if (meetingResult.IsFailed)
        {
            return meetingResult;
        }

        _meetingRepository.Add(meetingResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return meetingResult;
    }
}
