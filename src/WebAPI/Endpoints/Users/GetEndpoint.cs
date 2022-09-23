using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using UnicornValley.Domain.Common;
using UnicornValley.Infrastructure;

namespace UnicornValley.WebAPI.Endpoints.Users;

public class GetEndpoint : EndpointWithoutRequest
{
    private readonly AppDbContext _db;

    public GetEndpoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/users/{UserId}");
        AllowAnonymous();
        Summary(s => {
            s.Summary = "Get a single user";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = Route<Guid>("UserId");

        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == userId, ct);

        if (user is null)
        {
            AddError(DomainErrors.Common.NotFoundById.Message, DomainErrors.Common.NotFoundById.Code);
            ThrowIfAnyErrors();
            return;
        }

        await SendAsync(user, cancellation: ct);
    }
}