namespace UnicornValley.Application.Meetings.Commands;
public class CreateMeetingCommandHandler : IRequestHandler<CreateMeetingCommand, Result<Meeting>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IErrorHandler _errorHandler;

    public CreateMeetingCommandHandler(IUserRepository userRepository, IMeetingRepository meetingRepository, IUnitOfWork unitOfWork, IErrorHandler errorHandler)
    {
        _userRepository = userRepository;
        _meetingRepository = meetingRepository;
        _unitOfWork = unitOfWork;
        _errorHandler = errorHandler;
    }

    public async Task<Result<Meeting>> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.FindByIdAsync(request.CreatorId, cancellationToken);

        if (userResult.IsFailed)
        {
            await _errorHandler.HandleAsync(userResult, cancellationToken);
            return userResult.ToResult<Meeting>();
        }

        var meetingResult = Meeting.Create(Guid.NewGuid(), userResult.Value, request.Type, request.ScheduledAtUtc, request.Name, request.Location, request.MaximumNumberOfAttendees, request.InvitationValidBeforeInHours);

        if (meetingResult.IsFailed)
        {
            await _errorHandler.HandleAsync(meetingResult, cancellationToken);
            return meetingResult;
        }

        _meetingRepository.Add(meetingResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return meetingResult;
    }
}
