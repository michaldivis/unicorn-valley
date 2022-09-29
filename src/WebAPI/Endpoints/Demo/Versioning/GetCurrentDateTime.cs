namespace UnicornValley.WebAPI.Endpoints.Demo.Versioning;

public class GetCurrentDateTime : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/versioning/get-current-date-time");
        AllowAnonymous();
        Summary(s => s.Summary = "Return the current date and time");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync($"From initial version: {DateTime.UtcNow}", cancellation: ct);
    }
}
