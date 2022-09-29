using FluentResults;
using UnicornValley.Domain.Errors;
using UnicornValley.WebAPI.Utils;

namespace UnicornValley.WebAPI.Endpoints.Demo.Errors;

public class SingleErrorResult : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/errors/single-error-result");
        AllowAnonymous();
        Summary(s => s.Summary = "Return a single error from a result");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = Result.Fail(DomainErrors.User.InvalidUsername("sigma115"));

        await EndpointUtils.SendDomainErrorsAsync(this, result, SendAsync, cancellationToken: ct);
    }
}
