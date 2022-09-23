using Microsoft.EntityFrameworkCore;

namespace UnicornValley.WebAPI.Endpoints.Meetings;

public class GetAll : EndpointWithoutRequest
{
    private readonly AppDbContext _db;

    public GetAll(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/meetings/all");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get all meetings";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var meetings = await _db.Meetings
            .AsNoTracking()
            .IncludeAll()
            .ToListAsync(ct);

        await SendAsync(meetings, cancellation: ct);
    }
}