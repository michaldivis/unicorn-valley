using Microsoft.EntityFrameworkCore;

namespace UnicornValley.WebAPI.Endpoints.Invitations;

public class GetAll : EndpointWithoutRequest
{
    private readonly AppDbContext _db;

    public GetAll(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/invitations/all");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get all invitations";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var invitations = await _db.Invitations
            .AsNoTracking()
            .IncludeAll()
            .ToListAsync(ct);

        await SendAsync(invitations, cancellation: ct);
    }
}