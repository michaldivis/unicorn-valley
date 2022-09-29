namespace UnicornValley.WebAPI.Endpoints.Demo.Versioning;

public class GetCurrentDateTimeV2 : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/versioning/get-current-date-time");
        AllowAnonymous();
        Summary(s => s.Summary = "Return the current date and time");
        Version(2);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync($"From v2.0: {DateTime.UtcNow}", cancellation: ct);
    }
}
