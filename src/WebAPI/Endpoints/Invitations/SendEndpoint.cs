using FastEndpoints;
using MediatR;
using UnicornValley.Application.Invitations.Send;

namespace UnicornValley.WebAPI.Endpoints.Invitations;
public class SendEndpoint : Endpoint<SendInvitationCommand>
{
    private readonly IMediator _mediator;

    public SendEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/invitations/send");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SendInvitationCommand req, CancellationToken ct)
    {
        var result = await _mediator.Send(req, ct);
        //TODO turn result into response
        await SendAsync(result, cancellation: ct);
    }
}