using UnicornValley.Application.Invitations.Send;

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
        Summary(s =>
        {
            s.Summary = "Send a meeting invitation to a specific user";
        });
    }

    public override async Task HandleAsync(SendInvitationCommand req, CancellationToken ct)
    {
        var result = await _mediator.Send(req, ct);

        if (result.IsSuccess)
        {
            await SendAsync(result.Value, cancellation: ct);
            return;
        }

        this.HandleErrors(result);
    }
}