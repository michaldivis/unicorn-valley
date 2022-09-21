using FastEndpoints;
using MediatR;
using UnicornValley.Application.Meetings.Create;

namespace UnicornValley.WebAPI.Endpoints.Meetings;
public class CreateEndpoint : Endpoint<CreateMeetingCommand>
{
    private readonly IMediator _mediator;

    public CreateEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/meetings/create");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateMeetingCommand req, CancellationToken ct)
    {
        var result = await _mediator.Send(req, ct);
        //TODO turn result into response
        await SendAsync(result, cancellation: ct);
    }
}