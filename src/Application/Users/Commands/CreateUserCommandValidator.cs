using FluentValidation;

namespace UnicornValley.Application.Users.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
	public CreateUserCommandValidator()
	{
		RuleFor(a => a.Username)
			.EmailAddress().WithMessage("{PropertyName} has to be a valid e-mail address");
	}
}