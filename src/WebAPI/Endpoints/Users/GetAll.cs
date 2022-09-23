using Microsoft.EntityFrameworkCore;

namespace UnicornValley.WebAPI.Endpoints.Users;

public class GetAll : EndpointWithoutRequest
{
    private readonly AppDbContext _db;

    public GetAll(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/users/all");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get all users";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var users = await _db.Users
            .AsNoTracking()
            .ToListAsync(ct);

        await SendAsync(users, cancellation: ct);
    }
}