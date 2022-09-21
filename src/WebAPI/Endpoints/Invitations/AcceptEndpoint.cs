using FastEndpoints;
using MediatR;
using UnicornValley.Application.Invitations.Accept;

namespace UnicornValley.WebAPI.Endpoints.Invitations;

public class AcceptEndpoint : Endpoint<AcceptInvitationCommand>
{
    private readonly IMediator _mediator;

    public AcceptEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/invitations/accept");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AcceptInvitationCommand req, CancellationToken ct)
    {
        var result = await _mediator.Send(req, ct);
        //TODO turn result into response
        await SendAsync(result, cancellation: ct);
    }
}