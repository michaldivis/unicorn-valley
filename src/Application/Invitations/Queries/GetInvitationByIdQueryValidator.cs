using FluentValidation;

namespace UnicornValley.Application.Invitations.Queries;

public class GetInvitationByIdQueryValidator : AbstractValidator<GetInvitationByIdQuery>
{
    public GetInvitationByIdQueryValidator()
    {
        RuleFor(a => a.InvitationId)
            .NotEmpty().WithMessage("{PropertyName} has to be a valid GUID");
    }
}