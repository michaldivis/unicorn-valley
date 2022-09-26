using UnicornValley.Application.Invitations.Accept;

namespace UnicornValley.WebAPI.Endpoints.Invitations;

public class Accept : Endpoint<AcceptInvitationCommand>
{
    private readonly IMediator _mediator;
    private readonly ILogger<Accept> _logger;

    public Accept(IMediator mediator, ILogger<Accept> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override void Configure()
    {
        Post("/invitations/accept");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Accept a meeting invitation";
        });
    }

    public override async Task HandleAsync(AcceptInvitationCommand req, CancellationToken ct)
    {
        var result = await _mediator.Send(req, ct);

        if (result.IsSuccess)
        {
            await SendAsync(result.Value, cancellation: ct);
            return;
        }

        this.HandleErrors(_logger, result);
    }
}