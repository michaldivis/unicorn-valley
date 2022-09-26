using Microsoft.EntityFrameworkCore;
using UnicornValley.Domain.Entities;
using UnicornValley.Domain.Errors;

namespace UnicornValley.WebAPI.Endpoints.Invitations;

public class Get : EndpointWithoutRequest
{
    private readonly AppDbContext _db;

    public Get(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/invitations/{InvitationId}");
        AllowAnonymous();
        Summary(s => s.Summary = "Get a single invitation");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var invitationId = Route<Guid>("InvitationId");

        var invitation = await _db.Invitations
            .AsNoTracking()
            .IncludeAll()
            .FirstOrDefaultAsync(a => a.Id == invitationId, ct);

        if (invitation is null)
        {
            var error = DomainErrors.Common.NotFoundById(typeof(Invitation), invitationId);
            await EndpointUtils.SendDomainErrorsAsync(this, error, SendAsync, cancellationToken: ct);
            return;
        }

        await SendAsync(invitation, cancellation: ct);
    }
}