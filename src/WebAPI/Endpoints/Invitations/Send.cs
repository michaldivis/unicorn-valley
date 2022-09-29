using UnicornValley.Application.Invitations.Commands;
using UnicornValley.WebAPI.Utils;

namespace UnicornValley.WebAPI.Endpoints.Invitations;
public class Send : Endpoint<SendInvitationCommand>
{
    private readonly IMediator _mediator;

    public Send(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/invitations/send");
        AllowAnonymous();
        Summary(s => s.Summary = "Send a meeting invitation to a specific user");
    }

    public override async Task HandleAsync(SendInvitationCommand req, CancellationToken ct)
    {
        var result = await _mediator.Send(req, ct);

        if (result.IsFailed)
        {
            await EndpointUtils.SendDomainErrorsAsync(this, result, SendAsync, cancellationToken: ct);
            return;
        }

        await SendAsync(result.Value, cancellation: ct);
    }
}