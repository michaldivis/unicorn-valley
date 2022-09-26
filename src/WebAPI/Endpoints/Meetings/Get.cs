using Microsoft.EntityFrameworkCore;
using UnicornValley.Domain.Common;
using UnicornValley.Domain.Entities;

namespace UnicornValley.WebAPI.Endpoints.Meetings;

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
        Get("/meetings/{MeetingId}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get a single meeting";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var meetingId = Route<Guid>("MeetingId");

        var meeting = await _db.Meetings
            .AsNoTracking()
            .IncludeAll()
            .FirstOrDefaultAsync(a => a.Id == meetingId, ct);

        if (meeting is null)
        {
            this.HandleError(_logger, DomainErrors.Common.NotFoundById<Meeting>(meetingId));
            return;
        }

        await SendAsync(meeting, cancellation: ct);
    }
}