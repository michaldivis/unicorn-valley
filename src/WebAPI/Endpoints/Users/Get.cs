using Microsoft.EntityFrameworkCore;
using UnicornValley.Domain.Common;
using UnicornValley.Domain.Entities;

namespace UnicornValley.WebAPI.Endpoints.Users;

public class Get : EndpointWithoutRequest
{
    private readonly AppDbContext _db;

    public Get(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/users/{UserId}");
        AllowAnonymous();
        Summary(s => s.Summary = "Get a single user");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = Route<Guid>("UserId");

        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == userId, ct);

        if (user is null)
        {
            var error = DomainErrors.Common.NotFoundById<User>(userId);
            await EndpointUtils.SendDomainErrorsAsync(this, error, SendAsync, cancellationToken: ct);
            return;
        }

        await SendAsync(user, cancellation: ct);
    }
}