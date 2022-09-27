namespace UnicornValley.Application.Invitations.Notifications;
public class InvitationAcceptedNotificationHandler : INotificationHandler<InvitationAccepted>
{
    private readonly IEmailService _emailService;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IResultHandler _resultHandler;

    public InvitationAcceptedNotificationHandler(IEmailService emailService, IMeetingRepository meetingRepository, IResultHandler resultHandler)
    {
        _emailService = emailService;
        _meetingRepository = meetingRepository;
        _resultHandler = resultHandler;
    }

    public async Task Handle(InvitationAccepted notification, CancellationToken cancellationToken)
    {
        var meetingResult = await _meetingRepository.FindByIdAsync(notification.MeetingId, cancellationToken);
        await _resultHandler.HandleAsync(meetingResult, cancellationToken);

        if (meetingResult.IsFailed)
        {
            return;
        }

        var emailResult = await _emailService.SendInvitationAcceptedAsync(meetingResult.Value, cancellationToken);
        await _resultHandler.HandleAsync(emailResult, cancellationToken);

        if (emailResult.IsFailed)
        {
            return;
        }
    }
}
