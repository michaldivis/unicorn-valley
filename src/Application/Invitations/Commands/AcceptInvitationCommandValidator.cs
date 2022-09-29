using FluentValidation;

namespace UnicornValley.Application.Invitations.Commands;

public class AcceptInvitationCommandValidator : AbstractValidator<AcceptInvitationCommand>
{
    public AcceptInvitationCommandValidator()
    {
        RuleFor(a => a.InvitationId)
            .NotEmpty().WithMessage("{PropertyName} has to be a valid GUID");
    }
}
