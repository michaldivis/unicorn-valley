namespace UnicornValley.WebAPI.Endpoints.Demo.Versioning;

public class GetCurrentDateTimeV1 : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/versioning/get-current-date-time");
        AllowAnonymous();
        Summary(s => s.Summary = "Return the current date and time");
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync($"From v1.0: {DateTime.UtcNow}", cancellation: ct);
    }
}
