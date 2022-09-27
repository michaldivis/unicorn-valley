namespace UnicornValley.Application.Invitations.Commands;

public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, Result<Attendee>>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IInvitationRepository _invitationRepository;
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IResultHandler _resultHandler;

    public AcceptInvitationCommandHandler(IMeetingRepository meetingRepository, IInvitationRepository invitationRepository, IAttendeeRepository attendeeRepository, IUnitOfWork unitOfWork, IResultHandler resultHandler)
    {
        _meetingRepository = meetingRepository;
        _invitationRepository = invitationRepository;
        _attendeeRepository = attendeeRepository;
        _unitOfWork = unitOfWork;
        _resultHandler = resultHandler;
    }

    public async Task<Result<Attendee>> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        var invitationResult = await _invitationRepository.FindByIdAsync(request.InvitationId, cancellationToken);
        await _resultHandler.HandleAsync(invitationResult, cancellationToken);

        if (invitationResult.IsFailed)
        {
            return invitationResult.ToResult<Attendee>();
        }

        var meetingResult = await _meetingRepository.FindByIdAsync(invitationResult.Value.MeetingId, cancellationToken);
        await _resultHandler.HandleAsync(meetingResult, cancellationToken);

        if (meetingResult.IsFailed)
        {
            return meetingResult.ToResult<Attendee>();
        }

        var acceptResult = meetingResult.Value.AcceptInvitation(invitationResult.Value);
        await _resultHandler.HandleAsync(acceptResult, cancellationToken);

        if (acceptResult.IsFailed)
        {
            return acceptResult;
        }

        _attendeeRepository.Add(acceptResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return acceptResult;
    }
}
