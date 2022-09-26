using Microsoft.EntityFrameworkCore;
using UnicornValley.Domain.Common;
using UnicornValley.Domain.Entities;

namespace UnicornValley.WebAPI.Endpoints.Invitations;

public class Get : EndpointWithoutRequest
{
    private readonly AppDbContext _db;
    private readonly ILogger<Get> _logger;

    public Get(AppDbContext db, ILogger<Get> logger)
    {
        _db = db;
        _logger = logger;
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
            this.HandleError(_logger, DomainErrors.Common.NotFoundById<Invitation>(invitationId));
            return;
        }

        await SendAsync(invitation, cancellation: ct);
    }
}