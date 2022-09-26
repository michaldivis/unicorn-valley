namespace UnicornValley.Application.Invitations.Send;

public class SendInvitationCommandHandler : IRequestHandler<SendInvitationCommand, Result<Invitation>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IErrorHandler _errorHandler;

    public SendInvitationCommandHandler(IUserRepository userRepository, IMeetingRepository meetingRepository, IInvitationRepository invitationRepository, IUnitOfWork unitOfWork, IErrorHandler errorHandler)
    {
        _userRepository = userRepository;
        _meetingRepository = meetingRepository;
        _invitationRepository = invitationRepository;
        _unitOfWork = unitOfWork;
        _errorHandler = errorHandler;
    }

    public async Task<Result<Invitation>> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailed)
        {
            await _errorHandler.HandleAsync(userResult, cancellationToken);
            return userResult.ToResult<Invitation>();
        }

        var meetingResult = await _meetingRepository.FindByIdAsync(request.MeetingId, cancellationToken);
        if (meetingResult.IsFailed)
        {
            await _errorHandler.HandleAsync(meetingResult, cancellationToken);
            return meetingResult.ToResult<Invitation>();
        }

        var existingInvitation = await _invitationRepository.GetForUserAndMeeting(request.UserId, request.MeetingId);

        var invitationResult = meetingResult.Value.SendInvitation(userResult.Value, existingInvitation);
        if (invitationResult.IsFailed)
        {
            await _errorHandler.HandleAsync(invitationResult, cancellationToken);
            return invitationResult;
        }

        _invitationRepository.Add(invitationResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return invitationResult;
    }
}
