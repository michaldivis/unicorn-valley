﻿namespace UnicornValley.Application.Invitations.Commands;

public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, Result<Attendee>>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IInvitationRepository _invitationRepository;
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IErrorHandler _errorHandler;

    public AcceptInvitationCommandHandler(IMeetingRepository meetingRepository, IInvitationRepository invitationRepository, IAttendeeRepository attendeeRepository, IUnitOfWork unitOfWork, IErrorHandler errorHandler)
    {
        _meetingRepository = meetingRepository;
        _invitationRepository = invitationRepository;
        _attendeeRepository = attendeeRepository;
        _unitOfWork = unitOfWork;
        _errorHandler = errorHandler;
    }

    public async Task<Result<Attendee>> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        var invitationResult = await _invitationRepository.FindByIdAsync(request.InvitationId, cancellationToken);
        if (invitationResult.IsFailed)
        {
            await _errorHandler.HandleAsync(invitationResult, cancellationToken);
            return invitationResult.ToResult<Attendee>();
        }

        var meetingResult = await _meetingRepository.FindByIdAsync(invitationResult.Value.MeetingId, cancellationToken);
        if (meetingResult.IsFailed)
        {
            await _errorHandler.HandleAsync(meetingResult, cancellationToken);
            return meetingResult.ToResult<Attendee>();
        }

        var acceptResult = meetingResult.Value.AcceptInvitation(invitationResult.Value);
        if (acceptResult.IsFailed)
        {
            await _errorHandler.HandleAsync(acceptResult, cancellationToken);
            return acceptResult;
        }

        _attendeeRepository.Add(acceptResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return acceptResult;
    }
}
