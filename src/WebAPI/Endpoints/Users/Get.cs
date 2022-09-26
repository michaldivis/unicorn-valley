using UnicornValley.Application.Users.Queries;

namespace UnicornValley.WebAPI.Endpoints.Users;

public class Get : EndpointWithoutRequest
{
    private readonly IMediator _mediator;

    public Get(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/users/{UserId}");
        AllowAnonymous();
        Summary(s => s.Summary = "Get a single user");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = Route<Guid>("UserId");

        var result = await _mediator.Send(new GetUserByIdQuery { UserId = userId }, ct);

        if (result.IsFailed)
        {
            await EndpointUtils.SendDomainErrorsAsync(this, result, SendAsync, cancellationToken: ct);
            return;
        }

        await SendAsync(result.Value, cancellation: ct);
    }
}