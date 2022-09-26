﻿using Microsoft.EntityFrameworkCore;
using UnicornValley.Domain.Entities;
using UnicornValley.Domain.Errors;

namespace UnicornValley.WebAPI.Endpoints.Meetings;

public class Get : EndpointWithoutRequest
{
    private readonly AppDbContext _db;

    public Get(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/meetings/{MeetingId}");
        AllowAnonymous();
        Summary(s => s.Summary = "Get a single meeting");
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
            var error = DomainErrors.Common.NotFoundById(typeof(Meeting), meetingId);
            await EndpointUtils.SendDomainErrorsAsync(this, error, SendAsync, cancellationToken: ct);
            return;
        }

        await SendAsync(meeting, cancellation: ct);
    }
}