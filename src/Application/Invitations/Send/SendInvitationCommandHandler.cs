namespace UnicornValley.Application.Invitations.Send;

public class SendInvitationCommandHandler : IRequestHandler<SendInvitationCommand>
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

    public async Task<Unit> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailed)
        {
            await _errorHandler.HandleAsync(userResult, cancellationToken);
            return Unit.Value;
        }

        var meetingResult = await _meetingRepository.FindByIdAsync(request.MeetingId, cancellationToken);
        if (meetingResult.IsFailed)
        {
            await _errorHandler.HandleAsync(meetingResult, cancellationToken);
            return Unit.Value;
        }

        var invitationResult = meetingResult.Value.SendInvitation(userResult.Value);
        if (invitationResult.IsFailed)
        {
            await _errorHandler.HandleAsync(invitationResult, cancellationToken);
            return Unit.Value;
        }

        _invitationRepository.Add(invitationResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        //TODO send notification

        return Unit.Value;
    }
}
