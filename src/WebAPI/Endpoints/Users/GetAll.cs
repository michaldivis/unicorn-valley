using UnicornValley.Application.Users.Queries;

namespace UnicornValley.WebAPI.Endpoints.Users;

public class GetAll : EndpointWithoutRequest
{
    private readonly IMediator _mediator;

    public GetAll(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/users/all");
        AllowAnonymous();
        Summary(s => s.Summary = "Get all users");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var users = await _mediator.Send(new GetAllUsersQuery(), ct);
        await SendAsync(users, cancellation: ct);
    }
}