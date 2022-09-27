namespace UnicornValley.Application.Invitations.Commands;

public class SendInvitationCommandHandler : IRequestHandler<SendInvitationCommand, Result<Invitation>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IResultHandler _resultHandler;

    public SendInvitationCommandHandler(IUserRepository userRepository, IMeetingRepository meetingRepository, IInvitationRepository invitationRepository, IUnitOfWork unitOfWork, IResultHandler resultHandler)
    {
        _userRepository = userRepository;
        _meetingRepository = meetingRepository;
        _invitationRepository = invitationRepository;
        _unitOfWork = unitOfWork;
        _resultHandler = resultHandler;
    }

    public async Task<Result<Invitation>> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.FindByIdAsync(request.UserId, cancellationToken);
        await _resultHandler.HandleAsync(userResult, cancellationToken);

        if (userResult.IsFailed)
        {
            return userResult.ToResult<Invitation>();
        }

        var meetingResult = await _meetingRepository.FindByIdAsync(request.MeetingId, cancellationToken);
        await _resultHandler.HandleAsync(meetingResult, cancellationToken);

        if (meetingResult.IsFailed)
        {
            return meetingResult.ToResult<Invitation>();
        }

        var existingInvitation = await _invitationRepository.GetForUserAndMeeting(request.UserId, request.MeetingId);

        var invitationResult = meetingResult.Value.SendInvitation(userResult.Value, existingInvitation);
        await _resultHandler.HandleAsync(invitationResult, cancellationToken);

        if (invitationResult.IsFailed)
        {
            return invitationResult;
        }

        _invitationRepository.Add(invitationResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return invitationResult;
    }
}
