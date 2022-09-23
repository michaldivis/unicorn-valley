using FastEndpoints;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UnicornValley.Infrastructure;

namespace UnicornValley.WebAPI.Endpoints.Users;

public class GetAllEndpoint : EndpointWithoutRequest
{
    private readonly AppDbContext _db;

    public GetAllEndpoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/users/all");
        AllowAnonymous();
        Summary(s => {
            s.Summary = "Get all users";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        //TODO paginate get all users

        var users = await _db.Users
            .AsNoTracking()
            .ToListAsync(ct);

        await SendAsync(users, cancellation: ct);
    }
}