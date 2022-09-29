using UnicornValley.Application.Invitations.Queries;
using UnicornValley.WebAPI.Utils;

namespace UnicornValley.WebAPI.Endpoints.Invitations;

public class Get : EndpointWithoutRequest
{
    private readonly IMediator _mediator;

    public Get(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/invitations/{InvitationId}");
        AllowAnonymous();
        Summary(s => s.Summary = "Get a single invitation");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var invitationId = Route<Guid>("InvitationId");

        var result = await _mediator.Send(new GetInvitationByIdQuery { InvitationId = invitationId }, ct);

        if (result.IsFailed)
        {
            await EndpointUtils.SendDomainErrorsAsync(this, result, SendAsync, cancellationToken: ct);
            return;
        }

        await SendAsync(result.Value, cancellation: ct);
    }
}