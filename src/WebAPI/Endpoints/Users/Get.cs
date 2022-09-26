using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
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

            var errorResponse = CreateProblemDetails(this, error);

            await SendAsync(errorResponse, (int)HttpStatusCode.BadRequest, ct);

            return;
        }

        await SendAsync(user, cancellation: ct);
    }

    public static ProblemDetails CreateProblemDetails(IEndpoint endpoint, DomainError error, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        var problemDetails = new ProblemDetails
        {
            Type = $"/errors/{error.Code.Replace('.', '-').ToLower()}",
            Title = error.Title,
            Detail = error.Message,
            Instance = endpoint.HttpContext.Request.Path,
            Status = (int)httpStatusCode
        };

        foreach (var item in error.Metadata)
        {
            problemDetails.Extensions.Add(item);
        }

        return problemDetails;
    }
}