using Microsoft.EntityFrameworkCore;
using UnicornValley.Domain.Common;

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
        Summary(s =>
        {
            s.Summary = "Get a single invitation";
        });
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
            this.HandleError(DomainErrors.Common.NotFoundById);
            return;
        }

        await SendAsync(invitation, cancellation: ct);
    }
}