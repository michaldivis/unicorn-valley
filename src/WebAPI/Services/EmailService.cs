using FluentResults;
using UnicornValley.Domain.Abstractions;
using UnicornValley.Domain.Entities;

namespace UnicornValley.WebAPI.Services;

public class EmailService : IEmailService
{
    public Task<Result> SendInvitationAcceptedAsync(Meeting meeting, CancellationToken cancellationToken)
    {
        //TODO send email
        return Task.FromResult(Result.Ok());
    }
}