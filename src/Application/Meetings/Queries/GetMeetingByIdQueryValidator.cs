using FluentValidation;

namespace UnicornValley.Application.Meetings.Queries;

public class GetMeetingByIdQueryValidator : AbstractValidator<GetMeetingByIdQuery>
{
    public GetMeetingByIdQueryValidator()
    {
        RuleFor(a => a.MeetingId)
            .NotEmpty().WithMessage("{PropertyName} has to be a valid GUID");
    }
}