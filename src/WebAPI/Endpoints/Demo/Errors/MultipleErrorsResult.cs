using FluentResults;
using UnicornValley.Domain.Entities;
using UnicornValley.Domain.Errors;
using UnicornValley.WebAPI.Utils;

namespace UnicornValley.WebAPI.Endpoints.Demo.Errors;

public class MultipleErrorsResult : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/errors/multiple-errors-result");
        AllowAnonymous();
        Summary(s => s.Summary = "Return multiple errors from a result");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = Result.Fail(new[] {
            DomainErrors.User.InvalidUsername("sigma115"),
            DomainErrors.Meeting.AlreadyPassed(DateTime.UtcNow.AddMonths(-1)),
            DomainErrors.Common.NotFoundById(typeof(Invitation), Guid.NewGuid())
        });

        await EndpointUtils.SendDomainErrorsAsync(this, result, SendAsync, cancellationToken: ct);
    }
}