using UnicornValley.Application.Meetings.Queries;

namespace UnicornValley.WebAPI.Endpoints.Meetings;

public class GetAll : EndpointWithoutRequest
{
    private readonly IMediator _mediator;

    public GetAll(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/meetings/all");
        AllowAnonymous();
        Summary(s => s.Summary = "Get all meetings");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var meetings = await _mediator.Send(new GetAllMeetingsQuery(), ct);
        await SendAsync(meetings, cancellation: ct);
    }
}