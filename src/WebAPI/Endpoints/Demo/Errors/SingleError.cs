using UnicornValley.Domain.Errors;
using UnicornValley.WebAPI.Utils;

namespace UnicornValley.WebAPI.Endpoints.Demo.Errors;

public class SingleError : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/errors/single-error");
        AllowAnonymous();
        Summary(s => s.Summary = "Return a single error without result");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var error = DomainErrors.Meeting.InvitationAlreadyExists(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        await EndpointUtils.SendDomainErrorsAsync(this, error, SendAsync, cancellationToken: ct);
    }
}