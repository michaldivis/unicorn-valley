using UnicornValley.Application.Users.Create;

namespace UnicornValley.WebAPI.Endpoints.Users;
public class Create : Endpoint<CreateUserCommand>
{
    private readonly IMediator _mediator;
    private readonly ILogger<Create> _logger;

    public Create(IMediator mediator, ILogger<Create> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override void Configure()
    {
        Post("/users/create");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Create a user";
            s.ExampleRequest = new CreateUserCommand
            {
                Username = "deep.sleep@gmail.com"
            };
        });
    }

    public override async Task HandleAsync(CreateUserCommand req, CancellationToken ct)
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