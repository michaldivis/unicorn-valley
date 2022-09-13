namespace UnicornValley.Application.Invitations.Accept;
public class InvitationAcceptedHandler : INotificationHandler<InvitationAccepted>
{
    private readonly IEmailService _emailService;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IErrorHandler _errorHandler;

    public InvitationAcceptedHandler(IEmailService emailService, IMeetingRepository meetingRepository, IErrorHandler errorHandler)
    {
        _emailService = emailService;
        _meetingRepository = meetingRepository;
        _errorHandler = errorHandler;
    }

    public async Task Handle(InvitationAccepted notification, CancellationToken cancellationToken)
    {
        var meetingResult = await _meetingRepository.FindByIdAsync(notification.MeetingId, cancellationToken);
        if (meetingResult.IsFailed)
        {
            await _errorHandler.HandleAsync(meetingResult, cancellationToken);
            return;
        }

        var emailResult = await _emailService.SendInvitationAcceptedAsync(meetingResult.Value, cancellationToken);
        if (emailResult.IsFailed)
        {
            await _errorHandler.HandleAsync(emailResult, cancellationToken);
            return;
        }
    }
}
