using UnicornValley.Application.Meetings.Queries;
using UnicornValley.WebAPI.Utils;

namespace UnicornValley.WebAPI.Endpoints.Meetings;

public class Get : EndpointWithoutRequest
{
    private readonly IMediator _mediator;

    public Get(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/meetings/{MeetingId}");
        AllowAnonymous();
        Summary(s => s.Summary = "Get a single meeting");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var meetingId = Route<Guid>("MeetingId");

        var result = await _mediator.Send(new GetMeetingByIdQuery { MeetingId = meetingId }, ct);

        if (result.IsFailed)
        {
            await EndpointUtils.SendDomainErrorsAsync(this, result, SendAsync, cancellationToken: ct);
            return;
        }

        await SendAsync(result.Value, cancellation: ct);
    }
}