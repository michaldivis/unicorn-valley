using FluentValidation;

namespace UnicornValley.Application.Invitations.Commands;

public class SendInvitationCommandValidator : AbstractValidator<SendInvitationCommand>
{
    public SendInvitationCommandValidator()
    {
        RuleFor(a => a.UserId)
            .NotEmpty().WithMessage("{PropertyName} has to be a valid GUID");

        RuleFor(a => a.MeetingId)
            .NotEmpty().WithMessage("{PropertyName} has to be a valid GUID");
    }
}