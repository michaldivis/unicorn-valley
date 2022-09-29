using FluentValidation;

namespace UnicornValley.Application.Meetings.Commands;

public class CreateMeetingCommandValidator : AbstractValidator<CreateMeetingCommand>
{
    public CreateMeetingCommandValidator()
    {
        RuleFor(a => a.CreatorId)
            .NotEmpty().WithMessage("{PropertyName} has to be a valid GUID");

        RuleFor(a => a.ScheduledAtUtc)
            .GreaterThan(DateTime.UtcNow).WithMessage("{PropertyName} has to be in the future");

        RuleFor(a => a.Name)
            .MinimumLength(3).WithMessage("{PropertyName} has to be at least 3 characters long");

        RuleFor(a => a.Location)
            .MinimumLength(3).WithMessage("{PropertyName} has to be at least 3 characters long");

        When(a => a.Type == MeetingType.WithLimitedNumberOfAttendees, () =>
        {
            RuleFor(a => a.MaximumNumberOfAttendees)
                .NotNull().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("{PropertyName} has to be greater than 0");
        });

        When(a => a.Type == MeetingType.WithExpirationForInvitations, () =>
        {
            RuleFor(a => a.InvitationValidBeforeInHours)
                .NotNull().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("{PropertyName} has to be greater than 0");
        });
    }
}
