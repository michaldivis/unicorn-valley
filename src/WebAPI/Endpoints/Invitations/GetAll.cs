using UnicornValley.Application.Invitations.Queries;

namespace UnicornValley.WebAPI.Endpoints.Invitations;

public class GetAll : EndpointWithoutRequest
{
    private readonly IMediator _mediator;

    public GetAll(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/invitations/all");
        AllowAnonymous();
        Summary(s => s.Summary = "Get all invitations");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var invitations = await _mediator.Send(new GetAllInvitationsQuery(), ct);
        await SendAsync(invitations, cancellation: ct);
    }
}