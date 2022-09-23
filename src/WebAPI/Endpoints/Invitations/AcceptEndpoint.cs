using FastEndpoints;
using MediatR;
using UnicornValley.Application.Invitations.Accept;
using UnicornValley.Application.Invitations.Send;

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
        Summary(s => {
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

        EndpointUtils.HandleErrorResult(result, ThrowIfAnyErrors, AddError);
    }
}