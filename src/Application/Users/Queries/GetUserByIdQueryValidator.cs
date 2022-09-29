using FluentValidation;

namespace UnicornValley.Application.Users.Queries;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(a => a.UserId)
            .NotEmpty().WithMessage("{PropertyName} has to be a valid GUID");
    }
}