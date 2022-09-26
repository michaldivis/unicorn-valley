using UnicornValley.Application.Users.Create;

namespace UnicornValley.WebAPI.Endpoints.Users;
public class Create : Endpoint<CreateUserCommand>
{
    private readonly IMediator _mediator;

    public Create(IMediator mediator)
    {
        _mediator = mediator;
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

        if (result.IsFailed)
        {
            await EndpointUtils.SendDomainErrorsAsync(this, result, SendAsync, cancellationToken: ct);
            return;
        }

        await SendAsync(result.Value, cancellation: ct);
    }
}